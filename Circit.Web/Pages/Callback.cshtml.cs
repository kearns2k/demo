using Circit.Github.HttpClient.Interfaces;
using Circit.Github.oAuthHttpClient.Models.Request;
using Circit.Web.Models.AppSettings;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Circit.Web.Pages
{
    public class CallbackModel : PageModel
    {

        private readonly IOAuth _oAuth;
        private readonly IGithubApi _githubApi;
        private readonly GitHubOAuthSettings _gitHubOAuthSettings;
        private readonly ILogger<CallbackModel> _logger;


        public string Name { get; set; }
        public string Location { get; set; }
        public string AvatarUri { get; set; }

        public CallbackModel(IOAuth oAuth,
            IGithubApi githubApi,
            IOptions<GitHubOAuthSettings> options,
            ILogger<CallbackModel> logger)
        {
            _oAuth = oAuth;
            _githubApi = githubApi;
            _gitHubOAuthSettings = options.Value;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            ValidateAntiCSRFToken();

            var accessTokenRequest = new AccessTokenRequest
            {
                ClientId = _gitHubOAuthSettings.ClientId,
                ClientSecret = _gitHubOAuthSettings.ClientSecret,
                Code = HttpContext.Request.Query["code"].ToString(),
                RedirectUri = _gitHubOAuthSettings.RedirectUri
            };

            var accessToken = await _oAuth.GetAccessTokenAsync(accessTokenRequest,
                _gitHubOAuthSettings.BasicAuthCredential);
            var user = await _githubApi.GetUserAsync(accessToken.Data.AccessToken);

            Name = user.Data.Name;
            Location = user.Data.Location;
            AvatarUri = user.Data.AvatarUrl;
        }

        private void ValidateAntiCSRFToken()
        {
            const string CsrfMessage = "Anti-CSRF token invalid - potential attack";

            var randomToken = HttpContext.Session.GetString("RandomToken");
            var state = HttpContext.Request.Query["state"].ToString();

            if (randomToken != state)
            {
                _logger.LogCritical(CsrfMessage);
                throw new Exception(CsrfMessage);
            }
        }
    }
}