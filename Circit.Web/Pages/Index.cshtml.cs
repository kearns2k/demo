using Circit.Web.Models.AppSettings;
using Circit.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Circit.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly GitHubOAuthSettings _gitHubOAuthSettings;
        private readonly IRandomTokenService _randomTokenService;

        private const string RandomTokenSessionProperty = "RandomToken";

        public string GitHubAuthUrl { get; set; }

        public IndexModel(IOptions<GitHubOAuthSettings> options,
            IRandomTokenService randomTokenService)
        {
            _gitHubOAuthSettings = options.Value;
            _randomTokenService = randomTokenService;
        }

        public void OnGet()
        {
            //Generate token to pass as state & use as an anti-CSRF measure
            var randomToken =  _randomTokenService.GenerateRandomTokenAsync();
                        
            HttpContext.Session.SetString(RandomTokenSessionProperty, randomToken);
           
            GitHubAuthUrl = $"{_gitHubOAuthSettings.AuthUri}?client_id={_gitHubOAuthSettings.ClientId}&response_type=code&redirect_uri={_gitHubOAuthSettings.RedirectUri}&state={randomToken}"; 
        }
    }
}