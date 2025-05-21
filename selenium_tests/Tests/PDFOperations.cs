//using OpenQA.Selenium.Support.UI;
//using selenium_tests.SupportingFunctions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace selenium_tests.Tests
//{
//    [TestFixture]
//    public class PDFOperations
//    {
//        public required IWebDriver _driver;
//        public required WebDriverWait wait;
//        string url = "https://www.adobe.com/support/products/enterprise/knowledgecenter/media/c4611_sample_explain.pdf";

//        [SetUp]
//        public void TestInit()
//        {
//            _driver = new ChromeDriver();
//            _driver.Navigate().GoToUrl(url);
//            _driver.Manage().Window.Maximize();
//            _driver.Manage().Cookies.DeleteAllCookies();
//        }

//        [TearDown]
//        public void TestCleanup()
//        {
//            _driver.Quit();
//        }

//        [Test]
//        public void pdfOperations()
//        {
//            Uri pdfUrl = new Uri(url);
//            pdfUrl.
//        }
//    }
//}
