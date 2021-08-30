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

        // SETUP
        [OneTimeSetUp]
        public void ExtentReportStart()
        {
            ExtReport = new ExtentReports();
            ExcelApi = new ExcelAPI(@"C:\ABCShop.xlsx");
            var HTMLReporter = new ExtentHtmlReporter(@"C:\ABCShop.html");
            ExtReport.AttachReporter(HTMLReporter);
        }

        [SetUp]
        public void LaunchBrowser()
        {
            Driver = new ChromeDriver();
            Driver.Url = "https://www.ecomdeveloper.com/designs/demoshop/";
            //driver.Manage().Window.Maximize();
        }

        // TESTS
        [Test]
        public void TC_01_PerformCreateAccount()
        {
            CreateAccount ca = new CreateAccount(Driver, ExcelApi, ExtReport);
            ca.PerformCreateAccount();
        }

        [Test]
        public void TC_02_PerformLogin()
        {
            Login login = new Login(Driver, ExcelApi, ExtReport);
            login.PerformLogin();
        }

        // TEARDOWN
        [TearDown]
        public void CloseBrowser()
        {
            Driver.Quit();
        }

        [OneTimeTearDown]
        public void ExtentReportClose()
        {
            ExtReport.Flush();
        }
    }
}
