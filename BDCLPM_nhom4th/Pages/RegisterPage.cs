using OpenQA.Selenium;

namespace ParaBankTests.Pages
{
    public class RegisterPage : BasePage
    {
        // Input locators
        private By firstNameInput       = By.Id("customer.firstName");
        private By lastNameInput        = By.Id("customer.lastName");
        private By addressInput         = By.Id("customer.address.street");
        private By cityInput            = By.Id("customer.address.city");
        private By stateInput           = By.Id("customer.address.state");
        private By zipCodeInput         = By.Id("customer.address.zipCode");
        private By phoneInput           = By.Id("customer.phoneNumber");
        private By ssnInput             = By.Id("customer.ssn");
        private By usernameInput        = By.Id("customer.username");
        private By passwordInput        = By.Id("customer.password");
        private By confirmPasswordInput = By.Id("repeatedPassword");
        private By registerButton       = By.XPath("//input[@value='Register']");

        // Validation error locators (ParaBank dùng span id="*Error")
        private By firstNameError       = By.Id("customer.firstName.errors");
        private By lastNameError        = By.Id("customer.lastName.errors");
        private By addressError         = By.Id("customer.address.street.errors");
        private By cityError            = By.Id("customer.address.city.errors");
        private By stateError           = By.Id("customer.address.state.errors");
        private By zipCodeError         = By.Id("customer.address.zipCode.errors");
        private By phoneError           = By.Id("customer.phoneNumber.errors");
        private By ssnError             = By.Id("customer.ssn.errors");
        private By usernameError        = By.Id("customer.username.errors");
        private By passwordError        = By.Id("customer.password.errors");

        // Success locators
        private By successMessage = By.XPath("//div[@id='rightPanel']//p");
        private By welcomeTitle   = By.XPath("//h1[contains(text(),'Welcome')]");

        public RegisterPage(IWebDriver driver) : base(driver) { }

        public void Open()
        {
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/register.htm");
        }

        public void EnterFirstName(string value)        => Type(firstNameInput, value);
        public void EnterLastName(string value)         => Type(lastNameInput, value);
        public void EnterAddress(string value)          => Type(addressInput, value);
        public void EnterCity(string value)             => Type(cityInput, value);
        public void EnterState(string value)            => Type(stateInput, value);
        public void EnterZipCode(string value)          => Type(zipCodeInput, value);
        public void EnterPhone(string value)            => Type(phoneInput, value);
        public void EnterSSN(string value)              => Type(ssnInput, value);
        public void EnterUsername(string value)         => Type(usernameInput, value);
        public void EnterPassword(string value)         => Type(passwordInput, value);
        public void EnterConfirmPassword(string value)  => Type(confirmPasswordInput, value);
        public void ClickRegister()                     => Click(registerButton);

        public void Register(string firstName, string lastName, string address, string city,
            string state, string zipCode, string phone, string ssn,
            string username, string password, string confirmPassword)
        {
            EnterFirstName(firstName);
            EnterLastName(lastName);
            EnterAddress(address);
            EnterCity(city);
            EnterState(state);
            EnterZipCode(zipCode);
            EnterPhone(phone);
            EnterSSN(ssn);
            EnterUsername(username);
            EnterPassword(password);
            EnterConfirmPassword(confirmPassword);
            ClickRegister();
        }

        // Error visibility checks
        public bool IsFirstNameErrorDisplayed()  => IsDisplayed(firstNameError);
        public bool IsLastNameErrorDisplayed()   => IsDisplayed(lastNameError);
        public bool IsAddressErrorDisplayed()    => IsDisplayed(addressError);
        public bool IsCityErrorDisplayed()       => IsDisplayed(cityError);
        public bool IsStateErrorDisplayed()      => IsDisplayed(stateError);
        public bool IsZipCodeErrorDisplayed()    => IsDisplayed(zipCodeError);
        public bool IsPhoneErrorDisplayed()      => IsDisplayed(phoneError);
        public bool IsSsnErrorDisplayed()        => IsDisplayed(ssnError);
        public bool IsUsernameErrorDisplayed()   => IsDisplayed(usernameError);
        public bool IsPasswordErrorDisplayed()   => IsDisplayed(passwordError);

        // Check any error is shown
        public bool IsAnyErrorDisplayed() =>
            IsFirstNameErrorDisplayed() || IsLastNameErrorDisplayed() ||
            IsAddressErrorDisplayed()   || IsCityErrorDisplayed()     ||
            IsStateErrorDisplayed()     || IsZipCodeErrorDisplayed()  ||
            IsPhoneErrorDisplayed()     || IsSsnErrorDisplayed()      ||
            IsUsernameErrorDisplayed()  || IsPasswordErrorDisplayed();

        public string GetSuccessMessage()    => GetText(successMessage);
        public bool IsWelcomeDisplayed()     => IsDisplayed(welcomeTitle);

        public string GenerateUniqueUsername() =>
            "user_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
    }
}
