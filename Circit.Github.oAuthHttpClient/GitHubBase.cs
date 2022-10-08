using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;

namespace Circit.Github.HttpClient
{
    public class GitHubBase
    {
        public const string AuthorizationHeader = "Authorization";
        public const string JsonContentType = "application/json";
        public const string BasicAuth = "Basic";
        public const string BearerAuth = "Bearer";
        public const string GitOAuthHost = "github.com/login/oauth";
        public const string GitApiHost = "api.github.com";
        public const string UserAgent = "GitClient";
        public const string ErrorString = "error";

        public static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
        }

        public HttpWebRequest GetBasicRequest(string path, string authTokenType, string token, string host)
        {

            var targetUri = GetUri(path, host);

            var request = (HttpWebRequest)WebRequest.Create(targetUri);

            request.Headers.Add(AuthorizationHeader, $"{authTokenType} {token}");
            request.Accept = JsonContentType;
            request.ContentType = JsonContentType;
            request.UserAgent = UserAgent;

            return request;
        }

        public Uri GetUri(string path, string host)
        {
            var uriBuilder = new UriBuilder
            {
                Path = path,
                Scheme = Uri.UriSchemeHttps,
                Host = host
            };

            return uriBuilder.Uri;
        }
    }
}
