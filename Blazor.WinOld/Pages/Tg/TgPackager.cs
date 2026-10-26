using System.IO.Compression;
using System.Text;

namespace Blazor.WinOld.Pages.Tg;

/// <summary>
/// Packs the compiled program and its source into a single zip (works under WebAssembly).
/// </summary>
public static class TgPackager
{
    public static byte[] Zip(string name, byte[] exe, string source)
    {
        var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            WriteEntry(archive, $"{name}.EXE", exe);
            WriteEntry(archive, $"{name}.TG", Encoding.UTF8.GetBytes(source));
        }
        return ms.ToArray();
    }

    private static void WriteEntry(ZipArchive archive, string entryName, byte[] content)
    {
        var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
        using var stream = entry.Open();
        stream.Write(content, 0, content.Length);
    }
}
