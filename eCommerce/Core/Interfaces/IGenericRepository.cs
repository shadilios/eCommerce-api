using eCommerce.Core.Entities;
using eCommerce.Core.Specifications;

namespace eCommerce.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> Put(int id, T entity);

        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListEntityWithSpecAsync(ISpecification<T> spec);
    }
}
