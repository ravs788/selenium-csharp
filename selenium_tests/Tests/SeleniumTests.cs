using OpenQA.Selenium.Support.UI;
using selenium_tests.Models;
using selenium_tests.SupportingFunctions;
using selenium_tests.Utilities;

namespace selenium_tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class SeleniumTests
{
    public required IWebDriver _driver;
    public required SupportFunctions functions;
    public required WebDriverWait wait;

    [TearDown]
    public void TestCleanup()
    {
        _driver.Quit();
    }

    [SetUp]
    public void TestInit()
    {
        functions = new SupportFunctions();
        
        ChromeOptions options = new ChromeOptions();
        options.AddArguments("--headless");
        _driver = new ChromeDriver();
        _driver.Navigate().GoToUrl("https://ecommerce-playground.lambdatest.io/index.php?route=common/home");
        _driver.Manage().Window.Maximize();
        _driver.Manage().Cookies.DeleteAllCookies();
        wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        {
            PollingInterval = TimeSpan.FromSeconds(2),
        };
    }
    [Test]
    [Category("Smoke")]
    public void FirstTest_CompareProducts()
    {
        var expectedProduct1 = new ProductDetails
        {
            Id = 1,
            Name = "MacBook",
            Price = "$602.00",
            Model = "Product 16",
            Brand = "Apple",
            Weight = "0.00kg",
            Quantity = "1"
        };

        var expectedProduct2 = new ProductDetails
        {
            Id = 2,
            Name = "MacBook Air",
            Price = "$1,202.00",
            Model = "Product 17",
            Brand = "Apple",
            Weight = "0.00kg",
            Quantity = "2"
        };

        functions.CompareProducts(_driver,"MacBook", expectedProduct1.Id);
        functions.CompareProducts(_driver, "MacBook Air", expectedProduct1.Id);

        var compareButton = _driver.FindElement(By.XPath("//a[@aria-label='Compare']"));
        compareButton.Click();

        functions.AssertCompareProductDetails(_driver, expectedProduct1, 1);
        functions.AssertCompareProductDetails(_driver, expectedProduct2, 2);
    }

    [Test]
    [Category("Regression")]
    public void SecondTC_PurchaseProducts()
    {
        var expectedProduct1 = new ProductDetails
        {
            Id = 1,
            Name = "HP LP3065",
            Price = "$122.00",
            Model = "Product 21",
            Brand = "Hewlett-Packard",
            Weight = "1.00kg",
            Quantity = "2"
        };
        var searchText = "HP LP3065";

        var searchInput = _driver.FindElement(By.Name("search"));
        searchInput.SendKeys(searchText);
        

        wait.Until(element => 
            _driver.FindElement(By.XPath("//div[contains(@class,'search-input-group')]/div[@class='dropdown']/ul")).Displayed);

        var autoCompleteSearch = _driver.FindElements(By.XPath("//div[contains(@class,'search-input-group')]/div[@class='dropdown']/ul[contains(@class,'dropdown-menu')]/li//a/img[@alt='" + searchText + "']"));

        if (autoCompleteSearch.Count > 0)
        {
            autoCompleteSearch[0].Click();
        }
        else
        {
            var searchButton = _driver.FindElement(By.XPath("//button[text()='Search']"));
            searchButton.Click();
        }

        var itemQuantity = _driver.FindElement(By.XPath("//div[@class='entry-row row order-3 no-gutters ']//input[@name='quantity']"));
        itemQuantity.Clear();
        itemQuantity.Click();
        itemQuantity.SendKeys(expectedProduct1.Quantity);


        var sizeSelectionDropdown = _driver.FindElements(By.XPath("//select[@class='custom-select']"));
        if (sizeSelectionDropdown.Count > 0)
        {
            var sizeSelection = sizeSelectionDropdown[0];
            var selectElement = new SelectElement(sizeSelection);
            selectElement.SelectByIndex(2);
        }

        var addToCart = _driver.FindElement(By.XPath("//div[@class='entry-row row order-3 no-gutters ']//button[text()='Add to Cart']"));
        addToCart.Click();

        wait.Until(element => _driver.FindElement(By.XPath("//a[contains(normalize-space(.),'View Cart')]")).Displayed);
        wait.Until(element => _driver.FindElement(By.XPath("//a[contains(normalize-space(.),'View Cart')]")).Enabled);

        var viewCartButton = _driver.FindElements(By.XPath("//a[normalize-space(.)='View Cart']")).Last();
        viewCartButton.Click();

        var checkoutTotal = _driver.FindElement(By.XPath("//table[@class='table table-bordered m-0']/tbody/tr/td[text()='Total:']/following-sibling::td/strong"));
        Assert.That(checkoutTotal.Text, Is.EqualTo("$244.00"));

        var checkoutButton = _driver.FindElement(By.XPath("//a[text()='Checkout']"));
        checkoutButton.Click();

        var userDetails = new UserDetails
        {
            FirstName = "Eoin",
            LastName = "Fergusson",
            Email = string.Format("{0}@{1}.com", UtilityFunctions.GenerateRandomAlphabetString(10), UtilityFunctions.GenerateRandomAlphabetString(10)),
            Telephone = "1234567890",
            Password = "Password123",
            ConfirmPassord = "Password123",
            AccountType = AccountOption.Register
        };

        var billingAddress = new BillingAddress
        {
            FirstName = "Eoin",
            LastName = "Fergusson",
            Address1 = "123 Main St",
            Address2 = "Apt 4B",
            City = "Dublin",
            PostCode = "D01 1234",
            Company = "Tech Corp",
            Country = "Ireland",
            Region = "Meath"
        };

        functions.FillUserDetails(_driver,userDetails);
        functions.FillAddressForm(_driver,billingAddress);

        checkoutTotal = _driver.FindElement(By.XPath("//table[@id='checkout-total']/tbody/tr/td[text()='Total:']/following-sibling::td/strong"));
        Assert.That(checkoutTotal.Text, Is.EqualTo("$252.00"));

        var agreeToPrivacyPolicy = _driver.FindElement(By.XPath("//input[@id='input-account-agree']/following-sibling::label"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", agreeToPrivacyPolicy);
        agreeToPrivacyPolicy.Click();

        var agreeToTerms = _driver.FindElement(By.XPath("//input[@id='input-agree']/following-sibling::label"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", agreeToTerms);
        agreeToTerms.Click();

        wait.Until(element => _driver.FindElement(By.Id("button-save")).Enabled);

        var continueButton = _driver.FindElement(By.Id("button-save"));
        //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", continueButton);
        continueButton.Click();

        wait.Until(element => _driver.FindElement(By.XPath("//h1[text()='Confirm Order']")).Displayed);
        Assert.That(_driver.Title, Is.EqualTo("Confirm Order"));

        var confirmedOrderTotal = _driver.FindElement(By.XPath("//table[@class='table table-bordered table-hover mb-0']/tfoot/tr/td/strong[text()='Total:']//parent::td//following-sibling::td"));
        Assert.That(confirmedOrderTotal.Text, Is.EqualTo("$205.00"));

    }
}
