using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCShop.Pages;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace ABCShop
{
    [TestFixture]
    class Test
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;

        // ONE TIME SETUP
        [OneTimeSetUp]
        public void ExtentReportStart()
        {
            ExtReport = new ExtentReports();
            ExcelApi = new ExcelAPI(@"D:\Documents\Google Drive\Dev\Tests\C#\Testing\SeleniumTest\ABCShop\Outputs\ABCShop.xlsx");
            var HTMLReporter = new ExtentHtmlReporter(@"D:\Documents\Google Drive\Dev\Tests\C#\Testing\SeleniumTest\ABCShop\Outputs\ABCShop.html");
            ExtReport.AttachReporter(HTMLReporter);
        }

        //SETUP
        [SetUp]
        public void LaunchBrowser()
        {
            Driver = new ChromeDriver();
            Driver.Url = "https://www.ecomdeveloper.com/designs/demoshop/";
            Driver.Manage().Window.Maximize();
        }

        // TESTS
        [Test]
        public void TS_01_CreateAccount()
        {
            CreateAccount ca = new CreateAccount(Driver, ExcelApi, ExtReport);
            ca.PerformCreateAccount();
        }

        [Test]
        public void TS_02_Login()
        {
            Login login = new Login(Driver, ExcelApi, ExtReport);
            login.PerformLogin();
        }

        [Test]
        public void TS_03_ProductSearch()
        {
            ProductSearch PS = new ProductSearch(Driver, ExcelApi, ExtReport);
            PS.PerformProductSearch();
        }

        [Test]
        public void TS_04_ShoppingCart()
        {
            ShoppingCart shop = new ShoppingCart(Driver, ExcelApi, ExtReport);
            shop.PerformAddCart();
        }

        [Test]
        public void TS_05_AndroidApp()
        {
            AndroidApp androidApp = new AndroidApp(Driver, ExcelApi, ExtReport);
            androidApp.PerformDownload();
        }


        // TEARDOWN
        [TearDown]
        public void CloseBrowser()
        {
            Driver.Quit();
        }

        // ONE TIME TEARDOWN
        [OneTimeTearDown]
        public void ExtentReportClose()
        {
            ExtReport.Flush();
        }
    }
}
