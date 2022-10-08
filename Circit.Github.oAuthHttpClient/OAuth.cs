using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Circit.Github.oAuthHttpClient.Models.Response;
using Circit.Github.oAuthHttpClient.Models.Request;
using Circit.Github.HttpClient.Interfaces;

namespace Circit.Github.HttpClient
{

    public class OAuth : GitHubBase, IOAuth
    {
        private readonly ILogger<OAuth> _logger;
        private string message = "";
        

        public OAuth(ILogger<OAuth> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///  Returns access token to be used in furture github api call
        /// </summary>
        /// <returns>
        /// resturns AccessTokenResponse object containing properties AccessToken, Scope & TokenType
        /// </returns>
        /// <param name="accessTokenRequest"></param>
        /// <param name="basicAuthCredential"></param>
        public async Task<AccessTokenResponse> GetAccessTokenAsync(AccessTokenRequest accessTokenRequest, string basicAuthCredential)
        {
            var request = GetBasicRequest($"access_token", BasicAuth, basicAuthCredential, GitOAuthHost);
            request.Method = WebRequestMethods.Http.Post;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(accessTokenRequest, GetSerializerSettings());
                await streamWriter.WriteAsync(json);
            }

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
                                return new AccessTokenResponse()
                                {
                                    Success = false,
                                    Message = contentText
                                };
                            }
                            AccessTokenResponseData accessTokenResponseData = JsonConvert.DeserializeObject<AccessTokenResponseData>(contentText, GetSerializerSettings());

                            return new AccessTokenResponse()
                            {
                                Success = true,
                                Message = "",
                                Data = accessTokenResponseData == null ? null : new AccessTokenResponseData()
                                {
                                    AccessToken = accessTokenResponseData.AccessToken,
                                    TokenType = accessTokenResponseData.TokenType,
                                    Scope = accessTokenResponseData.Scope
                                }
                            };
                            
                        }
                    }

                   message = $"Unusual Github response '{httpWebResponse.StatusCode}' for GET {httpWebResponse.ResponseUri}  Content type '{httpWebResponse.ContentType}'";
                    throw new Exception(message);
                    _logger.LogCritical(message);}

                message = $"Unable to cast Github response of type '{response.GetType().FullName}' to '{typeof(HttpWebResponse).FullName}' when calling '{request.Method} {request.RequestUri}'";
                throw new Exception(message);
                _logger.LogCritical(message);
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

                message = $"Error during Github GetAccessTokenAsync request: ";
                _logger.LogCritical(message);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                message = $"Error during Github GetAccessTokenAsync request: ";
                _logger.LogCritical(message);
                throw new Exception(message, ex);
            }

        }

    }
}