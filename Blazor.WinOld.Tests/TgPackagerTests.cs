using System.IO.Compression;
using System.Text;
using Blazor.WinOld.Pages.Tg;
using Xunit;

namespace Blazor.WinOld.Tests;

public class TgPackagerTests
{
    [Fact]
    public void Zip_ContainsExeAndSource()
    {
        var exe = new byte[] { 1, 2, 3, 4, 5 };
        const string source = "writeln('hi');";

        var zipBytes = TgPackager.Zip("TG1", exe, source);

        using var archive = new ZipArchive(new MemoryStream(zipBytes), ZipArchiveMode.Read);
        var exeEntry = archive.GetEntry("TG1.EXE");
        var tgEntry = archive.GetEntry("TG1.TG");

        Assert.NotNull(exeEntry);
        Assert.NotNull(tgEntry);

        Assert.Equal(exe, ReadAll(exeEntry!));
        Assert.Equal(source, Encoding.UTF8.GetString(ReadAll(tgEntry!)));
    }

    private static byte[] ReadAll(ZipArchiveEntry entry)
    {
        using var stream = entry.Open();
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
