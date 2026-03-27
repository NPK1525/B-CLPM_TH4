using ParaBankTests.Pages;
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
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>TC_TS_17_01: Mở tài khoản mới hợp lệ.</summary>
        [Test]
        [Description("TC_TS_17_01: Mở tài khoản mới hợp lệ – chọn Savings, tài khoản nguồn hợp lệ")]
        public void TC_TS_17_01_OpenNewAccount_ValidInput_Successfully()
        {
            Assert.That(openAccountPage.IsOnPage(), Is.True,
                "TC_TS_17_01 PRE-CONDITION FAILED: Không ở trang Open New Account.");

            openAccountPage.SelectAccountType("SAVINGS");

            var fromAccounts = openAccountPage.GetAvailableFromAccountIds();
            Assert.That(fromAccounts, Has.Count.GreaterThan(0),
                "TC_TS_17_01 PRE-CONDITION FAILED: Không có tài khoản nguồn nào trong dropdown.");
            openAccountPage.SelectFromAccountId(fromAccounts[0]);
            openAccountPage.ClickOpenAccount();

            Assert.Multiple(() =>
            {
                Assert.That(openAccountPage.IsAccountOpened(), Is.True,
                    "TC_TS_17_01 FAILED: Không hiển thị thông báo mở tài khoản thành công.");
                Assert.That(string.IsNullOrEmpty(openAccountPage.GetNewAccountId()), Is.False,
                    "TC_TS_17_01 FAILED: New Account ID không được sinh ra.");
            });
        }

        /// <summary>TC_TS_17_02: Không chọn loại tài khoản – phát hiện bug thiếu validation.</summary>
        [Test]
        [Description("TC_TS_17_02: Không chọn loại tài khoản – phải hiển thị lỗi, KHÔNG mở tài khoản (BUG nếu pass)")]
        public void TC_TS_17_02_OpenNewAccount_EmptyAccountType_ShowsError()
        {
            Assert.That(openAccountPage.IsOnPage(), Is.True,
                "TC_TS_17_02 PRE-CONDITION FAILED: Không ở trang Open New Account.");

            openAccountPage.ClickOpenAccount();

            Assert.That(openAccountPage.IsAccountOpened(), Is.False,
                "TC_TS_17_02 FAILED: Hệ thống mở tài khoản dù không chọn loại – thiếu validation.");
        }

        /// <summary>TC_TS_17_03: Tài khoản nguồn không hợp lệ không có trong dropdown.</summary>
        [Test]
        [Description("TC_TS_17_03: Tài khoản nguồn không hợp lệ (#00000) – phải báo lỗi, KHÔNG mở tài khoản")]
        public void TC_TS_17_03_OpenNewAccount_InvalidSourceAccount_ShowsError()
        {
            Assert.That(openAccountPage.IsOnPage(), Is.True,
                "TC_TS_17_03 PRE-CONDITION FAILED: Không ở trang Open New Account.");

            openAccountPage.SelectAccountType("CHECKING");

            var fromAccounts = openAccountPage.GetAvailableFromAccountIds();
            bool invalidExists = fromAccounts.Contains("00000") || fromAccounts.Contains("Account #00000");
            Assert.That(invalidExists, Is.False,
                "TC_TS_17_03 INFO: Tài khoản #00000 không được phép xuất hiện trong dropdown.");

            openAccountPage.ClickOpenAccount();

            TestContext.WriteLine(
                $"TC_TS_17_03 INFO: Tài khoản nguồn có sẵn: [{string.Join(", ", fromAccounts)}].");
        }

        /// <summary>TC_TS_18_01: Dropdown loại tài khoản hiển thị đầy đủ SAVINGS và CHECKING.</summary>
        [Test]
        [Description("TC_TS_18_01: Kiểm tra dropdown loại tài khoản hiển thị đầy đủ SAVINGS và CHECKING")]
        public void TC_TS_18_01_AccountTypeDropdown_DisplaysSavingsAndChecking()
        {
            Assert.That(openAccountPage.IsOnPage(), Is.True,
                "TC_TS_18_01 PRE-CONDITION FAILED: Không ở trang Open New Account.");

            var accountTypes = openAccountPage.GetAvailableAccountTypes();

            Assert.Multiple(() =>
            {
                Assert.That(accountTypes, Has.Count.GreaterThan(0),
                    "TC_TS_18_01 FAILED: Dropdown loại tài khoản không có lựa chọn nào.");
                Assert.That(accountTypes, Does.Contain("CHECKING"),
                    "TC_TS_18_01 FAILED: Thiếu loại tài khoản CHECKING.");
                Assert.That(accountTypes, Does.Contain("SAVINGS"),
                    "TC_TS_18_01 FAILED: Thiếu loại tài khoản SAVINGS.");
            });

            Assert.DoesNotThrow(() => openAccountPage.SelectAccountType("SAVINGS"),
                "TC_TS_18_01 FAILED: Không thể chọn SAVINGS.");
            Assert.DoesNotThrow(() => openAccountPage.SelectAccountType("CHECKING"),
                "TC_TS_18_01 FAILED: Không thể chọn CHECKING.");
        }

        /// <summary>TC_TS_19_01: Chọn tài khoản nguồn hợp lệ.</summary>
        [Test]
        [Description("TC_TS_19_01: Chọn tài khoản nguồn hợp lệ từ dropdown – phải thành công")]
        public void TC_TS_19_01_SelectValidSourceAccount_Successfully()
        {
            Assert.That(openAccountPage.IsOnPage(), Is.True,
                "TC_TS_19_01 PRE-CONDITION FAILED: Không ở trang Open New Account.");

            var fromAccounts = openAccountPage.GetAvailableFromAccountIds();
            Assert.That(fromAccounts, Has.Count.GreaterThan(0),
                "TC_TS_19_01 FAILED: Dropdown tài khoản nguồn trống.");

            string validAccount = fromAccounts[0];
            Assert.DoesNotThrow(() => openAccountPage.SelectFromAccountId(validAccount),
                $"TC_TS_19_01 FAILED: Không thể chọn tài khoản nguồn '{validAccount}'.");
        }

        /// <summary>TC_TS_19_02: Tài khoản nguồn không hợp lệ không có trong dropdown.</summary>
        [Test]
        [Description("TC_TS_19_02: Tài khoản nguồn không hợp lệ (#00000) không được xuất hiện trong dropdown")]
        public void TC_TS_19_02_InvalidSourceAccount_NotInDropdown()
        {
            Assert.That(openAccountPage.IsOnPage(), Is.True,
                "TC_TS_19_02 PRE-CONDITION FAILED: Không ở trang Open New Account.");

            var fromAccounts = openAccountPage.GetAvailableFromAccountIds();

            bool invalidAccountExists = fromAccounts.Any(acc =>
                acc.Contains("00000") || acc == "Account #00000");

            Assert.That(invalidAccountExists, Is.False,
                "TC_TS_19_02 FAILED: Tài khoản #00000 xuất hiện trong dropdown – rủi ro bảo mật.");

            Assert.Throws<OpenQA.Selenium.NoSuchElementException>(
                () => openAccountPage.SelectFromAccountId("Account #00000"),
                "TC_TS_19_02 FAILED: Hệ thống cho phép chọn tài khoản không hợp lệ.");
        }

        /// <summary>TC_TS_20_01: Tài khoản mới xuất hiện trong Account Overview sau khi tạo.</summary>
        [Test]
        [Description("TC_TS_20_01: Tài khoản mới xuất hiện trong Account Overview sau khi tạo thành công")]
        public void TC_TS_20_01_NewAccountAppearsInOverview_WithCorrectInfo()
        {
            Assert.That(openAccountPage.IsOnPage(), Is.True,
                "TC_TS_20_01 PRE-CONDITION FAILED: Không ở trang Open New Account.");

            openAccountPage.SelectAccountType("CHECKING");
            openAccountPage.ClickOpenAccount();

            Assert.That(openAccountPage.IsAccountOpened(), Is.True,
                "TC_TS_20_01 PRE-CONDITION FAILED: Tạo tài khoản thất bại.");

            string newAccountId = openAccountPage.GetNewAccountId();
            Assert.That(string.IsNullOrEmpty(newAccountId), Is.False,
                "TC_TS_20_01 FAILED: Không lấy được ID tài khoản mới.");

            accountOverviewPage.GoToAccountOverview();
            System.Threading.Thread.Sleep(1000);

            Assert.That(accountOverviewPage.IsAccountInList(newAccountId), Is.True,
                $"TC_TS_20_01 FAILED: Tài khoản '{newAccountId}' không xuất hiện trong Account Overview.");

            string balance = accountOverviewPage.GetBalanceForAccount(newAccountId);
            Assert.Multiple(() =>
            {
                Assert.That(string.IsNullOrEmpty(balance), Is.False,
                    $"TC_TS_20_01 FAILED: Không đọc được số dư của tài khoản '{newAccountId}'.");
                Assert.That(balance, Does.Contain("$"),
                    $"TC_TS_20_01 FAILED: Số dư '{balance}' không có định dạng tiền tệ hợp lệ.");
            });
        }
    }
}
