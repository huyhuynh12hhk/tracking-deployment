using Microsoft.EntityFrameworkCore;

namespace ProductTrackingAPI
{
    public static class ServiceRegistry
    {
        public static async Task<WebApplication> EnsureDataInit<TDbContext>(this WebApplication app)
            where TDbContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }
            return app;
        }
    }
}
