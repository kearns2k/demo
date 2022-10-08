using Circit.Github.HttpClient.Models.Response;
using Circit.Github.oAuthHttpClient.Models.Request;
using Circit.Github.oAuthHttpClient.Models.Response;

namespace Circit.Github.HttpClient.Interfaces
{
    public interface IGithubApi
    {
        Task<UserResponse> GetUserAsync(string token);
    }
}
