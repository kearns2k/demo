using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Circit.Github.HttpClient.Interfaces;
using Circit.Github.HttpClient.Models.Response;

namespace Circit.Github.HttpClient
{
    public class GithubApi : GitHubBase, IGithubApi
    {
        private readonly ILogger<OAuth> _logger;
        private string message = "";

        public GithubApi(ILogger<OAuth> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///  Returns user details
        /// </summary>
        /// <returns>
        /// resturns UserResponse object containing properties Name, Location & Avatar_Url
        /// </returns>
        /// <param name="token"></param>
        public async Task<UserResponse> GetUserAsync(string token)
        {
            var request = GetBasicRequest($"user", BearerAuth, token, GitApiHost);
            request.Method = WebRequestMethods.Http.Get;

            try
            {
                var response = await request.GetResponseAsync();

                if (response is HttpWebResponse httpWebResponse)
                {
                    if (httpWebResponse.StatusCode == HttpStatusCode.OK && httpWebResponse.ContentType.Contains("application/json"))
                    {
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string contentText = await reader.ReadToEndAsync();

                            if (contentText.Contains("error"))
                            {
                                return new UserResponse()
                                {
                                    Success = false,
                                    Message = contentText
                                };
                            }

                            UserResponseData userResponseData = JsonConvert.DeserializeObject<UserResponseData>(contentText, GetSerializerSettings());

                            return new UserResponse()
                            {
                                Success = true,
                                Message = "",
                                Data = userResponseData == null ? null : new UserResponseData()
                                {
                                    Name = userResponseData.Name,
                                    Location = userResponseData.Location,
                                    AvatarUrl = userResponseData.AvatarUrl
                                }
                            };

                        }
                    }
                    message = $"Unusual Github response '{httpWebResponse.StatusCode}' for GET {httpWebResponse.ResponseUri}  Content type '{httpWebResponse.ContentType}'";
                    _logger.LogCritical(message);
                    throw new Exception(message);                    
                }
                message = $"Unable to cast Github response of type '{response.GetType().FullName}' to '{typeof(HttpWebResponse).FullName}' when calling '{request.Method} {request.RequestUri}'";
                _logger.LogCritical(message);
                throw new Exception(message);                
            }
            catch (WebException webException)
            {
                if (webException.Response is HttpWebResponse httpWebResponse)
                {
                    switch (httpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.Forbidden:
                        case HttpStatusCode.Unauthorized:
                            message = $"Invalid credential '{httpWebResponse.StatusCode}'";
                            _logger.LogCritical(message);
                            throw new Exception(message);
                        default:
                            message = $"Unusual error status code '{httpWebResponse.StatusCode}'";
                            _logger.LogCritical(message);
                            throw new Exception(message);
                    }
                }
                message = $"Error during Github GetUserAsync request";
                _logger.LogCritical(message);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                message = $"Error during Github GetUserAsync request: ";
                _logger.LogCritical(message);
                throw new Exception(message, ex);
            }

        }

    }
}