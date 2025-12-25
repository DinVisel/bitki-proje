using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Auth
{
    public interface IAuthUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<int> CreateAsync(User user);
    }
}
