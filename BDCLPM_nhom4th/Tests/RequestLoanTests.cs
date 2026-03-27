using NUnit.Framework;
using ParaBankTests.Pages;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("Functional")]
    public class RequestLoanTests : BaseTest
    {
        private const string Username = "Test1234";
        private const string Password = "111";

        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private RequestLoanPage requestLoanPage = null!;
        private string FromAccount = "";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            requestLoanPage = new RequestLoanPage(driver!);

            // Đăng nhập trước mỗi test
            loginPage.Open();
            loginPage.Login(Username, Password);

            // Lấy Account ID động
            accountOverviewPage.Open();
            FromAccount = accountOverviewPage.GetFirstAccountId();

            // Vào trang Request Loan
            requestLoanPage.Open();
        }

        [Test]
        [Description("TC_TS_38_01: Gửi yêu cầu vay thành công – khoản vay được Approved")]
        public void TC_TS_38_01_RequestLoan_Approved_WithValidData()
        {
            requestLoanPage.FillAndSubmit("1000", "100", FromAccount);
            requestLoanPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(requestLoanPage.IsFormVisible(), Is.False,
                    "#requestLoanForm phải ẩn sau khi submit.");
                Assert.That(requestLoanPage.IsResultVisible(), Is.True,
                    "#requestLoanResult phải hiển thị.");
                Assert.That(requestLoanPage.GetLoanStatus(), Is.EqualTo("Approved"),
                    "Status phải là 'Approved'.");
                Assert.That(requestLoanPage.IsApprovedVisible(), Is.True,
                    "#loanRequestApproved phải hiển thị.");
                Assert.That(requestLoanPage.GetLoanProvider(), Is.Not.Empty,
                    "Loan Provider không được rỗng.");
            });
        }

 
        [Test]
        [Description("TC_TS_38_02: Kiểm tra link tài khoản mới sau khi Approved")]
        public void TC_TS_38_02_RequestLoan_Approved_NewAccountLink()
        {
            requestLoanPage.FillAndSubmit("1000", "100", FromAccount);
            requestLoanPage.WaitForResult();

            var accountId = requestLoanPage.GetNewAccountId();
            var href = requestLoanPage.GetNewAccountHref();

            Assert.Multiple(() =>
            {
                Assert.That(accountId, Is.Not.Empty,
                    "Account ID mới không được rỗng.");
                Assert.That(href, Does.Contain($"/parabank/activity.htm?id={accountId}"),
                    "href của link phải trỏ đến activity.htm?id={accountId}.");
            });

            driver!.FindElement(OpenQA.Selenium.By.Id("newAccountId")).Click();
            Assert.That(driver.Url, Does.Contain($"activity.htm?id={accountId}"),
                "URL sau khi click phải chứa đúng account ID.");
        }
        [Test]
        [Description("TC_TS_38_03: #requestLoanForm ẩn, #requestLoanResult hiển thị, #requestLoanError ẩn")]
        public void TC_TS_38_03_RequestLoan_ContainersVisibility_AfterApproved()
        {
            requestLoanPage.FillAndSubmit("1000", "100", FromAccount);
            requestLoanPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(requestLoanPage.IsFormVisible(), Is.False,
                    "#requestLoanForm phải display:none.");
                Assert.That(requestLoanPage.IsResultVisible(), Is.True,
                    "#requestLoanResult phải visible.");
                Assert.That(requestLoanPage.IsErrorVisible(), Is.False,
                    "#requestLoanError phải display:none.");
            });
        }


        [Test]
        [Description("TC_TS_40_01: Denied khi Down Payment vượt số dư tài khoản")]
        public void TC_TS_40_01_RequestLoan_Denied_DownPaymentExceedsBalance()
        {
            requestLoanPage.FillAndSubmit("5000", "999999", FromAccount);
            requestLoanPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(requestLoanPage.GetLoanStatus(), Is.EqualTo("Denied"),
                    "Status phải là 'Denied'.");
                Assert.That(requestLoanPage.IsDeniedVisible(), Is.True,
                    "#loanRequestDenied phải hiển thị.");
                Assert.That(requestLoanPage.GetDeniedErrorMessage(),
                    Does.Contain("You do not have sufficient funds for the given down payment."),
                    "Thông báo lỗi không đúng.");
            });
        }

        [Test]
        [Description("TC_TS_40_02: Denied khi cả Loan Amount và Down Payment đều quá lớn")]
        public void TC_TS_40_02_RequestLoan_Denied_BothAmountsExceed()
        {
            requestLoanPage.FillAndSubmit("9999999", "9999999", FromAccount);
            requestLoanPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(requestLoanPage.GetLoanStatus(), Is.EqualTo("Denied"),
                    "Status phải là 'Denied'.");
                Assert.That(requestLoanPage.GetDeniedErrorMessage(),
                    Does.Contain("We cannot grant a loan in that amount with your available funds and down payment."),
                    "Thông báo lỗi không đúng.");
            });
        }

        [Test]
        [Description("TC_TS_39_01: Để trống Loan Amount – server trả về lỗi hệ thống")]
        public void TC_TS_39_01_RequestLoan_Error_WhenLoanAmountEmpty()
        {
            requestLoanPage.FillAndSubmit("", "100", FromAccount);
            requestLoanPage.WaitForResult();

            Assert.That(requestLoanPage.IsErrorVisible(), Is.True,
                "#requestLoanError phải hiển thị khi Loan Amount rỗng.");
        }

        [Test]
        [Description("TC_TS_39_02: Để trống Down Payment – server trả về lỗi hệ thống")]
        public void TC_TS_39_02_RequestLoan_Error_WhenDownPaymentEmpty()
        {
            requestLoanPage.FillAndSubmit("1000", "", FromAccount);
            requestLoanPage.WaitForResult();

            Assert.That(requestLoanPage.IsErrorVisible(), Is.True,
                "#requestLoanError phải hiển thị khi Down Payment rỗng.");
        }
    }
}