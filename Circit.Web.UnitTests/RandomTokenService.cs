using Circit.Web.Services;

namespace Circit.Web.UnitTests
{
    public class RandomTokenServiceTest
    {       

        [Test]
        public async Task CheckTokenIsCorrectFormat()
        {
            var token = await new RandomTokenService().GenerateRandomTokenAsync();
            var test = IsGuid(token.Substring(0, 36));
            Assert.IsTrue(test);
        }

        private static bool IsGuid(string stringToCheckForGuid)
        {
          
            return Guid.TryParse(stringToCheckForGuid, out var _);
        }
    }
}