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

        // Dữ liệu dùng chung
        private const string FirstName = "Nguyen";
        private const string LastName = "Van B";
        private const string Address = "456 Nguyen Hue";
        private const string City = "Ho Chi Minh";
        private const string State = "HCM";
        private const string ZipCode = "700000";
        private const string Phone = "0901234567";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            updatePage = new UpdateContactInfoPage(driver!);

            // Đăng nhập trước mỗi test
            loginPage.Open();
            loginPage.Login(
                TestDataHelper.Get("validUser", "username"),
                TestDataHelper.Get("validUser", "password"));

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
        /// Expected: Hiển thị thông báo lỗi cho các trường bắt buộc
        /// </summary>
        [Test]
        [Description("TC_TS_35_01: Kiểm tra khi để trống tất cả các trường")]
        public void TC_TS_35_01_UpdateContactInfo_FailsWhenAllFieldsEmpty()
        {
            updatePage.ClearAndFill("", "", "", "", "", "", "");
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsAnyErrorDisplayed(), Is.True,
                "Your updated address and phone number have been added to the system..");
        }

        /// <summary>
        /// TC_TS_35_02: Bỏ trống First Name
        /// Expected: Hiển thị thông báo lỗi cho First Name
        /// </summary>
        [Test]
        [Description("TC_TS_35_02: Kiểm tra bỏ trống First Name")]
        public void TC_TS_35_02_UpdateContactInfo_FailsWhenFirstNameEmpty()
        {
            updatePage.ClearAndFill("", LastName, Address, City, State, ZipCode, Phone);
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsFirstNameErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống First Name.");
        }

        /// <summary>
        /// TC_TS_35_03: Bỏ trống Last Name
        /// Expected: Hiển thị thông báo lỗi cho Last Name
        /// </summary>
        [Test]
        [Description("TC_TS_35_03: Kiểm tra bỏ trống Last Name")]
        public void TC_TS_35_03_UpdateContactInfo_FailsWhenLastNameEmpty()
        {
            updatePage.ClearAndFill(FirstName, "", Address, City, State, ZipCode, Phone);
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsLastNameErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Last Name.");
        }

        /// <summary>
        /// TC_TS_35_04: Bỏ trống Address
        /// Expected: Hiển thị thông báo lỗi cho Address
        /// </summary>
        [Test]
        [Description("TC_TS_35_04: Kiểm tra bỏ trống Address")]
        public void TC_TS_35_04_UpdateContactInfo_FailsWhenAddressEmpty()
        {
            updatePage.ClearAndFill(FirstName, LastName, "", City, State, ZipCode, Phone);
            updatePage.ClickUpdateProfile();

            Assert.That(updatePage.IsAddressErrorDisplayed(), Is.True,
                "Không hiển thị lỗi khi bỏ trống Address.");
        }
    }
}
