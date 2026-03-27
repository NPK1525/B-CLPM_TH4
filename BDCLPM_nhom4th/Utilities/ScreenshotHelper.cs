using OpenQA.Selenium;
using NUnit.Framework;

namespace ParaBankTests.Utilities
{
    public static class ScreenshotHelper
    {
        /// <summary>
        /// Chụp screenshot và trả về đường dẫn file.
        /// Trả về chuỗi rỗng nếu không chụp được.
        /// </summary>
        public static string TakeScreenshot(IWebDriver? driver, string testName)
        {
            if (driver == null) return string.Empty;

            try
            {
                // Lưu vào thư mục Screenshots ngay cạnh project, dễ tìm hơn
                string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\"));
                string folder = Path.Combine(projectRoot, "Screenshots");
                Directory.CreateDirectory(folder);

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"{testName}_{timestamp}.png";
                string filePath = Path.Combine(folder, fileName);

                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(filePath);

                // Đính kèm vào NUnit test report
                TestContext.AddTestAttachment(filePath, $"Screenshot: {testName}");
                TestContext.WriteLine($"Screenshot saved: {filePath}");

                return filePath;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Không thể chụp screenshot: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
