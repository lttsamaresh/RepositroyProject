using Microsoft.EntityFrameworkCore;
using RigORGenericRepository.GenericRepository;
using System;
using System.Threading.Tasks;

namespace RigORGenericRepository.UnitOfWork
{
    /// <summary>
    /// This calss used to for setting DBcontext dynamically from each microservice 
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        int Commit(bool autoHistory = false);
        Task<int> CommintAsync(bool autoHistory = false);

    }
    /// <summary>
    /// This class used to get dynamic DBcontext value
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
