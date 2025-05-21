using OpenQA.Selenium.Support.UI;
using selenium_tests.SupportingFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_tests.Page_Objects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        IWebElement searchInput => _driver.FindElement(By.Name("search"));
        IWebElement searchButton => _driver.FindElement(By.XPath("//button[text()='Search']"));
        string autoCompletePath = "//div[contains(@class,'search-input-group')]/div[@class='dropdown']/ul[contains(@class,'dropdown-menu')]/li//a/img[@alt='searchText']";
        
        public void SearchProduct(string searchText)
        {
            searchInput.Clear();
            searchInput.SendKeys(searchText);
            SelectSearchedProduct(searchText);
        }

        public void SelectSearchedProduct(string selectedProduct)
        {
            _wait.Until(element =>
                _driver.FindElement(By.XPath("//div[contains(@class,'search-input-group')]/div[@class='dropdown']/ul")).Displayed);

            _wait.Until(element =>
                _driver.FindElement(By.XPath("//div[contains(@class,'search-input-group')]/div[@class='dropdown']/ul")).Enabled);

            autoCompletePath = autoCompletePath.Replace("searchText", selectedProduct);

            IList<IWebElement> autoCompleteSearch = _driver.FindElements(By.XPath(autoCompletePath));

            Console.WriteLine(autoCompletePath);
            Console.WriteLine(autoCompleteSearch.Count);

            autoCompleteSearch[0].Click();

            autoCompletePath = autoCompletePath.Replace(selectedProduct, "searchText");

        }        

    }
}
