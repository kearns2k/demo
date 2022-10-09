using Circit.Web.Services.Interfaces;

namespace Circit.Web.Services
{
/// <summary>
///  Creates a random token that can be used to check for CSRF
/// </summary>
/// <returns>
/// A string consisting of a new guid and timestap
/// </returns>
    public class RandomTokenService : IRandomTokenService
    {
        public string GenerateRandomTokenAsync()
        {
            return Guid.NewGuid() + DateTime.Now.ToString();
        }
    }
}
