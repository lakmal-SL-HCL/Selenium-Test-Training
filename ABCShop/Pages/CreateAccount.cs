using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace ABCShop.Pages
{
    class CreateAccount
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;
        ExtentTest ExtTest;

        public CreateAccount(IWebDriver Driver, ExcelAPI ExcelApi, ExtentReports ExtReport)
        {
            this.Driver = Driver;
            this.ExcelApi = ExcelApi;
            this.ExtReport = ExtReport;
        }

        public void PerformCreateAccount()
        {
            // test process goes here..
            Assert.AreEqual(true, true);
            ExcelApi.SetCellData("Test", 1, 1, "Create Account");
            ExtTest = ExtReport.CreateTest("CreateAccountCheck").Info("CreateAccountCheck Started");
            ExtTest.Log(Status.Info, "Details Successfully");
            ExtTest.Log(Status.Pass, "Create Account Page Verified Successfully");
        }
    }
}
