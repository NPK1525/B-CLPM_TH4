using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBankTests.Pages
{
    public class RequestLoanPage : BasePage
    {
        private By loanAmountInput = By.Id("amount");
        private By downPaymentInput = By.Id("downPayment");
        private By fromAccountSelect = By.Id("fromAccountId");
        private By applyNowButton = By.XPath("//input[@value='Apply Now']");

        private By requestLoanForm = By.Id("requestLoanForm");
        private By requestLoanResult = By.Id("requestLoanResult");
        private By requestLoanError = By.Id("requestLoanError");
        private By loanRequestApproved = By.Id("loanRequestApproved");
        private By loanRequestDenied = By.Id("loanRequestDenied");

        private By loanStatus = By.Id("loanStatus");
        private By loanProviderName = By.Id("loanProviderName");
        private By newAccountIdLink = By.Id("newAccountId");
        private By deniedErrorMessage = By.CssSelector("#loanRequestDenied p.error");

        public RequestLoanPage(IWebDriver driver) : base(driver) { }

        public void Open()
            => driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/requestloan.htm");

        public void FillAndSubmit(string loanAmount, string downPayment, string fromAccount)
        {
            Type(loanAmountInput, loanAmount);
            Type(downPaymentInput, downPayment);
            var select = new SelectElement(driver.FindElement(fromAccountSelect));
            select.SelectByValue(fromAccount);
            Click(applyNowButton);
        }

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

        public bool IsFormVisible() => IsVisible(requestLoanForm);
        public bool IsResultVisible() => IsVisible(requestLoanResult);
        public bool IsErrorVisible() => IsVisible(requestLoanError);
        public bool IsApprovedVisible() => IsDisplayed(loanRequestApproved);
        public bool IsDeniedVisible() => IsDisplayed(loanRequestDenied);

        public string GetLoanStatus() => GetText(loanStatus);
        public string GetLoanProvider() => GetText(loanProviderName);
        public string GetNewAccountId() => GetText(newAccountIdLink);
        public string GetNewAccountHref() => driver.FindElement(newAccountIdLink).GetAttribute("href") ?? string.Empty;
        public string GetDeniedErrorMessage() => GetText(deniedErrorMessage);
        public void WaitForResult()
        {
            var explicitWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            explicitWait.Until(_ =>
                IsVisible(requestLoanResult) || IsVisible(requestLoanError));
        }
    }
}