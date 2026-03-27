using NUnit.Framework;
using ParaBankTests.Pages;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    /// <summary>
    /// Functional Tests: Bill Pay, Open New Account, Find Transactions
    /// </summary>
    [TestFixture]
    [Category("Functional")]
    public class FunctionalTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private BillPayPage billPayPage = null!;
        private OpenAccountPage openAccountPage = null!;
        private FindTransactionsPage findTransactionsPage = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            billPayPage = new BillPayPage(driver!);
            openAccountPage = new OpenAccountPage(driver!);
            findTransactionsPage = new FindTransactionsPage(driver!);

            loginPage.Open();
            loginPage.Login(
                TestDataHelper.Get("validUser", "username"),
                TestDataHelper.Get("validUser", "password"));
        }

        [Test]
        [Description("TC_FN_03: Bill Pay thành công với thông tin hợp lệ")]
        public void TC_FN_03_BillPay_Successfully()
        {
            accountOverviewPage.GoToBillPay();

            billPayPage.FillPayeeInfo(
                TestDataHelper.Get("billPay", "payeeName"),
                TestDataHelper.Get("billPay", "address"),
                TestDataHelper.Get("billPay", "city"),
                TestDataHelper.Get("billPay", "state"),
                TestDataHelper.Get("billPay", "zipCode"),
                TestDataHelper.Get("billPay", "phone"),
                "12345",
                TestDataHelper.Get("billPay", "amount")
            );
            billPayPage.ClickSendPayment();

            Assert.That(billPayPage.IsPaymentSuccessful(), Is.True,
                "Bill Pay không thành công.");
        }

        [Test]
        [Description("TC_FN_04: Mở tài khoản mới thành công")]
        public void TC_FN_04_OpenNewAccount_Successfully()
        {
            accountOverviewPage.GoToOpenAccount();
            openAccountPage.SelectAccountType("CHECKING");
            openAccountPage.ClickOpenAccount();

            Assert.That(openAccountPage.IsAccountOpened(), Is.True,
                "Mở tài khoản mới không thành công.");
        }

        [Test]
        [Description("TC_FN_05: Find Transactions page load được")]
        public void TC_FN_05_FindTransactions_PageLoads()
        {
            accountOverviewPage.GoToFindTransactions();

            Assert.That(findTransactionsPage.IsOnPage(), Is.True,
                "Trang Find Transactions không load được.");
        }

        [Test]
        [Category("GUI")]
        [Description("TC_GUI_05: Các link điều hướng trên trang Account Overview hiển thị đúng")]
        public void TC_GUI_05_AccountOverview_NavigationLinks_Displayed()
        {
            Assert.Multiple(() =>
            {
                Assert.That(accountOverviewPage.IsTransferFundsLinkDisplayed(), Is.True,
                    "Link Transfer Funds không hiển thị.");
                Assert.That(accountOverviewPage.IsBillPayLinkDisplayed(), Is.True,
                    "Link Bill Pay không hiển thị.");
                Assert.That(accountOverviewPage.IsOpenAccountLinkDisplayed(), Is.True,
                    "Link Open New Account không hiển thị.");
            });
        }
    }
}
