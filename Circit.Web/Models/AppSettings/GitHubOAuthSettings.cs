namespace Circit.Web.Models.AppSettings
{
    public class GitHubOAuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string AuthUri { get; set; }
        public string BasicAuthCredential { get; set; }

    }
}
