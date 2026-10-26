using Blazor.WinOld.Pages.Tg;
using Xunit;

namespace Blazor.WinOld.Tests;

public class TgCompilerTests
{
    [Fact]
    public void Compile_SingleWriteln_ReturnsItsText()
    {
        var result = TgCompiler.Compile("writeln('Bonjour');");

        Assert.True(result.Success);
        Assert.Equal(new[] { "Bonjour" }, result.Lines);
    }

    [Fact]
    public void Compile_MultipleLines_ReturnsEachText()
    {
        var result = TgCompiler.Compile("writeln('un');\nwriteln('deux');");

        Assert.True(result.Success);
        Assert.Equal(new[] { "un", "deux" }, result.Lines);
    }

    [Fact]
    public void Compile_DoubledQuote_IsUnescaped()
    {
        var result = TgCompiler.Compile("writeln('t''en as marre');");

        Assert.True(result.Success);
        Assert.Equal("t'en as marre", Assert.Single(result.Lines));
    }

    [Theory]
    [InlineData("WRITELN('a');")]           // case insensitive
    [InlineData("  writeln ( 'a' ) ;  ")]   // spaces around tokens
    [InlineData("writeln('a')")]            // optional semicolon
    public void Compile_AcceptsVariants(string source)
    {
        var result = TgCompiler.Compile(source);

        Assert.True(result.Success);
        Assert.Equal("a", Assert.Single(result.Lines));
    }

    [Fact]
    public void Compile_BlankLines_AreIgnored()
    {
        var result = TgCompiler.Compile("\n\nwriteln('x');\n   \n");

        Assert.True(result.Success);
        Assert.Equal("x", Assert.Single(result.Lines));
    }

    [Fact]
    public void Compile_UnknownLine_ReportsErrorWithLineNumber()
    {
        var result = TgCompiler.Compile("writeln('ok');\nreadln();");

        Assert.False(result.Success);
        Assert.Equal("Line 2: unknown statement", Assert.Single(result.Errors));
    }
}
