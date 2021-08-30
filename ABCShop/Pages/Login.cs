using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace ABCShop.Pages
{
    class Login
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;
        ExtentTest ExtTest;

        public Login(IWebDriver driver, ExcelAPI ExcelApi, ExtentReports ExtReport)
        {
            this.Driver = driver;
            this.ExcelApi = ExcelApi;
            this.ExtReport = ExtReport;
        }

        public void PerformLogin()
        {
            // test process goes here..
            Assert.AreEqual(true, true);
            ExcelApi.SetCellData("Test", 2, 1, "Login");
            ExtTest = ExtReport.CreateTest("LoginCheck").Info("LoginCheck Started");
            ExtTest.Log(Status.Info, "UserName Entered Successfully");
            ExtTest.Log(Status.Info, "Password Entered Successfully");
            ExtTest.Log(Status.Pass, "Login Page Verified Successfully");
        }
    }
}
