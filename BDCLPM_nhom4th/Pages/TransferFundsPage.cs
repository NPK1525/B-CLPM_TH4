using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBankTests.Pages
{
    public class TransferFundsPage : BasePage
    {
        private By amountInput = By.Id("amount");
        private By fromAccountSelect = By.Id("fromAccountId");
        private By toAccountSelect = By.Id("toAccountId");
        private By transferButton = By.XPath("//input[@value='Transfer']");
        private By successMessage = By.XPath("//h1[contains(text(),'Transfer Complete')]");
        private By pageHeader = By.XPath("//h1[contains(text(),'Transfer Funds')]");

        public TransferFundsPage(IWebDriver driver) : base(driver) { }

        public bool IsOnPage() => IsDisplayed(pageHeader);

        public void Transfer(string amount)
        {
            Type(amountInput, amount);
            Click(transferButton);
        }

        public bool IsTransferSuccessful()
        {
            return WaitForElementVisible(successMessage) != null;
        }
        public string GetSuccessMessage() => GetText(successMessage);

        public void SelectFromAccount(string accountId)
        {
            System.Threading.Thread.Sleep(500); // Ajax load
            var select = new SelectElement(driver.FindElement(fromAccountSelect));
            select.SelectByText(accountId);
        }

        public void SelectToAccount(string accountId)
        {
            System.Threading.Thread.Sleep(500); // Ajax load
            var select = new SelectElement(driver.FindElement(toAccountSelect));
            select.SelectByText(accountId);
        }

        public IList<string> GetFromAccountIds()
        {
            System.Threading.Thread.Sleep(500);
            var select = new SelectElement(driver.FindElement(fromAccountSelect));
            return select.Options.Select(opt => opt.Text).ToList();
        }

        public IList<string> GetToAccountIds()
        {
            System.Threading.Thread.Sleep(500);
            var select = new SelectElement(driver.FindElement(toAccountSelect));
            return select.Options.Select(opt => opt.Text).ToList();
        }

        public bool HasAmountError()
        {
            try
            {
                return IsDisplayed(By.Id("amount.errors"));
            }
            catch
            {
                return false;
            }
        }
    }
}
