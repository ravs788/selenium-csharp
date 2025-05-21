using OpenQA.Selenium.Support.UI;
using selenium_tests.Models;
using selenium_tests.SupportingFunctions;
using selenium_tests.Utilities;

namespace selenium_tests.Page_Objects
{
    public class ProductPage : BasePage
    {
        public ProductPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }
        IWebElement closePanelButton => _driver.FindElement(By.XPath("//div[@class='toast-header']/button"));
        IWebElement compareButton => _driver.FindElement(By.XPath("//button[normalize-space()='Compare this Product']"));        
        IWebElement compareProductsButton => _driver.FindElement(By.XPath("//a[@aria-label='Compare']"));                
        IWebElement itemQuantity => _driver.FindElement(By.XPath("//div[@class='entry-row row order-3 no-gutters ']//input[@name='quantity']"));
        IList<IWebElement> sizeSelectionDropdown => _driver.FindElements(By.XPath("//select[@class='custom-select']"));

        IWebElement addToCart => _driver.FindElement(By.XPath("//div[@class='entry-row row order-3 no-gutters ']//button[text()='Add to Cart']"));
        
        public void ClickCompareButton()
        {
            compareButton.Click();
        }

        public void ClickClosePanelButton()
        {
            _wait.Until(element => closePanelButton.Displayed && closePanelButton.Enabled);
            closePanelButton.Click();
        }

        public ProductDetails CompareProducts(ProductDetails expectedProductDetails, int productCompareIndex)
        {
            _wait.Until(element => compareProductsButton.Enabled);
            compareProductsButton.Click();

            return GetProductDetails(_driver, expectedProductDetails, productCompareIndex);
        }

        private ProductDetails GetProductDetails(IWebDriver _driver, ProductDetails expectedProductDetails, int productCompareIndex)
        {
            ProductDetails product = new ProductDetails();

            product.Name = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Product", productCompareIndex))).Text;
            product.Price = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Price", productCompareIndex))).Text;
            product.Model = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Model", productCompareIndex))).Text;
            product.Brand = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Brand", productCompareIndex))).Text;
            product.Weight = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Weight", productCompareIndex))).Text;

            return product;
        }

        public void AddProductToCart(int quantity)
        {
            itemQuantity.Clear();
            itemQuantity.Click();
            itemQuantity.SendKeys(quantity.ToString());

            if (sizeSelectionDropdown.Count > 0)
            {
                var sizeSelection = sizeSelectionDropdown[0];
                var selectElement = new SelectElement(sizeSelection);
                selectElement.SelectByIndex(1);
            }

            addToCart.Click();          

        }

        private string GetCompareProductDetailsCellXPath(string cellName, int productCompareIndex)
        {
            if(cellName == "Product")
                return $"//table/tbody/tr/td[text()='{cellName}']/following-sibling::td[{productCompareIndex}]//strong";
            else
                return $"//table/tbody/tr/td[text()='{cellName}']/following-sibling::td[{productCompareIndex}]";
        }
    }
}
