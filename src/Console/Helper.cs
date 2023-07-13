namespace Haystac.Console;

public static class Helper
{
    /// <summary>
    /// The 'short' date time format for log file and other indexing
    /// </summary>
    public const string DateFormat = "yyMMdd_HHmmss";
    /// <summary>
    /// ISO8601 DateTime format for text serialization
    /// </summary>
    public const string ISO8601DateFormat = @"yyyy-MM-ddTHH:mm:ss.fffffffZ";

    public static string GetCurrentDateTimeString(string format = DateFormat)
        => DateTime.Now.ToString(format);

    public static string GetEscapedFileName(string file)
        => Path.GetFileName(file).EscapeMarkup();

    public static void Write(string message)
    {
        string dt = GetCurrentDateTimeString(ISO8601DateFormat);
        AnsiConsole.MarkupLine($"[grey]{dt}:[/] {message}");
    }

    public static void WriteDivider(string text)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule($"[yellow]{text}[/]").RuleStyle("grey").LeftJustified());
    }
}