using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T:BaseEntity
    {
        #region WithOut Specification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
        #endregion

        #region With Specification
        Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> Spec);
        Task<T> GetByIdAsyncWithSpec(ISpecifications<T> Spec);
        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);
        #endregion
        Task AddAsync(T Item);
        void Delete(T Item);
        void Update(T Item);

    }
}
