using OpenQA.Selenium;

namespace ParaBankTests.Pages
{
    public class LoginPage : BasePage
    {
        private By usernameInput = By.Name("username");
        private By passwordInput = By.Name("password");
        private By loginButton = By.XPath("//input[@value='Log In']");
        private By errorMessage = By.CssSelector(".error");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void Open()
        {
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/index.htm");
        }

        public void Login(string username, string password)
        {
            Type(usernameInput, username);
            Type(passwordInput, password);
            Click(loginButton);
            // Chờ redirect khỏi trang login (URL không còn là index.htm)
            wait.Until(d => !d.Url.Contains("index.htm") || d.Url.Contains("overview"));
        }

        public bool IsUsernameInputDisplayed() => IsDisplayed(usernameInput);
        public bool IsPasswordInputDisplayed() => IsDisplayed(passwordInput);
        public bool IsLoginButtonDisplayed() => IsDisplayed(loginButton);

        public bool IsLoginButtonEnabled()
        {
            try { return driver.FindElement(loginButton).Enabled; }
            catch { return false; }
        }

        public void EnterUsername(string value) => Type(usernameInput, value);
        public void EnterPassword(string value) => Type(passwordInput, value);
        public void ClickLogin() => Click(loginButton);

        public string GetErrorMessage() => GetText(errorMessage);
        public bool IsErrorDisplayed() => IsDisplayed(errorMessage);
    }
}
