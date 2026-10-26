using System.Text;
using Blazor.WinOld.Pages.Tg;
using Xunit;

namespace Blazor.WinOld.Tests;

public class TgExeBuilderTests
{
    private static uint ReadU32(byte[] bytes, int offset) =>
        (uint)(bytes[offset] | bytes[offset + 1] << 8 | bytes[offset + 2] << 16 | bytes[offset + 3] << 24);

    private static ushort ReadU16(byte[] bytes, int offset) =>
        (ushort)(bytes[offset] | bytes[offset + 1] << 8);

    [Fact]
    public void Build_StartsWithDosSignature()
    {
        var exe = TgExeBuilder.Build(new[] { "Bonjour" });

        Assert.Equal((byte)'M', exe[0]);
        Assert.Equal((byte)'Z', exe[1]);
    }

    [Fact]
    public void Build_HasPeHeaderAtELfanew()
    {
        var exe = TgExeBuilder.Build(new[] { "Bonjour" });

        var eLfanew = ReadU32(exe, 0x3C);
        Assert.Equal((byte)'P', exe[eLfanew]);
        Assert.Equal((byte)'E', exe[eLfanew + 1]);
        Assert.Equal(0, exe[eLfanew + 2]);
        Assert.Equal(0, exe[eLfanew + 3]);

        // COFF machine (i386) and console subsystem.
        Assert.Equal(0x014C, ReadU16(exe, (int)eLfanew + 4));
        var optionalHeader = (int)eLfanew + 24;
        Assert.Equal(3, ReadU16(exe, optionalHeader + 68)); // Subsystem
    }

    [Fact]
    public void Build_SizesAreAligned()
    {
        var exe = TgExeBuilder.Build(new[] { "Bonjour" });

        var eLfanew = ReadU32(exe, 0x3C);
        var optionalHeader = (int)eLfanew + 24;
        var sizeOfImage = ReadU32(exe, optionalHeader + 56);

        Assert.Equal(0u, (uint)exe.Length % 0x200); // file aligned
        Assert.Equal(0u, sizeOfImage % 0x1000);     // image aligned
    }

    [Fact]
    public void Build_EmbedsTheTextAsUtf8()
    {
        var exe = TgExeBuilder.Build(new[] { "Accentué" });

        var needle = Encoding.UTF8.GetBytes("Accentué");
        Assert.True(IndexOf(exe, needle) >= 0, "The UTF-8 text should be embedded in the file.");
    }

    private static int IndexOf(byte[] haystack, byte[] needle)
    {
        for (int i = 0; i <= haystack.Length - needle.Length; i++)
        {
            var found = true;
            for (int j = 0; j < needle.Length; j++)
            {
                if (haystack[i + j] != needle[j]) { found = false; break; }
            }
            if (found) return i;
        }
        return -1;
    }
}
