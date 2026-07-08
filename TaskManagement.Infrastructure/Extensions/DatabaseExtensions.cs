using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync();
    }
}
