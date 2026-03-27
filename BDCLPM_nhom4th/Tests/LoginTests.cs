using NUnit.Framework;
using ParaBankTests.Pages;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private RegisterPage registerPage = null!;

        // Username được tạo 1 lần, dùng chung cho cả class
        private static string CreatedUsername = null!;
        private const string ValidPassword   = "Khang@2026";
        private const string WrongPassword   = "abc123";
        private const string WrongUsername   = "TestTHsai";

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            // Đăng ký tài khoản 1 lần duy nhất cho cả test class
            CreatedUsername = "TestTHkhang_" + DateTime.Now.ToString("HHmmss");

            registerPage = new RegisterPage(driver!);
            registerPage.Open();
            registerPage.Register(
                "Test", "Khang",
                "123 Le Loi", "Ho Chi Minh", "HCM", "700000",
                "0901234567", "123456789",
                CreatedUsername, ValidPassword, ValidPassword
            );

            // Logout sau khi đăng ký
            var overview = new AccountOverviewPage(driver!);
            overview.Logout();
        }

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            loginPage.Open();
        }

        /// <summary>
        /// TC_TS_05_01: Đăng nhập thành công với username và password hợp lệ
        /// Expected: Chuyển đến Accounts Overview, có menu hệ thống cùng nút logout
        /// </summary>
        [Test]
        [Category("Smoke")]
        [Description("TC_TS_05_01: Kiểm tra đăng nhập thành công với username và password hợp lệ")]
        public void TC_TS_05_01_LoginSuccessfully_WithValidCredentials()
        {
            loginPage.Login(CreatedUsername, ValidPassword);

            Assert.Multiple(() =>
            {
                Assert.That(accountOverviewPage.IsAtAccountOverview(), Is.True,
                    "Không chuyển đến trang Accounts Overview sau khi đăng nhập.");
                Assert.That(accountOverviewPage.IsLogoutVisible(), Is.True,
                    "Nút Logout không hiển thị sau khi đăng nhập.");
            });
        }

        /// <summary>
        /// TC_TS_06_01: Đăng nhập đúng Username nhưng sai Password
        /// Expected: Vẫn ở trang Login, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Category("Functional")]
        [Description("TC_TS_06_01: Kiểm tra đăng nhập khi nhập đúng Username nhưng sai Password")]
        public void TC_TS_06_01_Login_FailsWithCorrectUsernameWrongPassword()
        {
            loginPage.Login(CreatedUsername, WrongPassword);

            Assert.Multiple(() =>
            {
                Assert.That(loginPage.IsErrorDisplayed(), Is.True,
                    "Không hiển thị thông báo lỗi khi sai Password.");
                Assert.That(accountOverviewPage.IsAtAccountOverview(), Is.False,
                    "Đã vào được trang Overview dù sai Password.");
            });
        }

        /// <summary>
        /// TC_TS_06_02: Đăng nhập sai Username nhưng đúng Password
        /// Expected: Vẫn ở trang Login, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Category("Functional")]
        [Description("TC_TS_06_02: Kiểm tra đăng nhập khi nhập sai Username nhưng đúng Password")]
        public void TC_TS_06_02_Login_FailsWithWrongUsernameCorrectPassword()
        {
            loginPage.Login(WrongUsername, ValidPassword);

            Assert.Multiple(() =>
            {
                Assert.That(loginPage.IsErrorDisplayed(), Is.True,
                    "Không hiển thị thông báo lỗi khi sai Username.");
                Assert.That(accountOverviewPage.IsAtAccountOverview(), Is.False,
                    "Đã vào được trang Overview dù sai Username.");
            });
        }

        /// <summary>
        /// TC_TS_07_01: Không nhập Username và Password
        /// Expected: Vẫn ở trang Login, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Category("Functional")]
        [Description("TC_TS_07_01: Kiểm tra khi không nhập Username và Password")]
        public void TC_TS_07_01_Login_FailsWithEmptyUsernameAndPassword()
        {
            loginPage.ClickLogin();

            Assert.Multiple(() =>
            {
                Assert.That(loginPage.IsErrorDisplayed(), Is.True,
                    "Không hiển thị thông báo lỗi khi bỏ trống cả Username và Password.");
                Assert.That(accountOverviewPage.IsAtAccountOverview(), Is.False,
                    "Đã vào được trang Overview dù không nhập gì.");
            });
        }

        /// <summary>
        /// TC_TS_07_02: Không nhập Username, có Password
        /// Expected: Vẫn ở trang Login, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Category("Functional")]
        [Description("TC_TS_07_02: Kiểm tra khi không nhập Username")]
        public void TC_TS_07_02_Login_FailsWithEmptyUsername()
        {
            loginPage.EnterPassword(ValidPassword);
            loginPage.ClickLogin();

            Assert.Multiple(() =>
            {
                Assert.That(loginPage.IsErrorDisplayed(), Is.True,
                    "Không hiển thị thông báo lỗi khi bỏ trống Username.");
                Assert.That(accountOverviewPage.IsAtAccountOverview(), Is.False,
                    "Đã vào được trang Overview dù không nhập Username.");
            });
        }

        /// <summary>
        /// TC_TS_07_03: Có Username, không nhập Password
        /// Expected: Vẫn ở trang Login, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Category("Functional")]
        [Description("TC_TS_07_03: Kiểm tra khi không nhập Password")]
        public void TC_TS_07_03_Login_FailsWithEmptyPassword()
        {
            loginPage.EnterUsername(CreatedUsername);
            loginPage.ClickLogin();

            Assert.Multiple(() =>
            {
                Assert.That(loginPage.IsErrorDisplayed(), Is.True,
                    "Không hiển thị thông báo lỗi khi bỏ trống Password.");
                Assert.That(accountOverviewPage.IsAtAccountOverview(), Is.False,
                    "Đã vào được trang Overview dù không nhập Password.");
            });
        }
    }
}
