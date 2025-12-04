using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CleanArchitecture.Infrastructure.Data.DataContext;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Load connection string from API appsettings.json for design-time
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        // Design-time only: locate API project folder to read appsettings without relying on current working dir
        static string ResolveApiProjectDir()
        {
            var start = new DirectoryInfo(AppContext.BaseDirectory);
            var current = start;
            for (int i = 0; i < 6 && current != null; i++)
            {
                var apiDir = Path.Combine(current.FullName, "CleanArchitecture.API");
                if (Directory.Exists(apiDir)) return apiDir;
                current = current.Parent;
            }
            // Fallback to solution dir if present
            var cwd = Directory.GetCurrentDirectory();
            var candidate = Path.Combine(cwd, "CleanArchitecture.API");
            return Directory.Exists(candidate) ? candidate : cwd;
        }

        var baseDir = ResolveApiProjectDir();

        string? connectionString = null;

        // Try environment-specific appsettings first, then base appsettings
        var envAppSettingsPath = Path.Combine(baseDir, $"appsettings.{environment}.json");
        var appSettingsPath = Path.Combine(baseDir, "appsettings.json");

        string? TryReadConnection(string path)
        {
            if (!File.Exists(path)) return null;
            try
            {
                using var stream = File.OpenRead(path);
                using var doc = JsonDocument.Parse(stream);
                if (doc.RootElement.TryGetProperty("ConnectionStrings", out var connSection) &&
                    connSection.TryGetProperty("DefaultConnection", out var defaultConn))
                {
                    return defaultConn.GetString();
                }
            }
            catch { }
            return null;
        }

        connectionString = TryReadConnection(envAppSettingsPath)
            ?? TryReadConnection(appSettingsPath)
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=CleanArchDbdotnet10;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
