using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium;
using ParaBankTests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCLPM_nhom4th.Pages
{
    public class AccountDetailsPage : BasePage
    {
        private By accountDetailsHeader = By.XPath("//h1[contains(text(),'Account Details')]");
        private By accountId = By.Id("accountId");
        private By accountType = By.Id("accountType");
        private By balance = By.Id("balance");
        private By availableBalance = By.Id("availableBalance");

        private By transactionRows = By.XPath("//table[@id='transactionTable']/tbody/tr[not(th)]");

        public AccountDetailsPage(IWebDriver driver) : base(driver) { }

        public bool IsAtAccountDetails() => IsDisplayed(accountDetailsHeader);

        public string GetAccountId() => GetText(accountId);

        public string GetAccountType() => GetText(accountType);

        public string GetBalance() => GetText(balance);

        public string GetAvailableBalance() => GetText(availableBalance);

        public int GetTransactionsCount()
        {
            return driver.FindElements(transactionRows).Count;
        }
    }
}
