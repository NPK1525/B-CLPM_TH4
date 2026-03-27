using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBankTests.Pages
{
    public class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        protected IWebElement WaitForElementVisible(By by)
        {
            return wait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(by);
                    return element.Displayed ? element : null;
                }
                catch
                {
                    return null;
                }
            });
        }

        protected IWebElement WaitForElementClickable(By by)
        {
            return wait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(by);
                    return (element.Displayed && element.Enabled) ? element : null;
                }
                catch
                {
                    return null;
                }
            });
        }

        protected void Click(By by)
        {
            WaitForElementClickable(by).Click();
        }

        protected void Type(By by, string text)
        {
            var element = WaitForElementVisible(by);
            element.Clear();
            element.SendKeys(text ?? string.Empty);
        }

        protected string GetText(By by)
        {
            return WaitForElementVisible(by).Text.Trim();
        }

        protected bool IsDisplayed(By by)
        {
            try
            {
                return driver.FindElement(by).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}