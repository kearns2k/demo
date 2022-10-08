
namespace Circit.Github.HttpClient.Models.Response
{
    public class UserResponse : GitHubResponse<UserResponseData>
    { }
    public class UserResponseData
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string AvatarUrl { get; set; }
    }

}
