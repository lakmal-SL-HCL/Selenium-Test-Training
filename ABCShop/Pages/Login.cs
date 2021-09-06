using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Support.UI;

namespace ABCShop.Pages
{
    class Login
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;
        ExtentTest ExtTest;
        static readonly string home_url = "https://www.ecomdeveloper.com/designs/demoshop/";

        public Login(IWebDriver driver, ExcelAPI ExcelApi, ExtentReports ExtReport)
        {
            this.Driver = driver;
            this.ExcelApi = ExcelApi;
            this.ExtReport = ExtReport;
        }

        IWebElement login => Driver.FindElement(By.LinkText("login"));
        IWebElement email => Driver.FindElement(By.Name("email"));
        IWebElement password => Driver.FindElement(By.Name("password"));
        IWebElement button1 => Driver.FindElement(By.XPath("//body/section[@id='page-container']/section[@id='columns']/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/div[1]/div[2]/div[1]/form[1]/div[1]/input[3]"));
        IWebElement logout => Driver.FindElement(By.LinkText("Logout"));

        public void PerformLogin()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));

            for (int i = 2; i <= ExcelApi.GetRowCount("login_acc"); i++)
            {
                ExtTest = ExtReport.CreateTest("LoginAccountCheck_" + Convert.ToString(i - 1)).Info("Login test case started");
                ExtTest.Log(Status.Info, "Google Chrome opened successfully");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlMatches(home_url));
                ExtTest.Log(Status.Info, "ABC Shop application opened successfully");

                login.Click();
                ExtTest.Log(Status.Info, "login page opened successfully");

                string mail = ExcelApi.GetCellData("login_acc", i, 1);
                email.SendKeys(mail);
                ExtTest.Log(Status.Info, "Email selected");

                string pass = ExcelApi.GetCellData("login_acc", i, 2);
                password.SendKeys(pass);
                ExtTest.Log(Status.Info, "Password selected");

                button1.Click();

                string currentpage = Driver.Url;

                if (ExcelApi.GetCellData("login_acc", i, 3) == "true")
                {
                    if (currentpage.Equals("https://www.ecomdeveloper.com/designs/demoshop/index.php?route=account/account"))
                    {
                        ExcelApi.SetCellData("login_acc", i, 4, "Pass");
                        ExtTest.Log(Status.Info, "Existing user name and password");
                        ExtTest.Log(Status.Info, "Redirected to account page..");
                        ExtTest.Log(Status.Pass, "Login page verified successfully");
                        logout.Click();
                        
                    }
                    else
                    {
                        ExcelApi.SetCellData("login_acc", i, 4, "Fail");
                        ExtTest.Log(Status.Info, "Existing user name and password");
                        ExtTest.Log(Status.Info, "Loggin failed..");
                        ExtTest.Log(Status.Fail, "Login page not verified");
                    }
                }
                else
                {
                    if (currentpage.Equals("https://www.ecomdeveloper.com/designs/demoshop/index.php?route=account/login"))
                    {
                        ExcelApi.SetCellData("login_acc", i, 4, "Pass");
                        ExtTest.Log(Status.Info, "Invalid  user name and password");
                        ExtTest.Log(Status.Info, "Loggin failed..");
                        ExtTest.Log(Status.Pass, "Login page verified successfully");
                    }
                    else
                    {
                        ExcelApi.SetCellData("login_acc", i, 4, "Fail");
                        ExtTest.Log(Status.Info, "Invalid  user name and password");
                        ExtTest.Log(Status.Info, "Redirected to account page..");
                        ExtTest.Log(Status.Fail, "Login page not verified");
                        logout.Click();
                    }
                }
                Driver.Navigate().GoToUrl(home_url);
            }
        }
    }
}
