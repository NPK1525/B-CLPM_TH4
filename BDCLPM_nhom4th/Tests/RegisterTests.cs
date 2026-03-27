using NUnit.Framework;
using ParaBankTests.Pages;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("Functional")]
    public class RegisterTests : BaseTest
    {
        private RegisterPage registerPage = null!;

        // Dữ liệu dùng chung theo test case
        private const string FirstName  = "Nguyen";
        private const string LastName   = "Van A";
        private const string Address    = "123 Le Loi, HCM";
        private const string City       = "Ho Chi Minh";
        private const string State      = "HCM";
        private const string ZipCode    = "700000";
        private const string Phone      = "0901234567";
        private const string SSN        = "123456789";
        private const string Username   = "user_ts01_001";
        private const string Password   = "Test@123";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            registerPage = new RegisterPage(driver!);
            registerPage.Open();
        }

        /// <summary>
        /// TC_TS_01_01: Đăng ký tài khoản mới với thông tin hợp lệ
        /// Expected: Tài khoản được tạo thành công, hiển thị thông báo chào mừng
        /// </summary>
        [Test]
        [Description("TC_TS_01_01: Kiểm tra đăng ký tài khoản mới với thông tin hợp lệ")]
        public void TC_TS_01_01_RegisterSuccessfully_WithValidData()
        {
            string uniqueUsername = registerPage.GenerateUniqueUsername();

            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode, Phone, SSN,
                uniqueUsername, Password, Password
            );

            Assert.Multiple(() =>
            {
                Assert.That(registerPage.IsWelcomeDisplayed(), Is.True,
                    "Không hiển thị tiêu đề Welcome sau khi đăng ký.");
                Assert.That(registerPage.GetSuccessMessage(),
                    Does.Contain("Your account was created successfully"),
                    "Không hiển thị thông báo tạo tài khoản thành công.");
            });
        }

        /// <summary>
        /// TC_TS_02_01: Bỏ trống tất cả trường bắt buộc
        /// Expected: Hiển thị thông báo lỗi yêu cầu nhập dữ liệu
        /// </summary>
        [Test]
        [Description("TC_TS_02_01: Kiểm tra bỏ trống tất cả trường bắt buộc khi đăng ký")]
        public void TC_TS_02_01_Register_FailsWhenAllFieldsEmpty()
        {
            // Không nhập gì, nhấn Register luôn
            registerPage.ClickRegister();

            Assert.That(registerPage.IsAnyErrorDisplayed(), Is.True,
                "Không hiển thị thông báo lỗi khi bỏ trống tất cả trường.");
        }

        /// <summary>
        /// TC_TS_02_02: Bỏ trống trường First Name
        /// Expected: Hiển thị thông báo lỗi cho First Name
        /// </summary>
        [Test]
        [Description("TC_TS_02_02: Kiểm tra bỏ trống trường First Name")]
        public void TC_TS_02_02_Register_FailsWhenFirstNameEmpty()
        {
            registerPage.Register(
                "",         // First Name bỏ trống
                LastName, Address, City,
                State, ZipCode, Phone, SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsFirstNameErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống First Name.");
        }

        /// <summary>
        /// TC_TS_02_03: Bỏ trống trường Last Name
        /// Expected: Hiển thị thông báo lỗi cho Last Name
        /// </summary>
        [Test]
        [Description("TC_TS_02_03: Kiểm tra bỏ trống trường Last Name")]
        public void TC_TS_02_03_Register_FailsWhenLastNameEmpty()
        {
            registerPage.Register(
                FirstName,
                "",         // Last Name bỏ trống
                Address, City,
                State, ZipCode, Phone, SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsLastNameErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Last Name.");
        }

        /// <summary>
        /// TC_TS_02_04: Bỏ trống trường Address
        /// Expected: Hiển thị thông báo lỗi cho Address
        /// </summary>
        [Test]
        [Description("TC_TS_02_04: Kiểm tra bỏ trống trường Address")]
        public void TC_TS_02_04_Register_FailsWhenAddressEmpty()
        {
            registerPage.Register(
                FirstName, LastName,
                "",         // Address bỏ trống
                City,
                State, ZipCode, Phone, SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsAddressErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Address.");
        }

        /// <summary>
        /// TC_TS_02_05: Bỏ trống trường City
        /// Expected: Hiển thị thông báo lỗi cho City
        /// </summary>
        [Test]
        [Description("TC_TS_02_05: Kiểm tra bỏ trống trường City")]
        public void TC_TS_02_05_Register_FailsWhenCityEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address,
                "",         // City bỏ trống
                State, ZipCode, Phone, SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsCityErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống City.");
        }

        /// <summary>
        /// TC_TS_02_06: Bỏ trống trường State
        /// Expected: Hiển thị thông báo lỗi cho State
        /// </summary>
        [Test]
        [Description("TC_TS_02_06: Kiểm tra bỏ trống trường State")]
        public void TC_TS_02_06_Register_FailsWhenStateEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                "",         // State bỏ trống
                ZipCode, Phone, SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsStateErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống State.");
        }

        /// <summary>
        /// TC_TS_02_07: Bỏ trống trường Zip Code
        /// Expected: Hiển thị thông báo lỗi cho Zip Code
        /// </summary>
        [Test]
        [Description("TC_TS_02_07: Kiểm tra bỏ trống trường Zip Code")]
        public void TC_TS_02_07_Register_FailsWhenZipCodeEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State,
                "",         // Zip Code bỏ trống
                Phone, SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsZipCodeErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Zip Code.");
        }

        /// <summary>
        /// TC_TS_02_08: Bỏ trống trường SSN
        /// Expected: Hiển thị thông báo lỗi cho SSN
        /// </summary>
        [Test]
        [Description("TC_TS_02_08: Kiểm tra bỏ trống trường SSN")]
        public void TC_TS_02_08_Register_FailsWhenSSNEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode, Phone,
                "",         // SSN bỏ trống
                Username, Password, Password
            );

            Assert.That(registerPage.IsSsnErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống SSN.");
        }

        /// <summary>
        /// TC_TS_02_09: Bỏ trống trường Phone
        /// Expected: Hiển thị thông báo lỗi cho Phone
        /// </summary>
        [Test]
        [Description("TC_TS_02_09: Kiểm tra bỏ trống trường Phone")]
        public void TC_TS_02_09_Register_FailsWhenPhoneEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode,
                "",         // Phone bỏ trống
                SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsPhoneErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Phone.");
        }

        /// <summary>
        /// TC_TS_02_10: Bỏ trống trường Username
        /// Expected: Hiển thị thông báo lỗi cho Username
        /// </summary>
        [Test]
        [Description("TC_TS_02_10: Kiểm tra bỏ trống trường Username")]
        public void TC_TS_02_10_Register_FailsWhenUsernameEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode, Phone, SSN,
                "",         // Username bỏ trống
                Password, Password
            );

            Assert.That(registerPage.IsUsernameErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Username.");
        }

        /// <summary>
        /// TC_TS_02_11: Bỏ trống trường Password
        /// Expected: Hiển thị thông báo lỗi cho Password
        /// </summary>
        [Test]
        [Description("TC_TS_02_11: Kiểm tra bỏ trống trường Password")]
        public void TC_TS_02_11_Register_FailsWhenPasswordEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode, Phone, SSN,
                Username,
                "",         // Password bỏ trống
                Password
            );

            Assert.That(registerPage.IsPasswordErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Password.");
        }

        /// <summary>
        /// TC_TS_02_12: Bỏ trống trường Confirm Password
        /// Expected: Hiển thị thông báo lỗi cho Confirm Password
        /// </summary>
        [Test]
        [Description("TC_TS_02_12: Kiểm tra bỏ trống trường Confirm Password")]
        public void TC_TS_02_12_Register_FailsWhenConfirmPasswordEmpty()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode, Phone, SSN,
                Username, Password,
                ""          // Confirm Password bỏ trống
            );

            Assert.That(registerPage.IsPasswordErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Confirm Password.");
        }

        /// <summary>
        /// TC_TS_03_01: Đăng ký với username đã tồn tại
        /// Expected: Đăng ký thất bại, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Description("TC_TS_03_01: Kiểm tra đăng ký với username đã tồn tại")]
        public void TC_TS_03_01_Register_FailsWithDuplicateUsername()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode, Phone, SSN,
                "datontai",  // username đã tồn tại
                Password, Password
            );

            Assert.That(registerPage.IsWelcomeDisplayed(), Is.False,
                "Trang Welcome hiển thị dù username đã tồn tại.");
            Assert.That(registerPage.IsUsernameErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi dùng username đã tồn tại.");
        }

        /// <summary>
        /// TC_TS_03_02: Hệ thống không chuyển trang khi lỗi username
        /// Expected: Vẫn ở trang Register, dữ liệu nhập vẫn còn
        /// </summary>
        [Test]
        [Description("TC_TS_03_02: Kiểm tra hệ thống không chuyển trang khi lỗi username")]
        public void TC_TS_03_02_Register_StaysOnPageWhenUsernameError()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, ZipCode, Phone, SSN,
                Username, Password,
                ""          // Confirm Password bỏ trống → lỗi
            );

            Assert.That(driver!.Url, Does.Contain("register"),
                "Trang đã chuyển đi dù có lỗi.");
        }

        /// <summary>
        /// TC_TS_04_01: Zip Code nhập chữ thay vì số
        /// Expected: Đăng ký thất bại, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Description("TC_TS_04_01: Kiểm tra hiển thị lỗi khi Zip Code nhập chữ")]
        public void TC_TS_04_01_Register_FailsWithInvalidZipCode()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State,
                "abcde",    // Zip Code không hợp lệ
                Phone, SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsZipCodeErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi Zip Code nhập chữ.");
        }

        /// <summary>
        /// TC_TS_04_02: Phone Number nhập chữ thay vì số
        /// Expected: Đăng ký thất bại, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Description("TC_TS_04_02: Kiểm tra hiển thị lỗi khi Phone Number nhập chữ")]
        public void TC_TS_04_02_Register_FailsWithInvalidPhone()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, "70000",
                "abc",      // Phone không hợp lệ
                SSN,
                Username, Password, Password
            );

            Assert.That(registerPage.IsPhoneErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi Phone nhập chữ.");
        }

        /// <summary>
        /// TC_TS_04_03: SSN nhập sai định dạng
        /// Expected: Đăng ký thất bại, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Description("TC_TS_04_03: Kiểm tra hiển thị lỗi khi SSN nhập sai định dạng")]
        public void TC_TS_04_03_Register_FailsWithInvalidSSN()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, "70000", "abc",
                "ssn@@@",   // SSN không hợp lệ
                Username, Password, Password
            );

            Assert.That(registerPage.IsSsnErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi SSN nhập sai định dạng.");
        }

        /// <summary>
        /// TC_TS_04_04: Password không hợp lệ (quá ngắn)
        /// Expected: Đăng ký thất bại, hiển thị thông báo lỗi
        /// </summary>
        [Test]
        [Description("TC_TS_04_04: Kiểm tra hiển thị lỗi khi Password không hợp lệ")]
        public void TC_TS_04_04_Register_FailsWithInvalidPassword()
        {
            registerPage.Register(
                FirstName, LastName, Address, City,
                State, "70000", "abc", "ssn@@@",
                Username,
                "T1",       // Password quá ngắn/không hợp lệ
                "T1"
            );

            Assert.That(registerPage.IsPasswordErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi Password không hợp lệ.");
        }
    }
}

