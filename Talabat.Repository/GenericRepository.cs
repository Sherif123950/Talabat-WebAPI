using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data.Contexts;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Without Spec
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        => await _dbContext.Set<T>().FindAsync(Id);

        #endregion

        #region With Spec
        public async Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }


        public async Task<T> GetByIdAsyncWithSpec(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecifications<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).CountAsync();
        }

        public async Task AddAsync(T Item)
        {
            await _dbContext.Set<T>().AddAsync(Item);
        }

        public void Delete(T Item)
        => _dbContext.Set<T>().Remove(Item);

        public void Update(T Item)
        => _dbContext.Set<T>().Update(Item);
        #endregion
    }
}
