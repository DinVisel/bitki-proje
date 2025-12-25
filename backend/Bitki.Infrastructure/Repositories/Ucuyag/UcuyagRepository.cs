using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Ucuyag;

namespace Bitki.Infrastructure.Repositories.Ucuyag
{
    public class UcuyagRepository : IUcuyagRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UcuyagRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
