
using Circit.Github.HttpClient.Models.Response;

namespace Circit.Github.oAuthHttpClient.Models.Response
{
    public class AccessTokenResponse : GitHubResponse<AccessTokenResponseData>
    { }

    public class AccessTokenResponseData
    {
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
    }

}
