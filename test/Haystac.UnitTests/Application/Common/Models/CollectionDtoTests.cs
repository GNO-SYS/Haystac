using System.Text.Json;

using Haystac.Application.Common.Models;

namespace Haystac.UnitTests.Application.Common.Models;

public class CollectionDtoTests
{
    [Theory]
    [MemberData(nameof(JsonFiles))]
    public async Task CanParseFromJsonFile(string path,
        string expectedId, string expectedTitle, string expectedLicense,
        int expectedSpatialCount, int expectedTemporalCount,
        double[] expectedBbox, string[] expectedInterval)
    {
        var json = await File.ReadAllTextAsync(path);

        var dto = JsonSerializer.Deserialize<CollectionDto>(json);

        Assert.NotNull(dto);
        Assert.Equal(expectedId, dto.Identifier);
        Assert.Equal(expectedTitle, dto.Title);
        Assert.Equal(expectedLicense, dto.License);

        Assert.Equal(expectedSpatialCount, dto.Extent.Spatial.BoundingBox.Count);
        Assert.Equal(expectedTemporalCount, dto.Extent.Temporal.Interval.Count);

        Assert.Equal(expectedBbox, dto.Extent.Spatial.BoundingBox.Single());
        Assert.Equal(expectedInterval, dto.Extent.Temporal.Interval.Single());
    }

    public static IEnumerable<object[]> JsonFiles =>
        new List<object[]>
        {
            new object[]
            {
                Path.Combine("Fixtures", "pgelidarclassification.json"),
                "pgelidarclassification",
                "PGE Lidar Classification",
                "proprietary",
                1,
                1,
                new double[] { -120.84, 35.36, -120.82, 35.37 },
                new string[] {"2023-07-18T11:49:05.274387Z", "2023-07-18T11:49:05.274387Z"}
            }
        };
}
