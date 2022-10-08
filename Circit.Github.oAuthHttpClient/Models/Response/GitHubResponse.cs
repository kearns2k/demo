
namespace Circit.Github.HttpClient.Models.Response
{
    public class GitHubResponse<TData>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}
