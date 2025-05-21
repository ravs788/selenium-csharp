using OpenQA.Selenium.Support.UI;
using selenium_tests.Models;
using selenium_tests.SupportingFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_tests.Page_Objects
{
    public class CheckoutPage : BasePage
    {
        public CheckoutPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }
        IWebElement newCheckoutTotal => _driver.FindElement(By.XPath("//table[@id='checkout-total']/tbody/tr/td[text()='Total:']/following-sibling::td/strong"));
        IWebElement agreeToPrivacyPolicy => _driver.FindElement(By.XPath("//input[@id='input-account-agree']/following-sibling::label"));
        IWebElement agreeToTerms => _driver.FindElement(By.XPath("//input[@id='input-agree']/following-sibling::label"));

        IWebElement companyTxtBox => _driver.FindElement(By.Id("input-payment-company"));
        IWebElement address1TextBox => _driver.FindElement(By.Id("input-payment-address-1"));
        IWebElement address2TextBox => _driver.FindElement(By.Id("input-payment-address-2"));
        IWebElement cityTextBox => _driver.FindElement(By.Id("input-payment-city"));
        IWebElement postcodeTextBox => _driver.FindElement(By.Id("input-payment-postcode"));
        IWebElement countryLabel => _driver.FindElement(By.XPath("//label[@for='input-payment-country']"));
        IList<IWebElement> countrySelectionDropdown => _driver.FindElements(By.XPath("//select[@name='country_id']"));
        
        IWebElement regionLabel => _driver.FindElement(By.XPath("//label[@for='input-payment-zone']"));
        IList<IWebElement> regionSelectionDropdown => _driver.FindElements(By.XPath("//select[@name='zone_id']"));
        string regionSelectionOption = "//select[@name='zone_id']/option[text()='region']";

        IWebElement firstNameTextBox => _driver.FindElement(By.Id("input-payment-firstname"));
        IWebElement lastNameTextBox => _driver.FindElement(By.Id("input-payment-lastname"));
        IWebElement EmailTextBox => _driver.FindElement(By.Id("input-payment-email"));
        IWebElement telephoneTextBox => _driver.FindElement(By.Id("input-payment-telephone"));

        IWebElement passwordTextBox => _driver.FindElement(By.Id("input-payment-password"));
        IWebElement confirmTextBox => _driver.FindElement(By.Id("input-payment-confirm"));

        IWebElement continueButton => _driver.FindElement(By.Id("button-save"));

        public void FillAddressForm(BillingAddress billingAddress)
        {
            companyTxtBox.SendKeys(billingAddress.Company);
            address1TextBox.SendKeys(billingAddress.Address1);
            address2TextBox.SendKeys(billingAddress.Address2);
            cityTextBox.SendKeys(billingAddress.City);
            postcodeTextBox.SendKeys(billingAddress.PostCode);

            if (countrySelectionDropdown.Count > 0)
            {
                var countrySelection = countrySelectionDropdown[0];
                var selectElement = new SelectElement(countrySelection);
                countryLabel.Click();                
                selectElement.SelectByText(billingAddress.Country);
            }

            if (regionSelectionDropdown.Count > 0)
            {
                var regionSelection = regionSelectionDropdown[0];
                var selectElement = new SelectElement(regionSelection);
                //int count = selectElement.Options.Count;
                regionSelectionOption = regionSelectionOption.Replace("region", billingAddress.Region);

                _wait.Until(element => _driver.FindElement(By.XPath(regionSelectionOption)).Enabled);
                regionLabel.Click();

                selectElement.SelectByText(billingAddress.Region);
            }
        }

        public void FillUserDetails(UserDetails userDetails)
        {
            if (userDetails.AccountType == AccountOption.Register)
            {
                firstNameTextBox.SendKeys(userDetails.FirstName);
                lastNameTextBox.SendKeys(userDetails.LastName);
                EmailTextBox.SendKeys(userDetails.Email);
                telephoneTextBox.SendKeys(userDetails.Telephone);

                passwordTextBox.SendKeys(userDetails.Password);
                confirmTextBox.SendKeys(userDetails.ConfirmPassord);
            }
        }

        public string GetCheckoutTotal() => newCheckoutTotal.Text;

        public void checkout(UserDetails userDetails, BillingAddress billingAddress)
        {
            FillUserDetails(userDetails);
            FillAddressForm(billingAddress);
            
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", agreeToPrivacyPolicy);
            agreeToPrivacyPolicy.Click();

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", agreeToTerms);
            agreeToTerms.Click();

            _wait.Until(element => continueButton.Enabled);
            continueButton.Click();
        }
    }
}
