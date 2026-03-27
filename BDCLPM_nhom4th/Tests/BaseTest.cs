using System.Diagnostics;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    public class BaseTest
    {
        protected IWebDriver? driver;
        private Stopwatch _stopwatch = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = DriverFactory.InitDriver();
        }

        // Đóng browser 1 lần sau khi chạy xong toàn bộ test trong class
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver?.Quit();
            driver?.Dispose();
            driver = null;
        }

        [SetUp]
        public void SetUp()
        {
            driver!.Manage().Cookies.DeleteAllCookies();
            _stopwatch = Stopwatch.StartNew();
        }

        [TearDown]
        public void TearDown()
        {
            _stopwatch.Stop();

            var testMethodName = TestContext.CurrentContext.Test.MethodName ?? "";
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var screenshotPath = "";

            if (status == TestStatus.Failed)
            {
                screenshotPath = ScreenshotHelper.TakeScreenshot(driver, testMethodName);
            }

            var excelStatus = status == TestStatus.Passed ? "Pass" : "Fail";

            // Chỉ 3 tham số, bỏ actualResult
            ExcelReportHelper.WriteResult(
                testMethodName: testMethodName,
                status: excelStatus,
                screenshotPath: screenshotPath
            );
        }
    }
}