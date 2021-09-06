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
using System.Threading;

namespace ABCShop.Pages
{
    class CreateAccount
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;
        ExtentTest ExtTest;

        static readonly string home_url = "https://www.ecomdeveloper.com/designs/demoshop/";
        int telephone_number;

        public CreateAccount(IWebDriver Driver, ExcelAPI ExcelApi, ExtentReports ExtReport)
        {
            this.Driver = Driver;
            this.ExcelApi = ExcelApi;
            this.ExtReport = ExtReport;
        }

        IWebElement create_acc => Driver.FindElement(By.LinkText("create an account"));
        IWebElement first_name => Driver.FindElement(By.Name("firstname"));
        IWebElement last_name => Driver.FindElement(By.Name("lastname"));
        IWebElement email => Driver.FindElement(By.Name("email"));
        IWebElement telephone => Driver.FindElement(By.Name("telephone"));
        IWebElement address1 => Driver.FindElement(By.Name("address_1"));
        IWebElement city => Driver.FindElement(By.Name("city"));
        IWebElement country => Driver.FindElement(By.Name("country_id"));
        IWebElement state => Driver.FindElement(By.Name("zone_id"));
        IWebElement password => Driver.FindElement(By.Name("password"));
        IWebElement confirm_password => Driver.FindElement(By.Name("confirm"));
        IWebElement subscribe => Driver.FindElement(By.XPath("//input[@name='newsletter'][1]"));
        IWebElement privacy => Driver.FindElement(By.Name("agree"));
        IWebElement button => Driver.FindElement(By.ClassName("button"));
        IWebElement logout => Driver.FindElement(By.LinkText("Logout"));

        public void PerformCreateAccount()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));

            for (int i = 2; i <= ExcelApi.GetRowCount("create_acc"); i++)
            {
                ExtTest = ExtReport.CreateTest("CreateAccountCheck_" + Convert.ToString(i - 1)).Info("Create account test case started");
                ExtTest.Log(Status.Info, "Google Chrome opened successfully");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlMatches(home_url));
                ExtTest.Log(Status.Info, "ABC Shop application opened successfully");

                create_acc.Click();
                ExtTest.Log(Status.Info, "Create account page opened successfully");

                string fname = ExcelApi.GetCellData("create_acc", i, 1);
                first_name.SendKeys(fname);
                ExtTest.Log(Status.Info, "First Name selected");

                string lname = ExcelApi.GetCellData("create_acc", i, 2);
                last_name.SendKeys(lname);
                ExtTest.Log(Status.Info, "Last Name selected");

                string mail = ExcelApi.GetCellData("create_acc", i, 3);
                email.SendKeys(mail);
                ExtTest.Log(Status.Info, "Email selected");

                string tel = ExcelApi.GetCellData("create_acc", i, 4);
                telephone.SendKeys(tel);
                ExtTest.Log(Status.Info, "Telephone selected");

                string add1 = ExcelApi.GetCellData("create_acc", i, 5);
                address1.SendKeys(add1);
                ExtTest.Log(Status.Info, "Address1 selected");

                string cty = ExcelApi.GetCellData("create_acc", i, 6);
                city.SendKeys(cty);
                ExtTest.Log(Status.Info, "City selected");

                SelectElement sel = new SelectElement(country);
                sel.SelectByText(ExcelApi.GetCellData("create_acc", i, 7));
                ExtTest.Log(Status.Info, "Country selected");

                Thread.Sleep(2000);
                SelectElement sel1 = new SelectElement(state);
                sel1.SelectByText(ExcelApi.GetCellData("create_acc", i, 8));
                ExtTest.Log(Status.Info, "State selected");

                string pass = ExcelApi.GetCellData("create_acc", i, 9);
                password.SendKeys(pass);
                ExtTest.Log(Status.Info, "Password selected");

                string con_pass = ExcelApi.GetCellData("create_acc", i, 10);
                confirm_password.SendKeys(con_pass);
                ExtTest.Log(Status.Info, "Confirm Password selected");

                subscribe.Click();
                privacy.Click();
                button.Click();

                string currentpage = Driver.Url;

                try
                {
                    //CHECK TELEPHONE IS INTEGER
                    telephone_number = Convert.ToInt32(tel);

                    // CHECK FOR CONFIRM PASSWORD
                    if (!pass.Equals(con_pass))
                    {
                        if (currentpage.Equals("https://www.ecomdeveloper.com/designs/demoshop/index.php?route=account/register"))
                        {
                            ExcelApi.SetCellData("create_acc", i, 11, "Pass");
                            ExtTest.Log(Status.Info, "Password and confirm password mismatched");
                            ExtTest.Log(Status.Info, "Account not created");
                            ExtTest.Log(Status.Pass, "Create account page verified successfully");
                            continue;
                        }
                        else
                        {
                            ExcelApi.SetCellData("create_acc", i, 11, "Fail");
                            ExtTest.Log(Status.Info, "Password and confirm password mismatched");
                            ExtTest.Log(Status.Info, "Account created");
                            ExtTest.Log(Status.Fail, "Create account page not verified");
                            logout.Click();
                            continue;
                        }
                    }
                    
                    // CHECK FOR SUCCESSFULL ACCOUINT CREATION
                    if (currentpage.Equals("https://www.ecomdeveloper.com/designs/demoshop/index.php?route=account/success"))
                    {
                        ExcelApi.SetCellData("create_acc", i, 11, "Pass");
                        ExtTest.Log(Status.Info, "Account created");
                        ExtTest.Log(Status.Pass, "Create account page verified Successfully");
                        logout.Click();
                        continue;
                    }
                    else
                    {
                        ExcelApi.SetCellData("create_acc", i, 11, "Fail");
                        ExtTest.Log(Status.Info, "Account not created");
                        ExtTest.Log(Status.Fail, "Create account page not verified");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    if (currentpage.Equals("https://www.ecomdeveloper.com/designs/demoshop/index.php?route=account/register"))
                    {
                        ExcelApi.SetCellData("create_acc", i, 11, "Pass");
                        ExtTest.Log(Status.Info, "Telephone number is invalid");
                        ExtTest.Log(Status.Info, "Account not created");
                        ExtTest.Log(Status.Pass, "Create account page verified successfully");
                    }
                    else
                    {
                        ExcelApi.SetCellData("create_acc", i, 11, "Fail");
                        ExtTest.Log(Status.Info, "Telephone number is invalid");
                        ExtTest.Log(Status.Info, "Account created");
                        ExtTest.Log(Status.Fail, "Create account page not verified");
                        logout.Click();
                    }
                }
                Driver.Navigate().GoToUrl(home_url);
            }
        }
    }
}
