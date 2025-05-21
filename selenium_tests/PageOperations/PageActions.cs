//using Microsoft.Extensions.Configuration;
//using OpenQA.Selenium.Firefox;
//using OpenQA.Selenium.Support.UI;
//using selenium_tests.Page_Objects;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace selenium_tests.PageOperations
//{
//    public class PageActions
//    {
//        private IWebDriver _driver;
//        private WebDriverWait _wait;
//        public HomePage homePage{ get; private set; }
//        public ProductPage productPage{ get; private set; }
//        public CartPage cartPage{ get; private set; }
//        public CheckoutPage checkoutPage{ get; private set; }
//        public ConfirmationPage confirmationPage{ get; private set; }

//        public PageActions(IWebDriver driver, WebDriverWait wait)
//        {
//            _driver = driver;
//            _wait = wait;

//            var config = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.json")
//                .Build();
            
//            string headlessMode = config["AppConfig:Headless"];

//            if (config["AppConfig:Browser"] == "Chrome")
//            {
//                ChromeOptions options = new ChromeOptions();
//                if(headlessMode.Equals(true))
//                    options.AddArguments("--headless");
//                _driver = new ChromeDriver(options);                
//            }
//            else if (config["AppConfig:Browser"] == "Firefox")
//            {
//                FirefoxOptions options = new FirefoxOptions();
//                if (headlessMode.Equals(true))
//                    options.AddArguments("--headless");
//                _driver = new FirefoxDriver(options);
//            }
//            else
//            {
//                throw new Exception("Unsupported browser");
//            }

//            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
//            {
//                PollingInterval = TimeSpan.FromSeconds(2),
//            };


//            _driver.Manage().Window.Maximize();
//            _driver.Manage().Cookies.DeleteAllCookies();
//            _driver.Navigate().GoToUrl(config["AppConfig:BaseURl"]);

//            homePage = new HomePage();
//            productPage = new ProductPage();
//            cartPage = new CartPage();
//            checkoutPage = new CheckoutPage();
//            confirmationPage = new ConfirmationPage();
//        }

//        public void CloseDriver()
//        {
//            _driver.Quit();
//        }

//    }
//}
