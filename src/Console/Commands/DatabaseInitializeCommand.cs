using Haystac.Infrastructure.Persistence;

namespace Haystac.Console.Commands;

public class DatabaseInitializeCommand : AsyncCommand<DatabaseInitializeCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--seed")]
        [Description("Optional flag that indicates if we should try to 'seed' the Database with randomized data")]
        public bool SeedDatabase { get; set; } = false;
    }

    private readonly ApplicationDbContextInitializer _context;

    public DatabaseInitializeCommand(ApplicationDbContextInitializer context)
    {
        _context = context;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider("Initializing Database");

        try
        {
            await _context.InitializeAsync();

            if (settings.SeedDatabase)
            {
                Helper.Write("Seeding Database");
                await _context.SeedAsync();
                Helper.Write("\t .. Done!");
            }
        }
        catch (Exception ex)
        {
            Helper.Write($"Failed to initialize DB due to: {ex}");
            throw;
        }

        Helper.Write($"Successfully initialized DB!");
        return 0;
    }
}
