using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ABCShop.Pages
{
    class ShoppingCart
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;
        ExtentTest ExtTest;
        int Total;

        public ShoppingCart(IWebDriver driver, ExcelAPI ExcelApi, ExtentReports ExtReport)
        {
            this.Driver = driver;
            this.ExcelApi = ExcelApi;
            this.ExtReport = ExtReport;
        }

        static readonly string HomeUrl = "https://www.ecomdeveloper.com/designs/demoshop/index.php?route=common/home";

        IWebElement itemm => Driver.FindElement(By.XPath(" //body/section[@id='page-container']/section[@id='columns']/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[2]/a[1]"));
        IWebElement quntity => Driver.FindElement(By.XPath("//body/section[@id='page-container']/section[@id='columns']/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[2]/div[1]/input[1]"));
        IWebElement Addtocart => Driver.FindElement(By.XPath(" //input[@id='button-cart']"));

        public void PerformAddCart()
        {
            for (int i = 2; i <= ExcelApi.GetRowCount("Cart"); i++)
            {
                ExtTest = ExtReport.CreateTest("Shopping-Cart-Check").Info("Shopping-Cart-Check Started ");

                //scroll down position 
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript("window.scrollTo(0,200);");
                ExtTest.Log(Status.Info, "Scrolled down  Successfully");

                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                itemm.Click();
                ExtTest.Log(Status.Info, "Clicked On Item  Successfully");

                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                //checking the url is ok 
                string currentpage = Driver.Url;
                if (currentpage.Equals("https://www.ecomdeveloper.com/designs/demoshop/index.php?route=product/product&product_id=143"))
                {
                    ExtTest.Log(Status.Info, "Opened Page  Successfully URL Checked");
                }
                else
                {
                    ExtTest.Log(Status.Info, "Opened Page  UnSuccessfull  URL Wrong");
                }

                quntity.Clear();
                ExtTest.Log(Status.Info, "Exsisting Quntity Clear Successfull");
                string qty = ExcelApi.GetCellData("Cart", i, 1);
                quntity.SendKeys(qty);
                ExtTest.Log(Status.Info, "Get Data From Excel Successfull");


                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);

                Addtocart.Click();
                ExtTest.Log(Status.Info, "Add to Cart Button Click Successfull");

                Thread.Sleep(5000);
                ExtTest.Log(Status.Info, "Thred sleep exicuted  Successfull");

                if (Convert.ToInt32(qty) < 1)
                {
                    ExtTest.Log(Status.Info, " Enterd Quentity Is Less Than 1");
                    string massage = Driver.FindElement(By.XPath("//body/section[@id='page-container']/section[@id='sys-notification']/div[1]/div[1]/div[1]")).Text;
                    string chekstr = "Success: You have added";
                    Console.WriteLine(massage);
                    if (massage.Contains(chekstr))
                    {
                        ExtTest.Log(Status.Info, " Success message displayed, Should display an error message");
                        Console.WriteLine("sub if eka athule");
                        ExtTest.Log(Status.Fail, "Shopping-Cart-Check Failed");
                        ExcelApi.SetCellData("Cart", i, 2, "Fail");
                    }
                }
                else
                {
                    Total = Total + Convert.ToInt32(qty);
                    string carttag = Driver.FindElement(By.XPath("//span[@id='cart-total']")).Text;
                    Assert.AreEqual(true, carttag.Contains(Total + " item(s)"));
                    ExtTest.Log(Status.Info, " Check Item Count  Successfull");
                    ExcelApi.SetCellData("Cart", i, 2, "Pass");
                    ExtTest.Log(Status.Pass, "Shopping-Cart-Check Passed");
                }
                Driver.Navigate().GoToUrl(HomeUrl);
            }
        }
    }
}
