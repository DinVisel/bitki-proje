
using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Ozellik
{
    public interface IPropertyTypeRepository
    {
        Task<IEnumerable<PropertyType>> GetAllAsync();
    }
}
