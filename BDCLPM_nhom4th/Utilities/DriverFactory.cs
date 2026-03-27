using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParaBankTests.Utilities
{
    public static class DriverFactory
    {
        public static IWebDriver InitDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            return new ChromeDriver(options);
        }
    }
}