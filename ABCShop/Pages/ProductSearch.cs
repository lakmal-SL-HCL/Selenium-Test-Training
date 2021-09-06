using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCShop.Pages
{
    class ProductSearch
    {
        IWebDriver Driver;
        ExcelAPI ExcelApi;
        ExtentReports ExtReport;
        ExtentTest ExtTest;

        static string SearchKey,
            HomeUrl = "https://www.ecomdeveloper.com/designs/demoshop/",
            IconButtonSearchXpath = "//div[@id='search']/div[@class='button-search']",
            InputSearchXpath = "//input[@name='search']",
            InputSearchBoxXpath = "//div[@class='span10']/input[@name='search']",
            ButtonSearchId = "button-search",
            SearchResultXpath = "//div[@class='product-container']/div[@class='name']/a[contains(text(),'"+SearchKey+"')]";

        public ProductSearch(IWebDriver driver, ExcelAPI ExcelApi, ExtentReports ExtReport)
        {
            this.Driver = driver;
            this.ExcelApi = ExcelApi;
            this.ExtReport = ExtReport;
        }

        IWebElement IconButtonSearch => Driver.FindElement(By.XPath(IconButtonSearchXpath));
        IWebElement InputSearch => Driver.FindElement(By.XPath(InputSearchXpath));
        IWebElement InputSearchBox => Driver.FindElement(By.XPath(InputSearchBoxXpath));
        IWebElement ButtonSearch => Driver.FindElement(By.Id(ButtonSearchId));
        IList<IWebElement> SearchResult => Driver.FindElements(By.XPath(SearchResultXpath));

        public void PerformProductSearch()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            Actions action = new Actions(Driver);

            for (int i = 2; i <= ExcelApi.GetRowCount("SearchProduct"); i++)
            {
                // TEST DETAILS
                ExtTest = ExtReport.CreateTest("ProductSearchCheck_"+(i-1)).Info("Product search test case started");
                ExtTest.Log(Status.Info, "Google Chrome opened successfully");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlMatches(HomeUrl));
                ExtTest.Log(Status.Info, "ABC Shop application opened successfully");

                // HOME PAGE PROCESS
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(InputSearchXpath)));
                ExtTest.Log(Status.Info, "Search bar found successfully");
                SearchKey = ExcelApi.GetCellData("SearchProduct", i, 1);
                InputSearch.SendKeys(SearchKey);
                ExtTest.Log(Status.Info, "Search key '"+SearchKey+"' entered successfully");
                action.SendKeys(Keys.Enter).Build().Perform();
                //action.MoveToElement(IconButtonSearch).Click().Build().Perform();
                ExtTest.Log(Status.Info, "Search button clicked successfully");

                // SEARCH PAGE PROCESS
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(ButtonSearchId)));
                ExtTest.Log(Status.Info, "Search page loaded successfully");
                Assert.AreEqual(SearchKey, InputSearchBox.GetAttribute("value"));

                if(ExcelApi.GetCellData("SearchProduct", i, 2) == "true")
                {
                    if (SearchResult.Count >= 1)
                    {
                        ExcelApi.SetCellData("SearchProduct", i, 3, "Pass");
                        ExtTest.Log(Status.Info, "Items found");
                        ExtTest.Log(Status.Pass, "Search item page verified successfully");
                    }
                    else
                    {
                        ExcelApi.SetCellData("SearchProduct", i, 3, "Fail");
                        ExtTest.Log(Status.Info, "Items not found");
                        ExtTest.Log(Status.Fail, "Search item page not verified");
                    }
                }
                else
                {
                    if(SearchResult.Count == 0)
                    {
                        ExcelApi.SetCellData("SearchProduct", i, 3, "Pass");
                        ExtTest.Log(Status.Info, "Items not found");
                        ExtTest.Log(Status.Pass, "Search item page verified successfully");
                    }
                    else
                    {
                        ExcelApi.SetCellData("SearchProduct", i, 3, "Fail");
                        ExtTest.Log(Status.Info, "Items found");
                        ExtTest.Log(Status.Fail, "Search item page not verified");
                    }
                }

                // RETURN TO HOME
                Driver.Navigate().GoToUrl(HomeUrl);
            }
        }
    }
}
