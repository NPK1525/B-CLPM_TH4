using NUnit.Framework;
using ParaBankTests.Pages;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("Functional")]
    public class BillPayTests : BaseTest
    {
        private const string Username = "Test1234";
        private const string Password = "111";

        private LoginPage loginPage = null!;
        private BillPayPage billPayPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private string AccountNo = "";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            billPayPage = new BillPayPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);

            loginPage.Open();
            loginPage.Login(Username, Password);

            // Lấy Account ID động từ Account Overview
            accountOverviewPage.Open();
            AccountNo = accountOverviewPage.GetFirstAccountId();

            billPayPage.Open();
        }

        [Test]
        [Description("TC_TS_25_01: Thanh toán hóa đơn thành công với thông tin hợp lệ")]
        public void TC_TS_25_01_BillPay_Successfully_WithValidData()
        {
            billPayPage.FillPayeeInfo(
                "Do Mixi", "123 Main St", "New York", "NY", "10001",
                "901234567", AccountNo, "100"
            );
            billPayPage.ClickSendPayment();

            Assert.Multiple(() =>
            {
                Assert.That(billPayPage.IsPaymentSuccessful(), Is.True,
                    "Trang 'Bill Payment Complete' không hiển thị.");
                Assert.That(billPayPage.GetPayeeNameText(), Is.EqualTo("Do Mixi"),
                    "Payee name không đúng.");
                Assert.That(billPayPage.GetAmountText(), Is.EqualTo("$100.00"),
                    "Amount không đúng.");
                Assert.That(billPayPage.GetFromAccountText(), Is.EqualTo(AccountNo),
                    "From account không đúng.");
                Assert.That(billPayPage.IsFormVisible(), Is.False,
                    "#billpayForm phải ẩn sau khi thanh toán.");
                Assert.That(billPayPage.IsResultVisible(), Is.True,
                    "#billpayResult phải hiển thị.");
            });
        }

        [Test]
        [Description("TC_TS_26_01: Để trống tất cả fields – hiển thị đồng thời nhiều lỗi required")]
        public void TC_TS_26_01_BillPay_AllFieldsEmpty_ShowsAllErrors()
        {
            billPayPage.ClickSendPayment();

            Assert.Multiple(() =>
            {
                Assert.That(billPayPage.IsErrorNameDisplayed(), Is.True,
                    "Lỗi 'Payee name is required.' phải hiển thị.");
                Assert.That(billPayPage.IsErrorAddressDisplayed(), Is.True,
                    "Lỗi 'Address is required.' phải hiển thị.");
                Assert.That(billPayPage.IsErrorCityDisplayed(), Is.True,
                    "Lỗi 'City is required.' phải hiển thị.");
                Assert.That(billPayPage.IsErrorStateDisplayed(), Is.True,
                    "Lỗi 'State is required.' phải hiển thị.");
                Assert.That(billPayPage.IsErrorZipCodeDisplayed(), Is.True,
                    "Lỗi 'Zip Code is required.' phải hiển thị.");
                Assert.That(billPayPage.IsErrorPhoneDisplayed(), Is.True,
                    "Lỗi 'Phone number is required.' phải hiển thị.");
                Assert.That(billPayPage.IsErrorAmountEmptyDisplayed(), Is.True,
                    "Lỗi 'The amount cannot be empty.' phải hiển thị.");
                Assert.That(billPayPage.IsPaymentSuccessful(), Is.False,
                    "Form không được submit khi có lỗi.");
            });
        }

        [Test]
        [Description("TC_TS_26_02: Chỉ để trống Payee Name – chỉ hiện lỗi name")]
        public void TC_TS_26_02_BillPay_EmptyPayeeName_ShowsNameError()
        {
            billPayPage.FillPayeeInfo(
                "", "123 Main St", "New York", "NY", "10001",
                "0901234567", AccountNo, "100"
            );
            billPayPage.ClickSendPayment();

            Assert.Multiple(() =>
            {
                Assert.That(billPayPage.IsErrorNameDisplayed(), Is.True,
                    "Lỗi 'Payee name is required.' phải hiển thị.");
                Assert.That(billPayPage.IsErrorAddressDisplayed(), Is.False,
                    "Lỗi Address không được hiển thị.");
                Assert.That(billPayPage.IsPaymentSuccessful(), Is.False,
                    "Form không được submit.");
            });
        }

        [Test]
        [Description("TC_TS_26_04: Để trống Amount – hiện lỗi amount empty")]
        public void TC_TS_26_04_BillPay_EmptyAmount_ShowsAmountError()
        {
            billPayPage.FillPayeeInfo(
                "Do Mixi", "123 Main St", "New York", "NY", "10001",
                "0901234567", AccountNo, ""
            );
            billPayPage.ClickSendPayment();

            Assert.Multiple(() =>
            {
                Assert.That(billPayPage.IsErrorAmountEmptyDisplayed(), Is.True,
                    "Lỗi 'The amount cannot be empty.' phải hiển thị.");
                Assert.That(billPayPage.IsPaymentSuccessful(), Is.False,
                    "Form không được submit.");
            });
        }
        [Test]
        [Description("TC_TS_27_01: Account # và Verify Account # không khớp – hiện lỗi mismatch")]
        public void TC_TS_27_01_BillPay_AccountMismatch_ShowsMismatchError()
        {
            billPayPage.FillPayeeInfoWithMismatch(
                "Electric Company", "123 Main St", "New York", "NY", "10001",
                "0901234567", AccountNo, "99999", "100"
            );
            billPayPage.ClickSendPayment();

            Assert.Multiple(() =>
            {
                Assert.That(billPayPage.IsErrorVerifyMismatchDisplayed(), Is.True,
                    "Lỗi 'The account numbers do not match.' phải hiển thị.");
                Assert.That(billPayPage.IsPaymentSuccessful(), Is.False,
                    "Form không được submit khi account mismatch.");
            });
        }

        [Test]
        [Description("TC_TS_28_01: Amount nhập chữ – hiện lỗi invalid amount")]
        public void TC_TS_28_01_BillPay_InvalidAmount_ShowsAmountInvalidError()
        {
            billPayPage.FillPayeeInfo(
                "Electric Company", "123 Main St", "New York", "NY", "10001",
                "0901234567", AccountNo, "abc"
            );
            billPayPage.ClickSendPayment();

            Assert.Multiple(() =>
            {
                Assert.That(billPayPage.IsErrorAmountInvalidDisplayed(), Is.True,
                    "Lỗi 'Please enter a valid amount.' phải hiển thị.");
                Assert.That(billPayPage.IsPaymentSuccessful(), Is.False,
                    "Form không được submit.");
            });
        }

        [Test]
        [Description("TC_TS_29_01: Kiểm tra nội dung span payeeName, amount, fromAccountId sau khi thành công")]
        public void TC_TS_29_01_BillPay_SuccessResult_ShowsCorrectContent()
        {
            billPayPage.FillPayeeInfo(
                "Water Supply", "456 River Rd", "Boston", "MA", "02101",
                "0912345678", AccountNo, "75"
            );
            billPayPage.ClickSendPayment();

            Assert.Multiple(() =>
            {
                Assert.That(billPayPage.GetPayeeNameText(), Is.EqualTo("Water Supply"),
                    "span#payeeName phải là 'Water Supply'.");
                Assert.That(billPayPage.GetAmountText(), Is.EqualTo("$75.00"),
                    "span#amount phải là '$75.00'.");
                Assert.That(billPayPage.GetFromAccountText(), Is.EqualTo(AccountNo),
                    $"span#fromAccountId phải là '{AccountNo}'.");
            });
        }
    }
}