using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CatalogService.Infrastructure.Data
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-NOG8RIP\\SQLEXPRESS;Database=CatalogDb;uid=sa;pwd=1;encrypt=true;trustServerCertificate=true;"
            );

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}