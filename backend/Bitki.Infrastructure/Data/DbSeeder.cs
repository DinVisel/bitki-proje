using Dapper;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Services;

namespace Bitki.Infrastructure.Data
{
    public class DbSeeder
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IAuthService _authService;

        public DbSeeder(IDbConnectionFactory connectionFactory, IAuthService authService)
        {
            _connectionFactory = connectionFactory;
            _authService = authService;
        }

        public async Task InitializeAsync()
        {
            await EnsureUsersTableExists();
            await SeedDevUsers();
        }

        private async Task EnsureUsersTableExists()
        {
            using var connection = _connectionFactory.CreateConnection();
            string sql = @"
                CREATE TABLE IF NOT EXISTS users (
                    Id SERIAL PRIMARY KEY,
                    Username TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL
                );";
            await connection.ExecuteAsync(sql);
        }

        private async Task SeedDevUsers()
        {
            // Try explicit registration which checks existence internally in Service
            // But here we might want to check DB first to avoid unnecessary hashing work

            // Seed Admin
            await _authService.RegisterAsync("admin", "admin123");
            // Updating role to Admin manually since RegisterAsync defaults to User
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("UPDATE users SET Role = 'Admin' WHERE Username = 'admin'");

            // Seed Viewer
            await _authService.RegisterAsync("viewer", "viewer123");
        }
    }
}
