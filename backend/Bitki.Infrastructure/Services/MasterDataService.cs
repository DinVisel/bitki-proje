using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.MasterData;

namespace Bitki.Infrastructure.Services
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IUlkeRepository _ulkeRepository;

        public MasterDataService(IUlkeRepository ulkeRepository)
        {
            _ulkeRepository = ulkeRepository;
        }
    }
}
