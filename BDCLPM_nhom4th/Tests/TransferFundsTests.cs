using NUnit.Framework;
using ParaBankTests.Pages;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("TransferFunds")]
    public class TransferFundsTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private TransferFundsPage transferFundsPage = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            transferFundsPage = new TransferFundsPage(driver!);

            loginPage.Open();
            loginPage.Login(
                TestDataHelper.Get("validUser", "username"),
                TestDataHelper.Get("validUser", "password"));

            accountOverviewPage.GoToTransferFunds();
        }

        [Test]
        [Description("TS_21: Transfer Funds - Chuyển tiền thành công với số tiền hợp lệ")]
        public void TC_TS_21_01_TransferFunds_Successfully()
        {
            Thread.Sleep(1000);
            transferFundsPage.Transfer("10");
            Assert.That(transferFundsPage.IsTransferSuccessful(), Is.True, "Chuyển tiền không thành công.");
        }

        [Test]
        [Description("TS_22: Transfer Funds - Hiển thị lỗi khi số tiền không hợp lệ")]
        public void TC_TS_22_01_TransferWithInvalidAmount()
        {
            Thread.Sleep(1000);
            transferFundsPage.Transfer("abc");

            bool hasError = transferFundsPage.HasAmountError();
            if (!hasError)
                Assert.That(transferFundsPage.IsTransferSuccessful(), Is.False, "Lỗi: Vẫn chuyển tiền thành công dù lượng tiền là chữ.");
            else
                Assert.That(hasError, Is.True, "Lỗi amount không hiển thị.");
        }

        [Test]
        [Description("TS_23: Transfer Funds - Kiểm tra chọn tài khoản nguồn/đích")]
        public void TC_TS_23_01_SelectSourceAndDestinationAccounts()
        {
            Thread.Sleep(1000);
            var fromAccounts = transferFundsPage.GetFromAccountIds();
            var toAccounts = transferFundsPage.GetToAccountIds();

            Assert.Multiple(() =>
            {
                Assert.That(fromAccounts.Count, Is.GreaterThan(0), "Không có tài khoản nguồn.");
                Assert.That(toAccounts.Count, Is.GreaterThan(0), "Không có tài khoản đích.");
            });

            Assert.DoesNotThrow(() => transferFundsPage.SelectFromAccount(fromAccounts[0]));
            if (toAccounts.Count > 1)
                Assert.DoesNotThrow(() => transferFundsPage.SelectToAccount(toAccounts[1]));
        }

        [Test]
        [Description("TS_24: Transfer Funds - Kiểm tra cập nhật số dư sau khi chuyển tiền")]
        public void TC_TS_24_01_BalancesUpdatedCorrectly()
        {
            accountOverviewPage.GoToAccountOverview();
            string accountId = accountOverviewPage.GetFirstAccountId();
            string balanceStrFirst = accountOverviewPage.GetBalanceForAccount(accountId);
            float balanceFirst = float.Parse(balanceStrFirst.Replace("$", "").Replace(",", ""));

            accountOverviewPage.GoToTransferFunds();
            Thread.Sleep(1000);

            transferFundsPage.SelectFromAccount(accountId);
            transferFundsPage.Transfer("5");
            Assert.That(transferFundsPage.IsTransferSuccessful(), Is.True, "Chuyển tiền thất bại.");

            accountOverviewPage.GoToAccountOverview();
            string balanceStrSecond = accountOverviewPage.GetBalanceForAccount(accountId);
            float balanceSecond = float.Parse(balanceStrSecond.Replace("$", "").Replace(",", ""));

            Assert.That(balanceSecond, Is.EqualTo(balanceFirst - 5).Within(0.01f), "Số dư không được cập nhật đúng.");
        }
    }
}