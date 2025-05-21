using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using selenium_tests.Models;
using selenium_tests.Page_Objects;
using selenium_tests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_tests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class SeleniumPOM
    {
        public IWebDriver _driver;
        public WebDriverWait _wait;
        public BasePage basePage;
        public HomePage homePage;
        public ProductPage productPage;
        public CartPage cartPage;
        public CheckoutPage checkoutPage;
        public ConfirmationPage confirmationPage;

        [SetUp]
        public void TestInit()
        {

            basePage = new BasePage();

            _driver = basePage.SetupDriver();
            _wait = basePage.GetWait();

            homePage = new HomePage(_driver, _wait);
            productPage = new ProductPage(_driver, _wait);
            cartPage = new CartPage(_driver, _wait);
            checkoutPage = new CheckoutPage(_driver, _wait);
            confirmationPage = new ConfirmationPage(_driver, _wait);

        }

        [TearDown]
        public void TestTearDown()
        {

            basePage.TearDown();
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

            homePage.SearchProduct(expectedProduct1.Name);
            productPage.ClickCompareButton();
            //productPage.ClickClosePanelButton();

            homePage.SearchProduct(expectedProduct2.Name);
            productPage.ClickCompareButton();

            ProductDetails product = productPage.CompareProducts(expectedProduct1, 1);
            Assert.That(product.Name, Is.EqualTo(expectedProduct1.Name));
            Assert.That(product.Price, Is.EqualTo(expectedProduct1.Price));
            Assert.That(product.Model, Is.EqualTo(expectedProduct1.Model));
            Assert.That(product.Brand, Is.EqualTo(expectedProduct1.Brand));
            Assert.That(product.Weight, Is.EqualTo(expectedProduct1.Weight));

            product = productPage.CompareProducts(expectedProduct2, 2);
            Assert.That(product.Name, Is.EqualTo(expectedProduct2.Name));
            Assert.That(product.Price, Is.EqualTo(expectedProduct2.Price));
            Assert.That(product.Model, Is.EqualTo(expectedProduct2.Model));
            Assert.That(product.Brand, Is.EqualTo(expectedProduct2.Brand));
            Assert.That(product.Weight, Is.EqualTo(expectedProduct2.Weight));
        }

        [Test]
        [Category("Regression")]
        public void SecondTest_CompareProducts()
        {
            var expectedProduct = new ProductDetails
            {
                Id = 1,
                Name = "HP LP3065",
                Price = "$122.00",
                Model = "Product 21",
                Brand = "Hewlett-Packard",
                Weight = "1.00kg",
                Quantity = "2"
            };

            homePage.SearchProduct(expectedProduct.Name);
            productPage.AddProductToCart(2);

            Assert.That(cartPage.VerifyAddedItems(),Is.EqualTo("$244.00"));
            cartPage.PerformCheckout();

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
                Region = "Kildare"
            };

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

            Assert.That(checkoutPage.GetCheckoutTotal, Is.EqualTo("$252.00"));


            checkoutPage.checkout(userDetails, billingAddress);

            confirmationPage.WaitForPageLoad();
            Assert.That(confirmationPage.GetConfirmationMessage, Is.EqualTo("Confirm Order"));
            Assert.That(confirmationPage.GetConfirmedOrderTotal, Is.EqualTo("$205.00"));

        }
    }
}
