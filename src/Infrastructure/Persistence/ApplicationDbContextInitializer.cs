using Microsoft.Extensions.Logging;

namespace Haystac.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
                                           ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the DB");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the DB");
            throw;
        }
    }

    async Task TrySeedAsync()
    {
        //< Add the 'test' Client entity
        _context.Clients.Add(new Client
        {
            Name = "TestClient",
            ContactName = "Test",
            ContactEmail = "test@client.com"
        });

        //< TODO - Any test collection/items for the 'test' Client

        await _context.SaveChangesAsync();
    }
}
