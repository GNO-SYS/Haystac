namespace Haystac.Domain.Common;

public abstract class BaseStacEntity : BaseEntity
{
    /// <summary>
    /// The STAC version this <see cref="BaseStacEntity"/> implements
    /// </summary>
    public string StacVersion { get; set; } = string.Empty;

    /// <summary>
    /// A list of STAC extensions this <see cref="BaseStacEntity"/> implements
    /// </summary>
    public List<string>? Extensions { get; set; }

    /// <summary>
    /// The provider's identifier for the <see cref="BaseStacEntity"/>
    /// </summary>
    public string Identifier { get; set; } = string.Empty;
}
