using OpenQA.Selenium;

namespace ParaBankTests.Pages
{
    public class HomePage : BasePage
    {
        private readonly string baseUrl = "https://parabank.parasoft.com/parabank/index.htm";

        private By registerLink = By.LinkText("Register");
        private By usernameInput = By.Name("username");
        private By passwordInput = By.Name("password");
        private By loginButton = By.XPath("//input[@value='Log In']");
        private By logoutLink = By.LinkText("Log Out");
        private By errorMessage = By.CssSelector(".error");

        public HomePage(IWebDriver driver) : base(driver) { }

        public void Open()
        {
            driver.Navigate().GoToUrl(baseUrl);
        }

        public void GoToRegisterPage()
        {
            Click(registerLink);
        }

        public void Login(string username, string password)
        {
            Type(usernameInput, username);
            Type(passwordInput, password);
            Click(loginButton);
        }

        public bool IsLogoutDisplayed()
        {
            return IsDisplayed(logoutLink);
        }

        public string GetLoginErrorMessage()
        {
            return GetText(errorMessage);
        }
    }
}