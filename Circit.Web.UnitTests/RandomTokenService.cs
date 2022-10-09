using Circit.Web.Services;

namespace Circit.Web.UnitTests
{
    public class RandomTokenServiceTest
    {

        [Test]
        public void CheckTokenIsCorrectFormat()
        {
            var token = new RandomTokenService().GenerateRandomTokenAsync();
            var hasGuid = IsGuid(token.Substring(0, 36));
            var hasTimestamp = IsTimestamp(token.Substring(36, 19));
            Assert.IsTrue(hasGuid);
            Assert.IsTrue(hasTimestamp);
        }

        private static bool IsGuid(string stringToCheckForGuid)
        {
            return Guid.TryParse(stringToCheckForGuid, out var _);
        }

        private static bool IsTimestamp(string stringToCheckForTimestamp)
        {
            return DateTime.TryParse(stringToCheckForTimestamp, out var _);
        }
    }
}