using OpenQA.Selenium;

namespace ParaBankTests.Pages
{
    public class AccountOverviewPage : BasePage
    {
        private By accountsOverviewHeader = By.XPath("//h1[contains(text(),'Accounts Overview')]");
        private By logoutLink = By.LinkText("Log Out");
        private By transferFundsLink = By.LinkText("Transfer Funds");
        private By billPayLink = By.LinkText("Bill Pay");
        private By openAccountLink = By.LinkText("Open New Account");
        private By findTransactionsLink = By.LinkText("Find Transactions");
        private By firstAccountLink = By.CssSelector("#accountTable tbody tr td:first-child a");

        // Thêm các By còn thiếu
        private By accountTableRows = By.CssSelector("#accountTable tbody tr");
        private By totalBalanceCell = By.XPath("//table[@id='accountTable']//tfoot//td[contains(@colspan,'2')]/following-sibling::td[1]");

        public AccountOverviewPage(IWebDriver driver) : base(driver) { }

        public void Open()
            => driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/overview.htm");

        public bool IsAtAccountOverview() => IsDisplayed(accountsOverviewHeader);
        public bool IsLogoutVisible() => IsDisplayed(logoutLink);
        public void Logout() => Click(logoutLink);
        public void GoToTransferFunds() => Click(transferFundsLink);
        public void GoToBillPay() => Click(billPayLink);
        public void GoToOpenAccount() => Click(openAccountLink);
        public void GoToFindTransactions() => Click(findTransactionsLink);
        public bool IsTransferFundsLinkDisplayed() => IsDisplayed(transferFundsLink);
        public bool IsBillPayLinkDisplayed() => IsDisplayed(billPayLink);
        public bool IsOpenAccountLinkDisplayed() => IsDisplayed(openAccountLink);

        /// <summary>
        /// Lấy Account ID đầu tiên từ trang Account Overview
        /// </summary>
        public string GetFirstAccountId()
        {
            try
            {
                var el = WaitForElementVisible(firstAccountLink);
                return el.Text.Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// TS_12: Đếm số dòng tài khoản trong bảng
        /// </summary>
        public int GetAccountRowsCount()
        {
            try
            {
                WaitForElementVisible(accountTableRows);
                var rows = driver.FindElements(accountTableRows);
                return rows.Count;
            }
            catch { return 0; }
        }

        /// <summary>
        /// TS_13: Lấy tổng số dư hiển thị ở cuối bảng
        /// </summary>
        public string GetTotalBalance()
        {
            try
            {
                // ParaBank hiển thị total ở hàng cuối bảng dạng: "Total | $xxx"
                var footerRow = driver.FindElement(
                    By.XPath("//table[@id='accountTable']//tr[last()]/td[last()]"));
                return footerRow.Text.Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// TS_14, TS_15, TS_16: Click vào tài khoản đầu tiên
        /// </summary>
        public void ClickFirstAccount()
        {
            var el = WaitForElementVisible(firstAccountLink);
            el.Click();
        }

        /// <summary>
        /// TS_20, TS_24: Điều hướng về trang Account Overview
        /// </summary>
        public void GoToAccountOverview()
        {
            try
            {
                // Thử click link trên nav trước
                Click(By.LinkText("Accounts Overview"));
            }
            catch
            {
                // Fallback: navigate trực tiếp
                driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/overview.htm");
            }
        }

        /// <summary>
        /// TS_20: Kiểm tra accountId có xuất hiện trong danh sách không
        /// </summary>
        public bool IsAccountInList(string accountId)
        {
            try
            {
                WaitForElementVisible(firstAccountLink);
                var links = driver.FindElements(firstAccountLink);
                return links.Any(l => l.Text.Trim() == accountId);
            }
            catch { return false; }
        }

        public string GetBalanceForAccount(string accountId)
        {
            try
            {
                WaitForElementVisible(accountTableRows);
                var rows = driver.FindElements(accountTableRows);
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 2 && cells[0].Text.Trim() == accountId)
                        return cells[1].Text.Trim();
                }
                return string.Empty;
            }
            catch { return string.Empty; }
        }
    }
}