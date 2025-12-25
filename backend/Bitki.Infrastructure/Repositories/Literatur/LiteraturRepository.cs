using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Literatur;

namespace Bitki.Infrastructure.Repositories.Literatur
{
    public class LiteraturRepository : ILiteraturRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LiteraturRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
