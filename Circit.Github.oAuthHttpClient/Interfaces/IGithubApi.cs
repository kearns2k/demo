using Circit.Github.oAuthHttpClient.Models.Request;
using Circit.Github.oAuthHttpClient.Models.Response;

namespace Circit.Github.HttpClient.Interfaces
{
    public interface IOAuth
    {
        Task<AccessTokenResponse> GetAccessTokenAsync(AccessTokenRequest accessTokenRequest, string basicAuthCredential);
    }
}
