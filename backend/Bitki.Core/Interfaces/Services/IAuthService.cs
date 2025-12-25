namespace Bitki.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(string Token, string Role, string Username)?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
    }
}
