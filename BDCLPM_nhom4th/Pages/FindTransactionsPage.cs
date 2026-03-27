using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBankTests.Pages
{
    public class FindTransactionsPage : BasePage
    {
        // Form inputs
        private By accountSelect = By.Id("accountId");
        private By transactionIdInput = By.Id("transactionId");
        private By transactionDateInput = By.Id("transactionDate");
        private By fromDateInput = By.Id("fromDate");
        private By toDateInput = By.Id("toDate");
        private By amountInput = By.Id("amount");

        // Buttons
        private By findByIdButton = By.Id("findById");
        private By findByDateButton = By.Id("findByDate");
        private By findByDateRangeButton = By.Id("findByDateRange");
        private By findByAmountButton = By.Id("findByAmount");

        // Containers
        private By formContainer = By.Id("formContainer");
        private By resultContainer = By.Id("resultContainer");
        private By errorContainer = By.Id("errorContainer");

        // Result
        private By resultsTable = By.Id("transactionTable");
        private By transactionBody = By.Id("transactionBody");
        private By pageHeader = By.XPath("//h1[contains(text(),'Find Transactions')]");

        // Validation error spans
        private By transactionIdError = By.Id("transactionIdError");
        private By transactionDateError = By.Id("transactionDateError");
        private By dateRangeError = By.Id("dateRangeError");
        private By amountError = By.Id("amountError");

        public FindTransactionsPage(IWebDriver driver) : base(driver) { }

        public void Open()
            => driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/findtrans.htm");

        // ── Account selection ─────────────────────────────────────────────

        public void SelectAccount(string accountId)
        {
            var select = new SelectElement(WaitForElementVisible(accountSelect));
            select.SelectByValue(accountId);
        }

        // ── Find actions ──────────────────────────────────────────────────

        public void FindById(string transactionId)
        {
            Type(transactionIdInput, transactionId);
            Click(findByIdButton);
        }

        public void FindByDate(string date)
        {
            Type(transactionDateInput, date);
            Click(findByDateButton);
        }

        public void FindByDateRange(string fromDate, string toDate)
        {
            Type(fromDateInput, fromDate);
            Type(toDateInput, toDate);
            Click(findByDateRangeButton);
        }

        public void FindByAmount(string amount)
        {
            Type(amountInput, amount);
            Click(findByAmountButton);
        }

        // ── Visibility (JS computed style) ────────────────────────────────

        private bool IsVisible(By by)
        {
            try
            {
                var el = driver.FindElement(by);
                var display = ((IJavaScriptExecutor)driver)
                    .ExecuteScript("return window.getComputedStyle(arguments[0]).display;", el)?.ToString();
                return display != "none";
            }
            catch { return false; }
        }

        public bool IsOnPage() => IsDisplayed(pageHeader);
        public bool IsFormVisible() => IsVisible(formContainer);
        public bool IsResultVisible() => IsVisible(resultContainer);
        public bool IsErrorContainerVisible() => IsVisible(errorContainer);
        public bool IsResultsTableDisplayed() => IsDisplayed(resultsTable);

        // ── Result data ───────────────────────────────────────────────────

        public int GetResultRowCount()
        {
            try
            {
                return driver.FindElement(transactionBody)
                             .FindElements(By.TagName("tr")).Count;
            }
            catch { return 0; }
        }

        public bool HasResultRows() => GetResultRowCount() > 0;

        public string GetFirstTransactionLinkHref()
        {
            try
            {
                return driver.FindElement(transactionBody)
                             .FindElement(By.CssSelector("td a"))
                             .GetAttribute("href") ?? string.Empty;
            }
            catch { return string.Empty; }
        }

        public void ClickFirstTransactionLink()
        {
            driver.FindElement(transactionBody)
                  .FindElement(By.CssSelector("td a"))
                  .Click();
        }

        // ── Validation error checks ───────────────────────────────────────

        public string GetTransactionIdError() => GetText(transactionIdError);
        public string GetTransactionDateError() => GetText(transactionDateError);
        public string GetDateRangeError() => GetText(dateRangeError);
        public string GetAmountError() => GetText(amountError);

        public bool IsTransactionIdErrorDisplayed() => IsDisplayed(transactionIdError);
        public bool IsTransactionDateErrorDisplayed() => IsDisplayed(transactionDateError);
        public bool IsDateRangeErrorDisplayed() => IsDisplayed(dateRangeError);
        public bool IsAmountErrorDisplayed() => IsDisplayed(amountError);
        public void WaitForResult()
        {
            var explicitWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            explicitWait.Until(_ => IsVisible(resultContainer) || IsVisible(errorContainer));
        }
    }
}