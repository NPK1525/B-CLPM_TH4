using BDCLPM_nhom4th.Pages;
using ParaBankTests.Pages;
using ParaBankTests.Tests;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("AccountOverview")]
    public class AccountOverviewTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private AccountDetailsPage accountDetailsPage = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            accountDetailsPage = new AccountDetailsPage(driver!);

            loginPage.Open();
            loginPage.Login(
                TestDataHelper.Get("validUser", "username"),
                TestDataHelper.Get("validUser", "password"));
        }

        [Test]
        [Description("TS_11: Account Overview - Kiểm tra điều hướng sau đăng nhập")]
        public void TC_TS_11_01_NavigationLinks_WorkCorrectly()
        {
            Assert.Multiple(() =>
            {
                Assert.That(accountOverviewPage.IsTransferFundsLinkDisplayed(), Is.True, "Transfer Funds link must be visible.");
                Assert.That(accountOverviewPage.IsBillPayLinkDisplayed(), Is.True, "Bill Pay link must be visible.");
                Assert.That(accountOverviewPage.IsOpenAccountLinkDisplayed(), Is.True, "Open New Account link must be visible.");
            });

            accountOverviewPage.GoToTransferFunds();
            Assert.That(driver!.Url.Contains("transfer.htm"), Is.True, "Failed to navigate to Transfer Funds page.");
        }

        [Test]
        [Description("TS_12: Account Overview - Kiểm tra hiển thị danh sách tài khoản")]
        public void TC_TS_12_01_AccountList_Displayed()
        {
            int rowCount = accountOverviewPage.GetAccountRowsCount();
            Assert.That(rowCount, Is.GreaterThan(0), "Account table is empty or not displayed correctly.");
        }

        [Test]
        [Description("TS_13: Account Overview - Kiểm tra hiển thị số dư")]
        public void TC_TS_13_01_Balance_DisplayedCorrectly()
        {
            string totalBalance = accountOverviewPage.GetTotalBalance();
            Assert.That(string.IsNullOrEmpty(totalBalance), Is.False, "Total balance should not be empty.");
            Assert.That(totalBalance.StartsWith("$"), Is.True, "Total balance should be formatted correctly (start with $).");
        }

        [Test]
        [Description("TS_14: Account Overview - Kiểm tra chuyển đến chi tiết tài khoản")]
        public void TC_TS_14_01_NavigateToAccountDetails()
        {
            accountOverviewPage.ClickFirstAccount();
            Assert.That(accountDetailsPage.IsAtAccountDetails(), Is.True, "Failed to navigate to Account Details page.");
            Assert.That(driver!.Url.Contains("activity.htm"), Is.True, "URL doesn't match activity/details page pattern.");
        }

        [Test]
        [Description("TS_15: Account Overview - Kiểm tra hiển thị thông tin tài khoản")]
        public void TC_TS_15_01_AccountInfo_Displayed()
        {
            accountOverviewPage.ClickFirstAccount();
            Thread.Sleep(1000);

            Assert.Multiple(() =>
            {
                Assert.That(string.IsNullOrEmpty(accountDetailsPage.GetAccountId()), Is.False, "Account ID is missing");
                Assert.That(string.IsNullOrEmpty(accountDetailsPage.GetAccountType()), Is.False, "Account Type is missing");
                Assert.That(accountDetailsPage.GetBalance().StartsWith("$"), Is.True, "Balance format is incorrect");
                Assert.That(accountDetailsPage.GetAvailableBalance().StartsWith("$"), Is.True, "Available balance format is incorrect");
            });
        }

        [Test]
        [Description("TS_16: Account Overview - Kiểm tra lịch sử giao dịch")]
        public void TC_TS_16_01_TransactionHistory_Displayed()
        {
            accountOverviewPage.ClickFirstAccount();
            Thread.Sleep(500);

            int transactionCount = accountDetailsPage.GetTransactionsCount();
            Assert.That(transactionCount, Is.GreaterThanOrEqualTo(0), "Transaction table should load properly (even if empty).");
        }
    }
}