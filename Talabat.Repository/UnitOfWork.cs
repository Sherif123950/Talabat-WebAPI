using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data.Contexts;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private Hashtable _Repositories = new Hashtable();

        public UnitOfWork(StoreDbContext dbContext)
        {
            this._dbContext = dbContext;

        }
        public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Type = typeof(TEntity).Name;
            if (!_Repositories.ContainsKey(Type))
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _Repositories.Add(Type, Repository);
            }
            return _Repositories[Type] as IGenericRepository<TEntity>;
        }
    }
}
