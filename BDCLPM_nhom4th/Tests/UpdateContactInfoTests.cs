using NUnit.Framework;
using ParaBankTests.Pages;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("Functional")]
    public class UpdateContactInfoTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private UpdateContactInfoPage updatePage = null!;

        // Tài khoản tạo riêng cho test suite này
        private static string _testUsername = string.Empty;
        private const string _testPassword = "Test@123";

        // Fallback dùng john/demo nếu tài khoản mới bị internal error
        private static string _activeUsername = string.Empty;
        private static string _activePassword = string.Empty;

        private const string FirstName = "Nguyen";
        private const string LastName = "Van B";
        private const string Address = "456 Nguyen Hue";
        private const string City = "Ho Chi Minh";
        private const string State = "HCM";
        private const string ZipCode = "700000";
        private const string Phone = "0901234567";

        [OneTimeSetUp]
        public void RegisterTestAccount()
        {
            var regPage = new RegisterPage(driver!);
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver!, TimeSpan.FromSeconds(10));

            for (int attempt = 0; attempt < 3; attempt++)
            {
                _testUsername = "u_" + Guid.NewGuid().ToString("N")[..12];

                regPage.Open();
                regPage.Register(
                    firstName: "Test",
                    lastName: "User",
                    address: "123 Test St",
                    city: "TestCity",
                    state: "TS",
                    zipCode: "100000",
                    phone: "0900000000",
                    ssn: "987654321",
                    username: _testUsername,
                    password: _testPassword,
                    confirmPassword: _testPassword
                );

                wait.Until(d => !d.Url.EndsWith("register.htm") || d.PageSource.Contains("Welcome") || d.PageSource.Contains("error"));

                if (driver!.PageSource.Contains("Welcome") || !driver.Url.Contains("register"))
                {
                    _activeUsername = _testUsername;
                    _activePassword = _testPassword;
                    break;
                }
            }

            // Nếu đăng ký thất bại hoàn toàn, fallback về john/demo
            if (string.IsNullOrEmpty(_activeUsername))
            {
                _activeUsername = "john";
                _activePassword = "demo";
            }
        }

        [SetUp]
        public new void SetUp()
        {
            loginPage = new LoginPage(driver!);
            updatePage = new UpdateContactInfoPage(driver!);

            // Nếu session còn (sau đăng ký hoặc test trước), thử vào thẳng
            // Nếu bị redirect hoặc internal error, login lại bằng tài khoản đã đăng ký
            driver!.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/updateprofile.htm");
            System.Threading.Thread.Sleep(1500);

            string url = driver.Url;
            string body = driver.FindElement(OpenQA.Selenium.By.TagName("body")).Text;

            bool sessionExpired = !url.Contains("updateprofile");
            bool serverError = body.Contains("An internal error");

            if (sessionExpired || serverError)
            {
                loginPage.Open();
                loginPage.Login(_activeUsername, _activePassword);
                System.Threading.Thread.Sleep(1000);
            }

            updatePage.Open();
        }

        /// <summary>
        /// TC_TS_34_01: Cập nhật thông tin với dữ liệu hợp lệ
        /// Expected: Cập nhật thành công, hiển thị thông báo
        /// </summary>
        [Test]
        [Description("TC_TS_34_01: Kiểm tra cập nhật thông tin với dữ liệu hợp lệ")]
        public void TC_TS_34_01_UpdateContactInfo_Successfully()
        {
            updatePage.ClearAndFill(FirstName, LastName, Address, City, State, ZipCode, Phone);
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsUpdateSuccessful(), Is.True,
                "Không hiển thị thông báo cập nhật thành công.");
        }

        /// <summary>
        /// TC_TS_35_01: Để trống tất cả các trường
        /// Expected: Không cập nhật thành công
        /// </summary>
        [Test]
        [Description("TC_TS_35_01: Kiểm tra khi để trống tất cả các trường – không được cập nhật thành công")]
        public void TC_TS_35_01_UpdateContactInfo_FailsWhenAllFieldsEmpty()
        {
            updatePage.ClearAndFill("", "", "", "", "", "", "");
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsNotSuccessful(), Is.True,
                "Hệ thống vẫn cập nhật thành công dù để trống tất cả các trường.");
        }

        /// <summary>
        /// TC_TS_35_02: Bỏ trống First Name
        /// Expected: Không cập nhật thành công
        /// </summary>
        [Test]
        [Description("TC_TS_35_02: Kiểm tra bỏ trống First Name – không được cập nhật thành công")]
        public void TC_TS_35_02_UpdateContactInfo_FailsWhenFirstNameEmpty()
        {
            updatePage.ClearAndFill("", LastName, Address, City, State, ZipCode, Phone);
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsNotSuccessful(), Is.True,
                "Hệ thống vẫn cập nhật thành công dù bỏ trống First Name.");
        }

        /// <summary>
        /// TC_TS_35_03: Bỏ trống Last Name
        /// Expected: Không cập nhật thành công
        /// </summary>
        [Test]
        [Description("TC_TS_35_03: Kiểm tra bỏ trống Last Name – không được cập nhật thành công")]
        public void TC_TS_35_03_UpdateContactInfo_FailsWhenLastNameEmpty()
        {
            updatePage.ClearAndFill(FirstName, "", Address, City, State, ZipCode, Phone);
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsNotSuccessful(), Is.True,
                "Hệ thống vẫn cập nhật thành công dù bỏ trống Last Name.");
        }

        /// <summary>
        /// TC_TS_35_04: Bỏ trống Address
        /// Expected: Không cập nhật thành công
        /// </summary>
        [Test]
        [Description("TC_TS_35_04: Kiểm tra bỏ trống Address – không được cập nhật thành công")]
        public void TC_TS_35_04_UpdateContactInfo_FailsWhenAddressEmpty()
        {
            updatePage.ClearAndFill(FirstName, LastName, "", City, State, ZipCode, Phone);
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsNotSuccessful(), Is.True,
                "Hệ thống vẫn cập nhật thành công dù bỏ trống Address.");
        }
    }
}
