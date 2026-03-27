using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBankTests.Pages
{
    public class OpenAccountPage : BasePage
    {
        private By accountTypeSelect = By.Id("type");
        private By openAccountButton = By.XPath("//input[@value='Open New Account']");
        private By successMessage = By.XPath("//h1[contains(text(),'Account Opened')]");
        private By newAccountLink = By.Id("newAccountId");
        private By pageHeader = By.XPath("//h1[contains(text(),'Open New Account')]");
        private By fromAccountIdSelect = By.Id("fromAccountId");

        public OpenAccountPage(IWebDriver driver) : base(driver) { }

        public bool IsOnPage() => IsDisplayed(pageHeader);

        public void SelectAccountType(string type)
        {
            var select = new SelectElement(driver.FindElement(accountTypeSelect));
            select.SelectByText(type);
        }

        public void ClickOpenAccount() => Click(openAccountButton);

        public bool IsAccountOpened()
        {
            return WaitForElementVisible(successMessage) != null;
        }
        public string GetNewAccountId() => GetText(newAccountLink);

        public IList<string> GetAvailableAccountTypes()
        {
            System.Threading.Thread.Sleep(500);
            var select = new SelectElement(driver.FindElement(accountTypeSelect));
            return select.Options.Select(opt => opt.Text).ToList();
        }

        public void SelectFromAccountId(string accountId)
        {
            System.Threading.Thread.Sleep(500);
            var select = new SelectElement(driver.FindElement(fromAccountIdSelect));
            select.SelectByText(accountId);
        }

        public IList<string> GetAvailableFromAccountIds()
        {
            System.Threading.Thread.Sleep(500);
            var select = new SelectElement(driver.FindElement(fromAccountIdSelect));
            return select.Options.Select(opt => opt.Text).ToList();
        }
    }
}
