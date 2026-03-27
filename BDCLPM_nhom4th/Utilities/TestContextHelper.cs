using NUnit.Framework;

namespace ParaBankTests.Utilities
{
    public static class TestContextHelper
    {
        public static string GetTestMessage()
        {
            var msg = TestContext.CurrentContext.Result.Message;
            if (string.IsNullOrEmpty(msg)) return "";
            return msg.Length > 300 ? msg.Substring(0, 300) + "..." : msg;
        }
    }
}