using System.Text.RegularExpressions;

namespace Blazor.WinOld.Pages.Tg;

/// <summary>
/// Result of a Turbo GERARD compilation: the text of each writeln, or the errors.
/// </summary>
public sealed record TgCompileResult(IReadOnlyList<string> Lines, IReadOnlyList<string> Errors)
{
    public bool Success => Errors.Count == 0;
}

/// <summary>
/// Fake compiler of the Turbo GERARD language, which only knows <c>writeln('...');</c>.
/// </summary>
public static partial class TgCompiler
{
    // writeln('text'); with '' for a quote in the text, the ; is optional.
    [GeneratedRegex(@"^\s*writeln\s*\(\s*'((?:[^']|'')*)'\s*\)\s*;?\s*$", RegexOptions.IgnoreCase)]
    private static partial Regex WritelnRegex();

    public static TgCompileResult Compile(string? source)
    {
        var lines = new List<string>();
        var errors = new List<string>();
        var sourceLines = (source ?? string.Empty).Split('\n');

        for (var i = 0; i < sourceLines.Length; i++)
        {
            var line = sourceLines[i].TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var match = WritelnRegex().Match(line);
            if (match.Success)
                lines.Add(match.Groups[1].Value.Replace("''", "'"));
            else
                errors.Add($"Line {i + 1}: unknown statement");
        }

        return new TgCompileResult(lines, errors);
    }
}
