using OpenQA.Selenium.Support.UI;
using selenium_tests.Models;

namespace selenium_tests.SupportingFunctions
{
    [Parallelizable(ParallelScope.Self), TestFixture]
    public class SupportFunctions
    {
        
        public  void AssertCompareProductDetails(IWebDriver _driver, ProductDetails expectedProductDetails, int productCompareIndex)
        {
            var productName = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Product", productCompareIndex)));
            var productPrice = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Price", productCompareIndex)));
            var productModel = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Model", productCompareIndex)));
            var productBrand = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Brand", productCompareIndex)));
            var productWeight = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Weight", productCompareIndex)));

            Assert.That(productName.Text, Is.EqualTo(expectedProductDetails.Name));
            Assert.That(productPrice.Text, Is.EqualTo(expectedProductDetails.Price));
            Assert.That(productModel.Text, Is.EqualTo(expectedProductDetails.Model));
            Assert.That(productBrand.Text, Is.EqualTo(expectedProductDetails.Brand));
            Assert.That(productWeight.Text, Is.EqualTo(expectedProductDetails.Weight));
        }

        public void CompareProducts(IWebDriver _driver, string searchText, int productId)
        {
            var searchInput = _driver.FindElement(By.Name("search"));
            searchInput.SendKeys(searchText);
            Thread.Sleep(3000);

            var autoCompleteSearch = _driver.FindElements(By.XPath("//div[contains(@class,'search-input-group')]/div[@class='dropdown']/ul[contains(@class,'dropdown-menu')]/li//a/img[@alt='" + searchText + "']"));
            if (autoCompleteSearch.Count > 0)
            {
                autoCompleteSearch[0].Click();
            }
            else
            {
                var searchButton = _driver.FindElement(By.XPath("//button[@class='btn btn-default btn-lg']"));
                searchButton.Click();
            }

            var compareButton = _driver.FindElement(By.XPath("//button[normalize-space()='Compare this Product']"));
            compareButton.Click();
        }

        public  void FillAddressForm(IWebDriver _driver, BillingAddress billingAddress)
        {
            _driver.FindElement(By.Id("input-payment-company")).SendKeys(billingAddress.Company);
            _driver.FindElement(By.Id("input-payment-address-1")).SendKeys(billingAddress.Address1);
            _driver.FindElement(By.Id("input-payment-address-2")).SendKeys(billingAddress.Address2);
            _driver.FindElement(By.Id("input-payment-city")).SendKeys(billingAddress.City);
            _driver.FindElement(By.Id("input-payment-postcode")).SendKeys(billingAddress.PostCode);
            var sizeSelectionDropdown = _driver.FindElements(By.XPath("//select[@name='country_id']"));
            if (sizeSelectionDropdown.Count > 0)
            {
                var sizeSelection = sizeSelectionDropdown[0];
                var selectElement = new SelectElement(sizeSelection);
                selectElement.SelectByText(billingAddress.Country);
            }

            sizeSelectionDropdown = _driver.FindElements(By.XPath("//select[@name='zone_id']"));
            Thread.Sleep(1000);
            if (sizeSelectionDropdown.Count > 0)
            {
                var sizeSelection = sizeSelectionDropdown[0];
                var selectElement = new SelectElement(sizeSelection);
                selectElement.SelectByText(billingAddress.Region);
            }
        }

        public  void FillUserDetails(IWebDriver _driver, UserDetails userDetails)
        {
            if (userDetails.AccountType == AccountOption.Register)
            {
                _driver.FindElement(By.Id("input-payment-firstname")).SendKeys(userDetails.FirstName);
                _driver.FindElement(By.Id("input-payment-lastname")).SendKeys(userDetails.LastName);
                _driver.FindElement(By.Id("input-payment-email")).SendKeys(userDetails.Email);
                _driver.FindElement(By.Id("input-payment-telephone")).SendKeys(userDetails.Telephone);

                _driver.FindElement(By.Id("input-payment-password")).SendKeys(userDetails.Password);
                _driver.FindElement(By.Id("input-payment-confirm")).SendKeys(userDetails.ConfirmPassord);
            }
        }

        public  string GetCompareProductDetailsCellXPath(string cellName, int productCompareIndex)
        {
            return $"//table/tbody/tr/td[text()='{cellName}']/following-sibling::td[{productCompareIndex}]";
        }
    }
}