using System.Transactions;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories;
using Bitki.Core.Interfaces.Services;

namespace Bitki.Infrastructure.Services
{
    public class BitkiService : IBitkiService
    {
        private readonly IBitkiRepository _repository;

        public BitkiService(IBitkiRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Plant>> GetAllPlantsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Plant?> GetPlantByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreatePlantAsync(Plant plant)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var id = await _repository.AddAsync(plant);
                scope.Complete();
                return id;
            }
        }

        public async Task UpdatePlantAsync(Plant plant)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _repository.UpdateAsync(plant);
                scope.Complete();
            }
        }

        public async Task DeletePlantAsync(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _repository.DeleteAsync(id);
                scope.Complete();
            }
        }
    }
}
