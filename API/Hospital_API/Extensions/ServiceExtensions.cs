using Hospital_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital_API.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Connecting to the Microsoft SQL Database.
    /// The host, name, and password must be specified in the docker-compose.yml configuration file.
    /// environment:
    ///  - DB_HOST=host
    ///  - DB_NAME=name
    ///  - DB_SA_PASSWORD=password
    /// </summary>
    public static IServiceCollection AddMsSQLDatabase(this IServiceCollection services)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var password = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

        var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa; Password={password}";

        services.AddDbContext<AppDBContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        return services;
    }
}
