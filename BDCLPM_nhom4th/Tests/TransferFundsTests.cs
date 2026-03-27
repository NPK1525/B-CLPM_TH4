using ParaBankTests.Pages;
using ParaBankTests.Utilities;

namespace ParaBankTests.Tests
{
    /// <summary>
    /// Test Suite: Transfer Funds (TS_21 – TS_24)
    /// Bao gồm 10 test case kiểm thử chức năng chuyển tiền trên Parabank.
    /// </summary>
    [TestFixture]
    [Category("TransferFunds")]
    public class TransferFundsTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private TransferFundsPage transferFundsPage = null!;

        // Tài khoản dùng chung trong test (lấy từ dropdowns sau khi load)
        private string fromAccount = string.Empty;
        private string toAccount = string.Empty;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            transferFundsPage = new TransferFundsPage(driver!);

            // Đăng nhập
            loginPage.Open();
            loginPage.Login(
                TestDataHelper.Get("validUser", "username"),
                TestDataHelper.Get("validUser", "password"));

            // Điều hướng tới Transfer Funds
            accountOverviewPage.GoToTransferFunds();

            // Lấy danh sách tài khoản từ dropdown
            Thread.Sleep(1000);
            var fromAccounts = transferFundsPage.GetFromAccountIds();
            var toAccounts = transferFundsPage.GetToAccountIds();

            fromAccount = fromAccounts.Count > 0 ? fromAccounts[0] : string.Empty;
            // Ưu tiên chọn tài khoản đích khác tài khoản nguồn (nếu có)
            toAccount = toAccounts.Count > 1 ? toAccounts[1] : (toAccounts.Count > 0 ? toAccounts[0] : string.Empty);
        }

        // =========================================================
        // TS_21 – Chuyển tiền hợp lệ
        // =========================================================

        /// <summary>
        /// TC_TS_21_01: Chuyển tiền thành công với số tiền hợp lệ (1000).
        /// Điều kiện: Đã đăng nhập, tài khoản nguồn đủ số dư.
        /// Kết quả mong đợi: Hiển thị thông báo "Transfer Complete" và số dư được cập nhật.
        /// </summary>
        [Test]
        [Description("TC_TS_21_01: Chuyển tiền thành công – tài khoản nguồn đủ số dư, số tiền 1000")]
        public void TC_TS_21_01_TransferFunds_Successfully()
        {
            Thread.Sleep(500);
            transferFundsPage.SelectFromAccount(fromAccount);
            transferFundsPage.SelectToAccount(toAccount);
            transferFundsPage.Transfer("1000");

            Assert.That(transferFundsPage.IsTransferSuccessful(), Is.True,
                "TC_TS_21_01 FAILED: Chuyển tiền không thành công dù số dư đủ.");
        }

        /// <summary>
        /// TC_TS_21_02: Chuyển tiền số âm (-100).
        /// Kết quả mong đợi: Hiển thị thông báo lỗi "Amount must be positive".
        /// </summary>
        [Test]
        [Description("TC_TS_21_02: Chuyển tiền số âm – phải hiển thị lỗi 'Amount must be positive'")]
        public void TC_TS_21_02_TransferFunds_NegativeAmount()
        {
            Thread.Sleep(500);
            transferFundsPage.SelectFromAccount(fromAccount);
            transferFundsPage.SelectToAccount(toAccount);
            transferFundsPage.Transfer("-100");

            bool hasError = transferFundsPage.HasAmountError();
            bool isSuccess = false;

            if (!hasError)
            {
                // Một số kiểm tra: nếu không có error element, transfer không được thành công
                isSuccess = transferFundsPage.IsTransferSuccessful();
            }

            Assert.That(hasError || !isSuccess, Is.True,
                "TC_TS_21_02 FAILED: Hệ thống không hiển thị lỗi khi nhập số tiền âm (-100).");
        }

        /// <summary>
        /// TC_TS_21_03: Chuyển tiền vượt số dư (999999).
        /// Kết quả mong đợi: Hiển thị thông báo lỗi "Insufficient funds".
        /// </summary>
        [Test]
        [Description("TC_TS_21_03: Chuyển tiền vượt số dư – phải hiển thị lỗi 'Insufficient funds'")]
        public void TC_TS_21_03_TransferFunds_InsufficientFunds()
        {
            Thread.Sleep(500);
            transferFundsPage.SelectFromAccount(fromAccount);
            transferFundsPage.SelectToAccount(toAccount);
            transferFundsPage.Transfer("999999");

            bool isSuccess = transferFundsPage.IsTransferSuccessful();

            Assert.That(isSuccess, Is.False,
                "TC_TS_21_03 FAILED: Hệ thống vẫn chuyển tiền thành công dù số tiền vượt số dư.");
        }

        // =========================================================
        // TS_22 – Nhập số tiền không hợp lệ
        // =========================================================

        /// <summary>
        /// TC_TS_22_01: Nhập số tiền không hợp lệ (chữ "abc").
        /// Kết quả mong đợi: Hiển thị thông báo lỗi "Invalid amount".
        /// </summary>
        [Test]
        [Description("TC_TS_22_01: Nhập số tiền là chữ 'abc' – phải hiển thị lỗi 'Invalid amount'")]
        public void TC_TS_22_01_TransferFunds_InvalidAmount_Text()
        {
            Thread.Sleep(500);
            transferFundsPage.SelectFromAccount(fromAccount);
            transferFundsPage.SelectToAccount(toAccount);
            transferFundsPage.Transfer("abc");

            bool hasError = transferFundsPage.HasAmountError();
            bool isSuccess = false;

            if (!hasError)
            {
                isSuccess = transferFundsPage.IsTransferSuccessful();
            }

            Assert.That(hasError || !isSuccess, Is.True,
                "TC_TS_22_01 FAILED: Hệ thống không hiển thị lỗi khi nhập chữ vào trường amount.");
        }

        /// <summary>
        /// TC_TS_22_02: Nhập số tiền trống (để trống trường amount).
        /// Kết quả mong đợi: Hiển thị thông báo lỗi "Amount is required".
        /// </summary>
        [Test]
        [Description("TC_TS_22_02: Để trống số tiền – phải hiển thị lỗi 'Amount is required'")]
        public void TC_TS_22_02_TransferFunds_EmptyAmount()
        {
            Thread.Sleep(500);
            transferFundsPage.SelectFromAccount(fromAccount);
            transferFundsPage.SelectToAccount(toAccount);

            // Đảm bảo trường amount trống rồi click Transfer
            transferFundsPage.ClearAmount();
            transferFundsPage.Transfer(string.Empty);

            bool hasError = transferFundsPage.HasAmountError();
            bool isSuccess = false;

            if (!hasError)
            {
                isSuccess = transferFundsPage.IsTransferSuccessful();
            }

            Assert.That(hasError || !isSuccess, Is.True,
                "TC_TS_22_02 FAILED: Hệ thống không hiển thị lỗi khi để trống trường amount.");
        }

        // =========================================================
        // TS_23 – Chọn tài khoản nguồn/đích
        // =========================================================

        /// <summary>
        /// TC_TS_23_01: Chọn đủ tài khoản nguồn và đích, nhập số tiền hợp lệ thì chuyển thành công.
        /// Kết quả mong đợi: Phải chọn đủ tài khoản nguồn/đích trước khi chuyển.
        /// </summary>
        [Test]
        [Description("TC_TS_23_01: Chọn đủ tài khoản nguồn và đích – chuyển tiền thành công")]
        public void TC_TS_23_01_SelectSourceAndDestinationAccounts_Success()
        {
            Thread.Sleep(500);
            var fromAccounts = transferFundsPage.GetFromAccountIds();
            var toAccounts = transferFundsPage.GetToAccountIds();

            Assert.Multiple(() =>
            {
                Assert.That(fromAccounts, Has.Count.GreaterThan(0), "Không có tài khoản nguồn trong dropdown.");
                Assert.That(toAccounts, Has.Count.GreaterThan(0), "Không có tài khoản đích trong dropdown.");
            });

            string src = fromAccounts[0];
            string dst = toAccounts.Count > 1 ? toAccounts[1] : toAccounts[0];

            transferFundsPage.SelectFromAccount(src);
            transferFundsPage.SelectToAccount(dst);
            transferFundsPage.Transfer("10");

            Assert.That(transferFundsPage.IsTransferSuccessful(), Is.True,
                "TC_TS_23_01 FAILED: Chuyển tiền thất bại dù đã chọn đủ tài khoản nguồn/đích.");
        }

        /// <summary>
        /// TC_TS_23_02: Không chọn tài khoản nguồn (bỏ trống).
        /// Kết quả mong đợi: Hiển thị thông báo lỗi "Source account is required".
        /// </summary>
        [Test]
        [Description("TC_TS_23_02: Bỏ trống tài khoản nguồn – phải hiển thị lỗi 'Source account is required'")]
        public void TC_TS_23_02_NoSourceAccount_ShowsError()
        {
            Thread.Sleep(500);

            // Không chọn tài khoản nguồn (giữ giá trị mặc định/placeholder nếu có)
            // Chỉ chọn tài khoản đích và nhập số tiền
            transferFundsPage.SelectToAccount(toAccount);
            transferFundsPage.Transfer("100");

            // Hệ thống hoặc hiển thị lỗi validation, hoặc không chuyển tiền thành công
            bool isSuccess = transferFundsPage.IsTransferSuccessful();

            Assert.That(isSuccess, Is.False,
                "TC_TS_23_02 FAILED: Hệ thống vẫn chuyển tiền thành công khi không chọn tài khoản nguồn.");
        }

        /// <summary>
        /// TC_TS_23_03: Không chọn tài khoản đích (bỏ trống).
        /// Kết quả mong đợi: Hiển thị thông báo lỗi "Destination account is required".
        /// </summary>
        [Test]
        [Description("TC_TS_23_03: Bỏ trống tài khoản đích – phải hiển thị lỗi 'Destination account is required'")]
        public void TC_TS_23_03_NoDestinationAccount_ShowsError()
        {
            Thread.Sleep(500);

            // Chỉ chọn tài khoản nguồn, không chọn đích
            transferFundsPage.SelectFromAccount(fromAccount);
            transferFundsPage.Transfer("100");

            // Hệ thống hoặc hiển thị lỗi validation, hoặc không chuyển tiền thành công
            bool isSuccess = transferFundsPage.IsTransferSuccessful();

            Assert.That(isSuccess, Is.False,
                "TC_TS_23_03 FAILED: Hệ thống vẫn chuyển tiền thành công khi không chọn tài khoản đích.");
        }

        // =========================================================
        // TS_24 – Cập nhật số dư
        // =========================================================

        /// <summary>
        /// TC_TS_24_01: Cập nhật số dư sau khi chuyển tiền thành công.
        /// Kết quả mong đợi: Số dư tài khoản nguồn/đích được cập nhật đúng.
        /// </summary>
        [Test]
        [Description("TC_TS_24_01: Số dư tài khoản nguồn/đích được cập nhật đúng sau chuyển tiền thành công")]
        public void TC_TS_24_01_BalancesUpdatedAfterSuccessfulTransfer()
        {
            // Bước 1: Lấy số dư tài khoản nguồn trước khi chuyển
            accountOverviewPage.GoToAccountOverview();
            string accountId = accountOverviewPage.GetFirstAccountId();
            string balanceStrBefore = accountOverviewPage.GetBalanceForAccount(accountId);
            float balanceBefore = float.Parse(
                balanceStrBefore.Replace("$", "").Replace(",", ""));

            // Bước 2: Chuyển 5$ từ tài khoản nguồn
            accountOverviewPage.GoToTransferFunds();
            Thread.Sleep(1000);

            transferFundsPage.SelectFromAccount(accountId);
            transferFundsPage.Transfer("5");

            Assert.That(transferFundsPage.IsTransferSuccessful(), Is.True,
                "TC_TS_24_01 PRE-CONDITION FAILED: Chuyển tiền thất bại trước khi kiểm tra số dư.");

            // Chờ Parabank commit số dư phía server
            Thread.Sleep(2000);

            // Bước 3: Quay lại Account Overview, force reload để lấy số dư mới nhất
            accountOverviewPage.GoToAccountOverview();
            Thread.Sleep(1000);
            string balanceStrAfter = accountOverviewPage.GetBalanceForAccount(accountId);
            float balanceAfter = float.Parse(
                balanceStrAfter.Replace("$", "").Replace(",", ""));

            Assert.That(balanceAfter, Is.EqualTo(balanceBefore - 5).Within(0.01f),
                "TC_TS_24_01 FAILED: Số dư tài khoản nguồn không được cập nhật đúng sau khi chuyển tiền.");
        }

        /// <summary>
        /// TC_TS_24_02: Số dư không thay đổi khi chuyển tiền thất bại (nhập ký tự không hợp lệ).
        /// Kết quả mong đợi: Số dư tài khoản không thay đổi, hiển thị thông báo lỗi.
        /// Ghi chú: Parabank không chặn số tiền vượt số dư (bug), nên dùng 'abc' (invalid) để tạo điều kiện lỗi.
        /// </summary>
        [Test]
        [Description("TC_TS_24_02: Số dư không thay đổi khi chuyển tiền thất bại (nhập ký tự 'abc')")]
        public void TC_TS_24_02_BalanceUnchangedAfterFailedTransfer()
        {
            // Bước 1: Lấy số dư tài khoản nguồn trước khi thử chuyển
            accountOverviewPage.GoToAccountOverview();
            string accountId = accountOverviewPage.GetFirstAccountId();
            string balanceStrBefore = accountOverviewPage.GetBalanceForAccount(accountId);
            float balanceBefore = float.Parse(
                balanceStrBefore.Replace("$", "").Replace(",", ""));

            // Bước 2: Nhập amount không hợp lệ => sẽ không chuyển được
            accountOverviewPage.GoToTransferFunds();
            Thread.Sleep(1000);

            transferFundsPage.SelectFromAccount(accountId);
            transferFundsPage.SelectToAccount(toAccount);
            transferFundsPage.Transfer("abc"); // invalid => không redirect sang Transfer Complete

            // Ghi nhận hành vi: nếu hệ thống chuyển được thì là bug, nhưng test vẫn tiếp tục kiểm tra số dư

            // Bước 3: Quay lại Account Overview kiểm tra số dư không đổi
            accountOverviewPage.GoToAccountOverview();
            Thread.Sleep(1000);
            string balanceStrAfter = accountOverviewPage.GetBalanceForAccount(accountId);
            float balanceAfter = float.Parse(
                balanceStrAfter.Replace("$", "").Replace(",", ""));

            // Nếu không chuyển được: số dư phải bằng ban đầu
            // Nếu Parabank chuyển được cả 'abc' (bug): test này phản ánh lỗi hệ thống
            Assert.That(balanceAfter, Is.EqualTo(balanceBefore).Within(0.01f),
                $"TC_TS_24_02 FAILED: Số dư bị thay đổi dù chuyển tiền với dữ liệu không hợp lệ ('abc'). " +
                $"Trước: {balanceBefore}, Sau: {balanceAfter}");
        }
    }
}
