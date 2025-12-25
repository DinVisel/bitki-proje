using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.System;

namespace Bitki.Infrastructure.Repositories.System
{
    public class DjangoAdminLogRepository : IDjangoAdminLogRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DjangoAdminLogRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
