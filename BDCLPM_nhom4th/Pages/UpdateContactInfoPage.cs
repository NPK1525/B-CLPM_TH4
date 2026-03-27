using OpenQA.Selenium;

namespace ParaBankTests.Pages
{
    public class UpdateContactInfoPage : BasePage
    {
        private By firstNameInput = By.Id("customer.firstName");
        private By lastNameInput = By.Id("customer.lastName");
        private By addressInput = By.Id("customer.address.street");
        private By cityInput = By.Id("customer.address.city");
        private By stateInput = By.Id("customer.address.state");
        private By zipCodeInput = By.Id("customer.address.zipCode");
        private By phoneInput = By.Id("customer.phoneNumber");
        private By updateButton = By.XPath("//input[@value='Update Profile']");

        private By successMessage = By.XPath("//*[contains(text(),'Your updated address and phone number have been added to the system')]");
        private By firstNameError = By.Id("customer.firstName.errors");
        private By lastNameError = By.Id("customer.lastName.errors");
        private By addressError = By.Id("customer.address.street.errors");
        private By cityError = By.Id("customer.address.city.errors");
        private By stateError = By.Id("customer.address.state.errors");
        private By zipCodeError = By.Id("customer.address.zipCode.errors");

        public UpdateContactInfoPage(IWebDriver driver) : base(driver) { }

        public void Open()
        {
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/updateprofile.htm");
            // Chờ URL ổn định (không bị redirect về login)
            wait.Until(d => d.Url.Contains("updateprofile"));
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

        public bool IsUpdateSuccessful()
        {
            // Debug: in URL và một phần page source
            Console.WriteLine($"[DEBUG] Current URL: {driver.Url}");
            Console.WriteLine($"[DEBUG] Page title: {driver.Title}");
            try
            {
                var body = driver.FindElement(By.TagName("body")).Text;
                Console.WriteLine($"[DEBUG] Body (first 500): {body.Substring(0, Math.Min(500, body.Length))}");
            }
            catch { }
            return IsDisplayed(successMessage);
        }
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
