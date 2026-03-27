using OpenQA.Selenium;

namespace ParaBankTests.Pages
{
    /// <summary>
    /// BillPayPage — cập nhật thêm methods cho validation và result checks.
    /// COPY ĐÈ LÊN file Pages/BillPayPage.cs cũ.
    /// </summary>
    public class BillPayPage : BasePage
    {
        // Form inputs (giữ nguyên name selectors theo HTML)
        private By payeeNameInput = By.Name("payee.name");
        private By addressInput = By.Name("payee.address.street");
        private By cityInput = By.Name("payee.address.city");
        private By stateInput = By.Name("payee.address.state");
        private By zipCodeInput = By.Name("payee.address.zipCode");
        private By phoneInput = By.Name("payee.phoneNumber");
        private By accountNumberInput = By.Name("payee.accountNumber");
        private By verifyAccountInput = By.Name("verifyAccount");
        private By amountInput = By.Name("amount");
        private By sendPaymentButton = By.XPath("//input[@value='Send Payment']");

        // Result / error containers
        private By billpayForm = By.Id("billpayForm");
        private By billpayResult = By.Id("billpayResult");
        private By billpayError = By.Id("billpayError");

        // Success result spans
        private By payeeNameSpan = By.Id("payeeName");
        private By amountSpan = By.Id("amount");
        private By fromAccountSpan = By.Id("fromAccountId");
        private By successHeader = By.XPath("//h1[contains(text(),'Bill Payment Complete')]");

        // Validation error spans
        private By errName = By.Id("validationModel-name");
        private By errAddress = By.Id("validationModel-address");
        private By errCity = By.Id("validationModel-city");
        private By errState = By.Id("validationModel-state");
        private By errZipCode = By.Id("validationModel-zipCode");
        private By errPhone = By.Id("validationModel-phoneNumber");
        private By errAccountEmpty = By.Id("validationModel-account-empty");
        private By errAccountInvalid = By.Id("validationModel-account-invalid");
        private By errVerifyEmpty = By.Id("validationModel-verifyAccount-empty");
        private By errVerifyMismatch = By.Id("validationModel-verifyAccount-mismatch");
        private By errAmountEmpty = By.Id("validationModel-amount-empty");
        private By errAmountInvalid = By.Id("validationModel-amount-invalid");

        private By fromAccountSelect = By.Name("fromAccountId");

        public BillPayPage(IWebDriver driver) : base(driver) { }

        public void Open()
            => driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/billpay.htm");


        public void FillPayeeInfo(string name, string address, string city, string state,
            string zip, string phone, string accountNumber, string amount)
        {
            Type(payeeNameInput, name);
            Type(addressInput, address);
            Type(cityInput, city);
            Type(stateInput, state);
            Type(zipCodeInput, zip);
            Type(phoneInput, phone);
            Type(accountNumberInput, accountNumber);
            Type(verifyAccountInput, accountNumber);
            Type(amountInput, amount);
        }

        public void FillPayeeInfoWithMismatch(string name, string address, string city,
            string state, string zip, string phone,
            string accountNumber, string verifyAccount, string amount)
        {
            Type(payeeNameInput, name);
            Type(addressInput, address);
            Type(cityInput, city);
            Type(stateInput, state);
            Type(zipCodeInput, zip);
            Type(phoneInput, phone);
            Type(accountNumberInput, accountNumber);
            Type(verifyAccountInput, verifyAccount);
            Type(amountInput, amount);
        }

        public void ClickSendPayment() => Click(sendPaymentButton);


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

        public bool IsFormVisible() => IsVisible(billpayForm);
        public bool IsResultVisible() => IsVisible(billpayResult);
        public bool IsErrorVisible() => IsVisible(billpayError);

        public bool IsPaymentSuccessful() => IsDisplayed(successHeader);
        public string GetPayeeNameText() => GetText(payeeNameSpan);
        public string GetAmountText() => GetText(amountSpan);
        public string GetFromAccountText() => GetText(fromAccountSpan);

        public bool IsErrorNameDisplayed() => IsDisplayed(errName);
        public bool IsErrorAddressDisplayed() => IsDisplayed(errAddress);
        public bool IsErrorCityDisplayed() => IsDisplayed(errCity);
        public bool IsErrorStateDisplayed() => IsDisplayed(errState);
        public bool IsErrorZipCodeDisplayed() => IsDisplayed(errZipCode);
        public bool IsErrorPhoneDisplayed() => IsDisplayed(errPhone);
        public bool IsErrorAccountEmptyDisplayed() => IsDisplayed(errAccountEmpty);
        public bool IsErrorAccountInvalidDisplayed() => IsDisplayed(errAccountInvalid);
        public bool IsErrorVerifyEmptyDisplayed() => IsDisplayed(errVerifyEmpty);
        public bool IsErrorVerifyMismatchDisplayed() => IsDisplayed(errVerifyMismatch);
        public bool IsErrorAmountEmptyDisplayed() => IsDisplayed(errAmountEmpty);
        public bool IsErrorAmountInvalidDisplayed() => IsDisplayed(errAmountInvalid);

        // ── Dropdown check ──────────────────────────────────────────────────

        public IList<string> GetFromAccountOptions()
        {
            var select = new OpenQA.Selenium.Support.UI.SelectElement(
                driver.FindElement(fromAccountSelect));
            return select.Options.Select(o => o.GetAttribute("value") ?? string.Empty).ToList();
        }
    }
}