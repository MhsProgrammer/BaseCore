using BaseCore.Application.Contracts.Persistance;
using BaseCore.Domain.Common;
using BaseCore.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaseCoreContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories = new();
        public UnitOfWork(BaseCoreContext context)
        {
            _context = context;
        }
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;

            return (IAsyncRepository<TEntity>)_repositories.GetOrAdd(type , t =>
            {
                var repositoryType = typeof(AsyncRepository<>).MakeGenericType(typeof(TEntity));
                return Activator.CreateInstance(repositoryType, _context)
                ?? throw new InvalidOperationException(
                    $"Could not create repository instance for {t}"
                    );
            });
        }
    }
}
