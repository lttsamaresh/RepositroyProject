using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigORGenericRepository.UnitOfWork;
using System.Diagnostics.CodeAnalysis;

namespace RigORGenericRepository
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// This class is used to add generic repository to dependency injection 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceLibary(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

           // services.AddScoped(typeof(IUnitOfWorkV1), typeof(UnitOfWorkV1));

          
            return services;
        }
    }
}
