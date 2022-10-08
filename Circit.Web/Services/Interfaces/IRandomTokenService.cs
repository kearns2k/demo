namespace Circit.Web.Services.Interfaces
{
    public interface IRandomTokenService
    {
        Task<string> GenerateRandomTokenAsync();
    }
}
