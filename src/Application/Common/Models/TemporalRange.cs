using Haystac.Application.Search.Queries;

namespace Haystac.Application.Common.Models;

//< See: https://github.com/radiantearth/stac-api-spec/blob/release/v1.0.0/implementation.md#datetime-parameter-handling
public record TemporalRange
{
    public DateTime Start { get; internal set; }
    public DateTime End { get; internal set; }

    public bool Touches(TemporalRange other)
    {
        return Start.CompareTo(other.End) <= 0 && End.CompareTo(other.Start) <= 0;
    }
    
    public static TemporalRange? Generate(Item item)
    {
        var dt = item.GetDateTime();

        if (dt == null)
        {
            (var start, var end) = item.GetDateTimeRange();

            if (start == null && end == null) return null;

            return new TemporalRange
            {
                Start = start ?? DateTime.MinValue,
                End = end ?? DateTime.MaxValue
            };
        }
        else
        {
            return new TemporalRange { Start = (DateTime)dt, End = (DateTime)dt };
        }
    }

    public static TemporalRange? Generate(SearchQuery query)
    {
        if (query.DateTime == null) return null;

        var vals = query.DateTime.ToUpper().Split('/');

        var start = ParseDateTime(vals[0]);

        return new TemporalRange
        {
            Start = start,
            End = vals.Length > 1 ? ParseDateTime(vals[1], isStart: false) : start,
        };
    }

    static DateTime ParseDateTime(string value, bool isStart = true)
    {
        if (value == "..") return isStart ? DateTime.MinValue : DateTime.MaxValue;

        return DateTime.Parse(value, null, DateTimeStyles.RoundtripKind);
    }
}
