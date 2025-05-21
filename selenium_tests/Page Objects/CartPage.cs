using OpenQA.Selenium.Support.UI;
using selenium_tests.SupportingFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_tests.Page_Objects
{
    public class CartPage : BasePage
    {
        public CartPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        IWebElement viewCartButton => _driver.FindElements(By.XPath("//a[normalize-space()='View Cart']")).FirstOrDefault();
        IWebElement checkoutTotal => _driver.FindElement(By.XPath("//table[@class='table table-bordered m-0']/tbody/tr/td[text()='Total:']/following-sibling::td/strong"));
        IWebElement checkoutButton => _driver.FindElement(By.XPath("//a[text()='Checkout']"));

        public string VerifyAddedItems()
        {
            _wait.Until(driver1 => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
            _wait.Until(element => viewCartButton!=null);
            _wait.Until(element => viewCartButton.Displayed);
            _wait.Until(element => viewCartButton.Enabled);

            viewCartButton.Click();
            return checkoutTotal.Text;

            
        }

        public void PerformCheckout()
        {
            checkoutButton.Click();
        }

    }
}
