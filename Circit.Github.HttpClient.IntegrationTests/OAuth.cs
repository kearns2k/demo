using Circit.Github.HttpClient.Interfaces;
using Circit.Github.oAuthHttpClient.Models.Request;
using Circit.Github.oAuthHttpClient.Models.Response;
using Microsoft.Extensions.Logging;

namespace Circit.Github.HttpClient.IntegrationTests
{

    [TestFixture]
    public class OAuthTests
    {
        private const string ClientId = "b6c2723457ce902257a3";
        private const string ClientSecret = "12a359a3500b0247b984455f8974dff60503df8a";
        private const string Credential = "YjZjMjcyMzQ1N2NlOTAyMjU3YTM6YjFmMzI2NzE0ZDNkZWE0MGVkYzUwODdiN2FmMGE2YTg5OTBkNDhhMw==";
        private const string RedirectUri = "https://localhost:7198/callback";
        [Test]
        public async Task GetAccessToken_WithExpiredCode_Test()
        {
            var mockSettingsService = new Mock<ILogger<OAuth>>();

            IOAuth oAuth = new OAuth(mockSettingsService.Object);

            AccessTokenRequest accessTokenRequest = new()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Code = "",
                RedirectUri = RedirectUri

            };

            AccessTokenResponse result = await oAuth.GetAccessTokenAsync(accessTokenRequest, Credential);

            result.Message.Should().Contain("The code passed is incorrect or expired");
            result.Success.Should().BeFalse();

            Console.WriteLine(result);
        }

        [Test]
        public async Task GetAccessToken_WithWrongClientSecret_Test()
        {
            var mockSettingsService = new Mock<ILogger<OAuth>>();

            IOAuth oAuth = new OAuth(mockSettingsService.Object);

            AccessTokenRequest accessTokenRequest = new()
            {
                ClientId = ClientId,
                ClientSecret = "1234",
                Code = "",
                RedirectUri = RedirectUri

            };

            AccessTokenResponse result = await oAuth.GetAccessTokenAsync(accessTokenRequest, Credential);

            result.Message.Should().Contain("The client_id and/or client_secret passed are incorrect.");
            result.Success.Should().BeFalse();

            Console.WriteLine(result);
        }
    }
}