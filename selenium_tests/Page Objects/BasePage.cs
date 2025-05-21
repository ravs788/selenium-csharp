using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using selenium_tests.SupportingFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_tests.Page_Objects
{
    
    public class BasePage
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;

        public BasePage()
        {
            
        }

        public IWebDriver SetupDriver()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string headlessMode = config["AppConfig:Headless"];

            if (config["AppConfig:Browser"] == "Chrome")
            {
                ChromeOptions options = new ChromeOptions();
                if (headlessMode.Equals(true))
                    options.AddArguments("--headless");
                _driver = new ChromeDriver(options);
            }
            else if (config["AppConfig:Browser"] == "Firefox")
            {
                FirefoxOptions options = new FirefoxOptions();
                if (headlessMode.Equals(true))
                    options.AddArguments("--headless");
                _driver = new FirefoxDriver(options);
            }
            else
            {
                throw new Exception("Unsupported browser");
            }

            _driver.Manage().Window.Maximize();
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Navigate().GoToUrl(config["AppConfig:BaseURl"]);

            return _driver;
        }

        public WebDriverWait GetWait()
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
            {
                PollingInterval = TimeSpan.FromSeconds(2),
            };

            return _wait;
        }

        public void TearDown()
        {
            _driver.Quit();
        }

    }
}
