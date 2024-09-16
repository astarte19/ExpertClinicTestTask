using ExpertClinicTestTask.Domain.Interfaces.Account;
using ExpertClinicTestTask.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExpertClinicTestTask.Infrastructure.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfRepositories(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationContext>(
            options =>
            {
                options.UseSqlServer(connectionString);
            }
            , ServiceLifetime.Transient
        );

        services.AddScoped<Dictionary<Type, ApplicationContext>>();
        services.AddSingleton<DbContextFactory>();
        
        services.AddTransient<IAccountRepository, AccountRepository>();
        
        return services;
    }
}