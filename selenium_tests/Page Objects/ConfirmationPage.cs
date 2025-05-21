using OpenQA.Selenium.Support.UI;
using selenium_tests.SupportingFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_tests.Page_Objects
{
    public class ConfirmationPage : BasePage
    {
        public ConfirmationPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        IWebElement orderConfirmationMessage => _driver.FindElement(By.XPath("//h1[text()='Confirm Order']"));
        IWebElement confirmedOrderTotal => _driver.FindElement(By.XPath("//table[@class='table table-bordered table-hover mb-0']/tfoot/tr/td/strong[text()='Total:']//parent::td//following-sibling::td"));

        public bool WaitForPageLoad() => _wait.Until(element => orderConfirmationMessage.Displayed);

        public string GetConfirmationMessage() => _driver.Title;
        public string GetConfirmedOrderTotal() => confirmedOrderTotal.Text;
    }
}
