using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Auth;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Auth
{
    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AuthUserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM users WHERE Username = @Username";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO users (Username, PasswordHash, Role)
                VALUES (@Username, @PasswordHash, @Role)
                RETURNING Id;";
            return await connection.QuerySingleAsync<int>(sql, user);
        }
    }
}
