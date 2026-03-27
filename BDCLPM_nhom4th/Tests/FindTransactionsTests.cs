using NUnit.Framework;
using ParaBankTests.Pages;

namespace ParaBankTests.Tests
{
    [TestFixture]
    [Category("Functional")]
    public class FindTransactionsTests : BaseTest
    {
        private const string Username = "Test1234";
        private const string Password = "111";

        private LoginPage loginPage = null!;
        private AccountOverviewPage accountOverviewPage = null!;
        private FindTransactionsPage findPage = null!;
        private string AccountNo = "";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            loginPage = new LoginPage(driver!);
            accountOverviewPage = new AccountOverviewPage(driver!);
            findPage = new FindTransactionsPage(driver!);

            loginPage.Open();
            loginPage.Login(Username, Password);

            accountOverviewPage.Open();
            AccountNo = accountOverviewPage.GetFirstAccountId();

            findPage.Open();
            findPage.SelectAccount(AccountNo);
        }

        [Test]
        [Description("TC_TS_30_01: Tìm giao dịch theo Transaction ID hợp lệ – có kết quả")]
        public void TC_TS_30_01_FindTransactions_ByValidId_ShowsResults()
        {
            findPage.FindById("1001");
            findPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(findPage.IsResultVisible(), Is.True,
                    "#resultContainer phải hiển thị.");
                Assert.That(findPage.IsResultsTableDisplayed(), Is.True,
                    "#transactionTable phải hiển thị.");
                Assert.That(findPage.HasResultRows(), Is.True,
                    "Bảng kết quả phải có ít nhất 1 dòng.");
            });
        }

        [Test]
        [Description("TC_TS_30_03: Tìm theo ngày hợp lệ MM-DD-YYYY – có kết quả")]
        public void TC_TS_30_03_FindTransactions_ByValidDate_ShowsResults()
        {
            findPage.FindByDate("01-15-2024");
            findPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(findPage.IsResultVisible(), Is.True,
                    "#resultContainer phải hiển thị.");
                Assert.That(findPage.IsResultsTableDisplayed(), Is.True,
                    "#transactionTable phải hiển thị.");
            });
        }

        [Test]
        [Description("TC_TS_30_04: Tìm theo ngày không có giao dịch – bảng rỗng, không lỗi")]
        public void TC_TS_30_04_FindTransactions_ByDateWithNoResults_ShowsEmptyTable()
        {
            findPage.FindByDate("01-01-2000");

            System.Threading.Thread.Sleep(3000);

            Assert.Multiple(() =>
            {
                Assert.That(findPage.IsErrorContainerVisible(), Is.False,
                    "#errorContainer không được hiển thị.");
                // Nếu result hiện thì bảng phải rỗng
                if (findPage.IsResultVisible())
                {
                    Assert.That(findPage.GetResultRowCount(), Is.EqualTo(0),
                        "Bảng phải rỗng khi không có giao dịch.");
                }
            });
        }

        [Test]
        [Description("TC_TS_31_01: Tìm theo Date Range hợp lệ – có kết quả")]
        public void TC_TS_31_01_FindTransactions_ByValidDateRange_ShowsResults()
        {
            findPage.FindByDateRange("01-01-2024", "06-30-2024");
            findPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(findPage.IsResultVisible(), Is.True,
                    "#resultContainer phải hiển thị.");
                Assert.That(findPage.IsResultsTableDisplayed(), Is.True,
                    "#transactionTable phải hiển thị.");
            });
        }

        [Test]
        [Description("TC_TS_32_01: Tìm theo Amount hợp lệ – có kết quả")]
        public void TC_TS_32_01_FindTransactions_ByValidAmount_ShowsResults()
        {
            findPage.FindByAmount("100");
            findPage.WaitForResult();

            Assert.Multiple(() =>
            {
                Assert.That(findPage.IsResultVisible(), Is.True,
                    "#resultContainer phải hiển thị.");
                Assert.That(findPage.IsResultsTableDisplayed(), Is.True,
                    "#transactionTable phải hiển thị.");
            });
        }

        [Test]
        [Description("TC_TS_33_01: Transaction ID nhập chữ – hiện lỗi 'Invalid transaction ID'")]
        public void TC_TS_33_01_FindTransactions_InvalidTransactionId_ShowsError()
        {
            findPage.FindById("abc");
            Assert.Multiple(() =>
            {
                Assert.That(findPage.IsTransactionIdErrorDisplayed(), Is.True,
                    "span#transactionIdError phải hiển thị.");
                Assert.That(findPage.GetTransactionIdError(), Is.EqualTo("Invalid transaction ID"),
                    "Nội dung lỗi phải là 'Invalid transaction ID'.");
                Assert.That(findPage.IsResultVisible(), Is.False,
                    "#resultContainer phải vẫn ẩn.");
            });
        }
    }
}