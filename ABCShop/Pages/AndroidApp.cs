using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ABCShop.Pages
{
    class AndroidApp
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;
        ExtentTest ExtTest;

        public AndroidApp(IWebDriver driver, ExcelAPI ExcelApi, ExtentReports ExtReport)
        {
            this.Driver = driver;
            this.ExcelApi = ExcelApi;
            this.ExtReport = ExtReport;
        }

        IWebElement Gplay => Driver.FindElement(By.XPath("//body/section[@id='page-container']/section[@id='footer']/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/p[1]/a[1]/img[1]"));

        public void PerformDownload()
        {
            Actions action = new Actions(Driver);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));

            ExtTest = ExtReport.CreateTest("Android App Button Check").Info("Android App Button Cheak Check Started");
            Console.WriteLine(Driver.Title);
            
            action.MoveToElement(Gplay).Click().Build().Perform();
            ExtTest.Log(Status.Info, "Download icon clicked  successfully");
            Thread.Sleep(5000);

            Driver.SwitchTo().Window(Driver.WindowHandles[1]);

            String currentpage = Driver.Url;
            if (currentpage.Equals("https://play.google.com/store/apps/details?id=com.demoshop.abc"))
            {
                ExtTest.Log(Status.Info, "Redirect to Google play store Successfully");
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(text(),'Demo - ABC Shop')]")));
                IWebElement name = Driver.FindElement(By.XPath("//span[contains(text(),'Demo - ABC Shop')]"));
                Console.WriteLine(name.Displayed);
                Boolean Display = name.Displayed;

                if (Display)
                {
                    ExtTest.Log(Status.Pass, "App Found Successfully");
                }
                else
                {
                    ExtTest.Log(Status.Fail, "App not Found");
                }
            }
            else
            {
                ExtTest.Log(Status.Fail, "Redirect to Google play Failed");
            }
        }
    }
}
