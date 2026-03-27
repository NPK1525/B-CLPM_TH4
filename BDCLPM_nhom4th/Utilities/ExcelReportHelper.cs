using ClosedXML.Excel;
using System;
using System.IO;

namespace ParaBankTests.Utilities
{
    public static class ExcelReportHelper
    {
        private static readonly string ExcelFilePath =
            Path.Combine(@"E:\Excel_test", "TestScenario_nhom4.xlsx");

        private static readonly object _lock = new();

        private static string ExtractTestCaseId(string testMethodName)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                testMethodName, @"TC_TS_\d+_\d+");
            return match.Success ? match.Value : testMethodName;
        }

        public static void WriteResult(
            string testMethodName,
            string status,
            string screenshotPath)
        {
            lock (_lock)
            {
                if (!File.Exists(ExcelFilePath))
                {
                    Console.WriteLine($"[ExcelReport] File không tồn tại: {ExcelFilePath}");
                    return;
                }

                var testCaseId = ExtractTestCaseId(testMethodName);

                using var workbook = new XLWorkbook(ExcelFilePath);

                foreach (var ws in workbook.Worksheets)
                {
                    if (TryWriteToSheet(ws, testCaseId, testMethodName, status, screenshotPath))
                    {
                        workbook.Save();
                        Console.WriteLine($"[ExcelReport] Đã ghi '{testCaseId}' vào sheet '{ws.Name}'");
                        return;
                    }
                }

                Console.WriteLine($"[ExcelReport] Không tìm thấy '{testCaseId}' trong bất kỳ sheet nào.");
                workbook.Save();
            }
        }

        private static bool TryWriteToSheet(
            IXLWorksheet ws,
            string testCaseId,
            string testMethodName,
            string status,
            string screenshotPath)
        {
            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
            int lastCol = ws.LastColumnUsed()?.ColumnNumber() ?? 0;

            if (lastRow == 0 || lastCol == 0) return false;

            int headerRow = FindHeaderRow(ws, lastRow, lastCol);
            if (headerRow == 0)
            {
                Console.WriteLine($"[ExcelReport] Sheet '{ws.Name}': không tìm thấy header row.");
                return false;
            }

            int colTestCaseId = FindColumnExact(ws, headerRow, lastCol, "Test Case ID");
            int colTestscripts = FindColumnExact(ws, headerRow, lastCol, "Testscripts");
            int colResult = FindColumnExact(ws, headerRow, lastCol, "Result");
            int colScreenshot = FindColumnExact(ws, headerRow, lastCol, "Screenshot");
            int colActual = FindColumnExact(ws, headerRow, lastCol, "Actual Result");

            if (colTestCaseId == 0 || colTestscripts == 0 || colResult == 0)
            {
                Console.WriteLine($"[ExcelReport] Sheet '{ws.Name}': thiếu cột bắt buộc. " +
                    $"TestCaseId={colTestCaseId}, Testscripts={colTestscripts}, Result={colResult}");
                return false;
            }

            int targetRow = FindTestCaseRow(ws, testCaseId, colTestCaseId, headerRow + 1, lastRow);
            if (targetRow == 0) return false;

            // Ghi tên test method vào cột Testscripts
            ws.Cell(targetRow, colTestscripts).Value = testMethodName;

            // Ghi Passed/Failed vào cột Result
            bool isPassed = status.Equals("Pass", StringComparison.OrdinalIgnoreCase);
            var resultCell = ws.Cell(targetRow, colResult);
            resultCell.Value = isPassed ? "Passed" : "Failed";
            resultCell.Style.Font.Bold = true;
            resultCell.Style.Font.FontColor = isPassed ? XLColor.Green : XLColor.Red;

            // ✅ Ghi Actual Result cho cả Pass lẫn Fail
            if (colActual > 0)
            {
                var actualCell = ws.Cell(targetRow, colActual);
                if (isPassed)
                {
                    actualCell.Value = "Test passed successfully.";
                    actualCell.Style.Font.FontColor = XLColor.Green;
                }
                else
                {
                    var message = TestContextHelper.GetTestMessage();
                    actualCell.Value = !string.IsNullOrEmpty(message)
                        ? message
                        : "Test failed (no error message).";
                    actualCell.Style.Font.FontColor = XLColor.Red;
                }
            }

            if (colScreenshot > 0)
                ws.Cell(targetRow, colScreenshot).Value = screenshotPath ?? "";

            return true;
        }

        private static int FindHeaderRow(IXLWorksheet ws, int lastRow, int lastCol)
        {
            int searchUntil = Math.Min(lastRow, 10);
            for (int r = 1; r <= searchUntil; r++)
            {
                bool hasActual = false, hasTestscripts = false;
                for (int c = 1; c <= lastCol; c++)
                {
                    var val = ws.Cell(r, c).GetString();
                    if (val.Contains("Actual Result", StringComparison.OrdinalIgnoreCase)) hasActual = true;
                    if (val.Contains("Testscripts", StringComparison.OrdinalIgnoreCase)) hasTestscripts = true;
                }
                if (hasActual && hasTestscripts) return r;
            }
            return 0;
        }

        private static int FindColumnExact(IXLWorksheet ws, int row, int lastCol, string keyword)
        {
            // Ưu tiên 1: exact match
            for (int c = 1; c <= lastCol; c++)
            {
                var val = ws.Cell(row, c).GetString().Trim();
                if (val.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                    return c;
            }

            // Ưu tiên 2: starts with, loại trừ Expected/Actual
            for (int c = 1; c <= lastCol; c++)
            {
                var val = ws.Cell(row, c).GetString().Trim();
                if (val.StartsWith(keyword, StringComparison.OrdinalIgnoreCase)
                    && !val.Contains("Expected", StringComparison.OrdinalIgnoreCase)
                    && !val.Contains("Actual", StringComparison.OrdinalIgnoreCase))
                    return c;
            }

            // Fallback: contains, loại trừ Expected/Actual
            for (int c = 1; c <= lastCol; c++)
            {
                var val = ws.Cell(row, c).GetString().Trim();
                if (val.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    && !val.Contains("Expected", StringComparison.OrdinalIgnoreCase)
                    && !val.Contains("Actual", StringComparison.OrdinalIgnoreCase))
                    return c;
            }

            return 0;
        }

        private static int FindTestCaseRow(
            IXLWorksheet ws,
            string testCaseId,
            int colTestCaseId,
            int startRow,
            int lastRow)
        {
            for (int r = startRow; r <= lastRow; r++)
            {
                var cellVal = ws.Cell(r, colTestCaseId).GetString().Trim();
                if (cellVal.Equals(testCaseId, StringComparison.OrdinalIgnoreCase))
                    return r;
            }
            return 0;
        }
    }
}