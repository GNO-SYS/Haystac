namespace Haystac.Infrastructure.Identity;

public record class TestUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? ClientId { get; set; } = null;

    public ICollection<string> Roles { get; set; } = new HashSet<string>();

    public ICollection<string> Policies { get; set; } = new HashSet<string>();
}