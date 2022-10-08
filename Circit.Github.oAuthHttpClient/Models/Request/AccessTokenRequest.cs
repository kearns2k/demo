
namespace Circit.Github.oAuthHttpClient.Models.Request
{
    public class AccessTokenRequest
    {
        public AccessTokenRequest()
        {

        }
      
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Code { get; set; }

        public string RedirectUri { get; set; }

    }
}
