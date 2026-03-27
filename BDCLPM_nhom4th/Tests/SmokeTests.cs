using NUnit.Framework;
using ParaBankTests.Pages;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    /// <summary>
    /// Smoke Test: kiểm tra nhanh hệ thống có hoạt động trước khi chạy các test khác.
    /// </summary>
    [TestFixture]
    [Category("Smoke")]
    public class SmokeTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private TransferFundsPage transferFundsPage = null!;

        private string validUsername = null!;
        private string validPassword = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            transferFundsPage = new TransferFundsPage(driver!);

            validUsername = TestDataHelper.Get("validUser", "username");
            validPassword = TestDataHelper.Get("validUser", "password");
        }

        [Test]
        [Description("TC_SM_01: Smoke - Đăng nhập thành công")]
        public void TC_SM_01_Smoke_LoginSuccess()
        {
            loginPage.Open();
            loginPage.Login(validUsername, validPassword);

            Assert.That(accountOverviewPage.IsAtAccountOverview(), Is.True,
                "Smoke test thất bại: Không vào được trang Accounts Overview.");
        }

        [Test]
        [Description("TC_SM_02: Smoke - Logout thành công")]
        public void TC_SM_02_Smoke_LogoutSuccess()
        {
            loginPage.Open();
            loginPage.Login(validUsername, validPassword);
            accountOverviewPage.Logout();

            Assert.That(loginPage.IsLoginButtonDisplayed(), Is.True,
                "Smoke test thất bại: Không quay về trang Login sau khi logout.");
        }

        [Test]
        [Description("TC_SM_03: Smoke - Transfer Funds page load được sau khi đăng nhập")]
        public void TC_SM_03_Smoke_TransferFundsPageLoads()
        {
            loginPage.Open();
            loginPage.Login(validUsername, validPassword);
            accountOverviewPage.GoToTransferFunds();

            Assert.That(transferFundsPage.IsOnPage(), Is.True,
                "Smoke test thất bại: Trang Transfer Funds không load được.");
        }
    }
}
