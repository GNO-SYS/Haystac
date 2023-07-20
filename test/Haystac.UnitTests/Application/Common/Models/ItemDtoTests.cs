using System.Text.Json;

using Haystac.Application.Common.Models;

namespace Haystac.UnitTests.Application.Common.Models;

public class ItemDtoTests
{
    [Theory]
    [MemberData(nameof(ItemJsonFiles))]
    public async Task CanParseFromJsonFile(string path,
        string expectedId, string expectedCollection)
    {
        var json = await File.ReadAllTextAsync(path);

        var itemDto = JsonSerializer.Deserialize<ItemDto>(json);

        Assert.NotNull(itemDto);
        Assert.Equal(expectedId, itemDto.Identifier);
        Assert.Equal(expectedCollection, itemDto.Collection);
    }

    public static IEnumerable<object[]> ItemJsonFiles =>
        new List<object[]>
        {
            new object[] 
            { 
                Path.Combine("Fixtures", "stac_item.json"),
                "PGE_2_46.las",
                "pgelidarclassification"
            }
        };
}
