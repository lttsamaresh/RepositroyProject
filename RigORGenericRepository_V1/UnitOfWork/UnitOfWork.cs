using Microsoft.EntityFrameworkCore;
using RigORGenericRepository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RigORGenericRepository.UnitOfWork
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    ///  This calss used to for setting DBcontext dynamically from each microservice 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {

        private Dictionary<(Type type, string name), object> _repositories;

        /// <summary>
        /// Using the Constructor we are initializing the _context variable
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
           
        }
        /// <summary>
        /// This Context property will return the DBContext object
        /// </summary>
        public TContext Context
        {
            get;
        }

        /// <summary>
        /// Send DBContext dynamically to generic repoistory calss
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return (IGenericRepository<TEntity>)GetOrAddRepository(typeof(TEntity), new GenericRepository<TEntity>(Context));

        }

        internal object GetOrAddRepository(Type type, Object repo)
        {
            _repositories ??= new Dictionary<(Type type, string name), object>();

            if (_repositories.TryGetValue((type, repo.GetType().FullName), out var repository)) return repository;
            _repositories.Add((type, repo.GetType().FullName), repo);
            return repo;
        }
        /// <summary>
        /// If all the Transactions are completed successfuly then we need to call this Commit() 
        /// </summary>
        /// <param name="autoHistory"></param>
        /// <returns></returns>
        public int Commit(bool autoHistory = false)
        {
            if (autoHistory) Context.EnsureAutoHistory();
            return Context.SaveChanges();
        }

        public async Task<int> CommintAsync(bool autoHistory = false)
        {
            if (autoHistory) Context.EnsureAutoHistory();
            return await Context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
