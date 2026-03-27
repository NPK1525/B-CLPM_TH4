using OpenQA.Selenium;

namespace ParaBankTests.Pages
{
    public class UpdateContactInfoPage : BasePage
    {
        private readonly By firstNameInput = By.Id("customer.firstName");
        private readonly By lastNameInput = By.Id("customer.lastName");
        private readonly By addressInput = By.Id("customer.address.street");
        private readonly By cityInput = By.Id("customer.address.city");
        private readonly By stateInput = By.Id("customer.address.state");
        private readonly By zipCodeInput = By.Id("customer.address.zipCode");
        private readonly By phoneInput = By.Id("customer.phoneNumber");
        private readonly By updateButton = By.XPath("//input[@value='Update Profile']");

        private readonly By successMessage = By.XPath("//*[contains(text(),'Your updated address and phone number have been added to the system')]");
        private readonly By firstNameError = By.Id("customer.firstName.errors");
        private readonly By lastNameError = By.Id("customer.lastName.errors");
        private readonly By addressError = By.Id("customer.address.street.errors");
        private readonly By cityError = By.Id("customer.address.city.errors");
        private readonly By stateError = By.Id("customer.address.state.errors");
        private readonly By zipCodeError = By.Id("customer.address.zipCode.errors");

        public UpdateContactInfoPage(IWebDriver driver) : base(driver) { }

        public void Open()
        {
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/updateprofile.htm");
            // Chờ URL ổn định — nếu bị redirect về login thì SetUp sẽ xử lý
            var longWait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(15));
            longWait.Until(d => d.Url.Contains("updateprofile") || d.Url.Contains("index") || d.Url.Contains("login"));
            // Chỉ wait element nếu đang ở đúng trang
            if (driver.Url.Contains("updateprofile"))
                WaitForElementVisible(firstNameInput);
        }

        public void ClearAndFill(string firstName, string lastName, string address,
            string city, string state, string zipCode, string phone)
        {
            Type(firstNameInput, firstName);
            Type(lastNameInput, lastName);
            Type(addressInput, address);
            Type(cityInput, city);
            Type(stateInput, state);
            Type(zipCodeInput, zipCode);
            Type(phoneInput, phone);
        }

        public void ClickUpdateProfile() => Click(updateButton);

        // Chờ trang phản hồi sau submit (success hoặc error)
        public void WaitForResponse()
        {
            wait.Until(d =>
                IsDisplayed(successMessage) ||
                IsDisplayed(firstNameError) ||
                IsDisplayed(lastNameError) ||
                IsDisplayed(addressError) ||
                d.PageSource.Contains("An internal error") ||
                d.PageSource.Contains("error")
            );
        }

        public bool IsUpdateSuccessful() => IsDisplayed(successMessage);

        public bool IsNotSuccessful() => !IsDisplayed(successMessage);

        public bool IsFirstNameErrorDisplayed() => IsDisplayed(firstNameError);
        public bool IsLastNameErrorDisplayed() => IsDisplayed(lastNameError);
        public bool IsAddressErrorDisplayed() => IsDisplayed(addressError);
        public bool IsCityErrorDisplayed() => IsDisplayed(cityError);
        public bool IsStateErrorDisplayed() => IsDisplayed(stateError);
        public bool IsZipCodeErrorDisplayed() => IsDisplayed(zipCodeError);

        public bool IsAnyErrorDisplayed() =>
            IsFirstNameErrorDisplayed() || IsLastNameErrorDisplayed() ||
            IsAddressErrorDisplayed() || IsCityErrorDisplayed() ||
            IsStateErrorDisplayed() || IsZipCodeErrorDisplayed();
    }
}
