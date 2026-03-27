using ParaBankTests.Pages;
using ParaBankTests.Tests;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("OpenAccount")]
    public class OpenAccountTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private OpenAccountPage openAccountPage = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            openAccountPage = new OpenAccountPage(driver!);

            loginPage.Open();
            loginPage.Login(
                TestDataHelper.Get("validUser", "username"),
                TestDataHelper.Get("validUser", "password"));
            accountOverviewPage.GoToOpenAccount();
        }

        [Test]
        [Description("TS_17: Open New Account - Kiểm tra mở tài khoản mới thành công")]
        public void TC_TS_17_01_OpenNewAccount_Successfully()
        {
            openAccountPage.SelectAccountType("SAVINGS");
            Thread.Sleep(500);
            openAccountPage.ClickOpenAccount();
            Assert.That(openAccountPage.IsAccountOpened(), Is.True, "Không hiển thị thông báo mở tài khoản thành công.");
            Assert.That(string.IsNullOrEmpty(openAccountPage.GetNewAccountId()), Is.False, "New Account ID không được sinh ra.");
        }

        [Test]
        [Description("TS_18: Open New Account - Kiểm tra hiển thị đúng loại tài khoản trong Dropdown")]
        public void TC_TS_18_01_VerifyAccountTypes()
        {
            var accountTypes = openAccountPage.GetAvailableAccountTypes();
            Assert.Multiple(() =>
            {
                Assert.That(accountTypes.Count, Is.GreaterThan(0), "Không có loại tài khoản nào được hiển thị.");
                Assert.That(accountTypes, Does.Contain("CHECKING"), "Thiếu loại tài khoản CHECKING.");
                Assert.That(accountTypes, Does.Contain("SAVINGS"), "Thiếu loại tài khoản SAVINGS.");
            });
        }

        [Test]
        [Description("TS_19: Open New Account - Kiểm tra chọn tài khoản nguồn hợp lệ")]
        public void TC_TS_19_01_VerifySourceAccounts()
        {
            var fromAccountIds = openAccountPage.GetAvailableFromAccountIds();
            Assert.That(fromAccountIds.Count, Is.GreaterThan(0), "Dropdown tài khoản nguồn trống.");
            string firstAccountId = fromAccountIds[0];
            Assert.DoesNotThrow(() => openAccountPage.SelectFromAccountId(firstAccountId), "Không thể chọn tài khoản nguồn hợp lệ.");
        }

        [Test]
        [Description("TS_20: Open New Account - Kiểm tra hiển thị sau khi tạo, tài khoản mới xuất hiện")]
        public void TC_TS_20_01_NewAccountDisplaysInOverview()
        {
            openAccountPage.SelectAccountType("CHECKING");
            openAccountPage.ClickOpenAccount();
            Assert.That(openAccountPage.IsAccountOpened(), Is.True, "Tạo tài khoản thất bại.");
            string newAccountId = openAccountPage.GetNewAccountId();
            accountOverviewPage.GoToAccountOverview();
            Thread.Sleep(500);
            bool isFound = accountOverviewPage.IsAccountInList(newAccountId);
            Assert.That(isFound, Is.True, $"Tài khoản mới sinh ra {newAccountId} không xuất hiện trong Accounts Overview.");
        }
    }
}