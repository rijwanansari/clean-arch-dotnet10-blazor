using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CleanArchitecture.Infrastructure.Data.DataContext;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use environment variable for connection string, fallback to LocalDB for development
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=CleanArchDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
