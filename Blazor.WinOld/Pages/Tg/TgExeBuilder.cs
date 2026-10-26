using System.Text;

namespace Blazor.WinOld.Pages.Tg;

/// <summary>
/// Builds a tiny Windows console program that prints the given lines, then waits for Enter.
/// The whole file is written by hand (no C compiler needed); the equivalent C source is:
/// <code>
/// // tgstub.c
/// #include &lt;windows.h&gt;
/// int main(void) {
///     SetConsoleOutputCP(65001); // UTF-8
///     DWORD n;
///     WriteFile(GetStdHandle(STD_OUTPUT_HANDLE), msg, msgLen, &amp;n, 0);
///     char c;
///     ReadFile(GetStdHandle(STD_INPUT_HANDLE), &amp;c, 1, &amp;n, 0); // keep the window open
///     ExitProcess(0);
/// }
/// </code>
/// </summary>
public static class TgExeBuilder
{
    private const uint ImageBase = 0x0040_0000;
    private const uint SectionAlignment = 0x1000;
    private const uint FileAlignment = 0x200;
    private const uint TextRva = 0x1000; // first (and only) section

    public static byte[] Build(IEnumerable<string> lines)
    {
        var text = string.Join("\r\n", lines);
        if (text.Length > 0) text += "\r\n";
        text += "Press Enter to close...\r\n";
        var message = Encoding.UTF8.GetBytes(text);

        // Layout of the .text section (all offsets are from the start of the section).
        // [code] [import directory + thunks] [kernel32 name + function names] [message] [scratch]
        var section = new MemoryStream();
        var w = new BinaryWriter(section);

        // Placeholders patched once the layout is known.
        var codeOffset = 0;

        // --- Reserve space by writing the pieces in order, tracking their section offsets. ---
        // 1) Code: filled last, so first compute where the data lives.
        //    We emit the code into a temporary buffer once offsets are known, below.

        // The import lookup/address thunks and names.
        // kernel32 functions we import, in call order of their IAT slots.
        string[] functions =
        {
            "SetConsoleOutputCP",
            "GetStdHandle",
            "WriteFile",
            "ReadFile",
            "ExitProcess"
        };

        // We first decide the section offset of each block.
        // Code size is fixed once functions/message addresses are known; build code into a buffer
        // using absolute virtual addresses (ImageBase + TextRva + offset).

        // Compute offsets. Start past a generous code area; we will assert the real code fits.
        // Instead of guessing, assemble in two passes: pass 1 with placeholder VAs to get code length.
        // The code length does not depend on the actual address values, only on the instructions,
        // so a single assembly with the final addresses is correct if we lay data after a fixed code size.

        // Simpler: lay data first at a fixed offset leaving room for code, then assemble code.
        // Build the data blocks into a buffer and record their section offsets relative to a base.

        // --- Build data block (import dir, thunks, names, message) in a buffer. ---
        var data = new MemoryStream();
        var dw = new BinaryWriter(data);

        // We don't yet know the section offset where 'data' starts (after the code).
        // Record offsets within 'data', then add the data base once chosen.

        // Import Directory Table: one descriptor for kernel32 + a null terminator descriptor.
        // Each descriptor is 20 bytes.
        int importDirOffset = 0; // within data
        // 2 descriptors
        for (int i = 0; i < 2 * 20; i++) dw.Write((byte)0);

        // Import Lookup Table (ILT) and Import Address Table (IAT): N entries + null, 4 bytes each.
        int iltOffset = (int)data.Length;
        for (int i = 0; i < functions.Length + 1; i++) dw.Write(0u);
        int iatOffset = (int)data.Length;
        for (int i = 0; i < functions.Length + 1; i++) dw.Write(0u);

        // Hint/Name entries (2-byte hint + name + null, word aligned).
        var nameOffsets = new int[functions.Length];
        for (int i = 0; i < functions.Length; i++)
        {
            nameOffsets[i] = (int)data.Length;
            dw.Write((ushort)0); // hint
            dw.Write(Encoding.ASCII.GetBytes(functions[i]));
            dw.Write((byte)0);
            if (data.Length % 2 != 0) dw.Write((byte)0);
        }

        // DLL name.
        int dllNameOffset = (int)data.Length;
        dw.Write(Encoding.ASCII.GetBytes("kernel32.dll"));
        dw.Write((byte)0);
        if (data.Length % 2 != 0) dw.Write((byte)0);

        // Message bytes.
        int messageOffset = (int)data.Length;
        dw.Write(message);

        // Scratch dword for the "bytes written/read" out parameter.
        if (data.Length % 4 != 0)
            for (int p = (int)(data.Length % 4); p < 4; p++) dw.Write((byte)0);
        int scratchOffset = (int)data.Length;
        dw.Write(0u);
        int inputBufOffset = (int)data.Length;
        dw.Write((byte)0);

        var dataBytes = data.ToArray();

        // --- Assemble code, now that we can compute addresses. ---
        // The data block is placed right after the code. We need the code length to know the data base.
        // Assemble twice: the instruction sequence is fixed, so first assemble with a provisional data
        // base to get the exact code length, then re-assemble with the real base. Since x86 near calls
        // and mov-immediates are fixed length regardless of the operand value, the length is stable.
        byte[] AssembleCode(uint dataBaseVa)
        {
            uint iatVa = dataBaseVa + (uint)iatOffset;
            uint msgVa = dataBaseVa + (uint)messageOffset;
            uint scratchVa = dataBaseVa + (uint)scratchOffset;
            uint inputBufVa = dataBaseVa + (uint)inputBufOffset;

            // IAT slot VAs by function index (indirect call through the IAT).
            uint Slot(int index) => iatVa + (uint)(index * 4);

            var code = new MemoryStream();
            var c = new BinaryWriter(code);

            void CallIndirect(int fnIndex)
            {
                // FF 15 <abs addr of IAT slot>
                c.Write((byte)0xFF);
                c.Write((byte)0x15);
                c.Write(Slot(fnIndex));
            }
            void PushImm(uint v) { c.Write((byte)0x68); c.Write(v); }
            void PushByte(sbyte v) { c.Write((byte)0x6A); c.Write((byte)v); }

            // SetConsoleOutputCP(65001)
            PushImm(65001);
            CallIndirect(0);

            // h = GetStdHandle(-11)  -> EAX
            PushImm(unchecked((uint)-11));
            CallIndirect(1);

            // WriteFile(h, msg, len, &scratch, 0)  (stdcall: push right-to-left)
            c.Write((byte)0x6A); c.Write((byte)0x00);      // push 0 (overlapped)
            PushImm(scratchVa);                            // push &scratch
            PushImm((uint)message.Length);                 // push len
            PushImm(msgVa);                                // push msg
            c.Write((byte)0x50);                           // push eax (handle)
            CallIndirect(2);

            // h = GetStdHandle(-10)  -> EAX (stdin)
            PushImm(unchecked((uint)-10));
            CallIndirect(1);

            // ReadFile(h, &buf, 1, &scratch, 0)
            c.Write((byte)0x6A); c.Write((byte)0x00);      // push 0
            PushImm(scratchVa);                            // push &scratch
            PushByte(1);                                   // push 1
            PushImm(inputBufVa);                           // push &buf
            c.Write((byte)0x50);                           // push eax (handle)
            CallIndirect(3);

            // ExitProcess(0)
            c.Write((byte)0x6A); c.Write((byte)0x00);
            CallIndirect(4);

            return code.ToArray();
        }

        // Pass 1: provisional base to measure code length.
        int codeLen = AssembleCode(ImageBase + TextRva + 0x100).Length;
        uint dataBase = ImageBase + TextRva + (uint)codeOffset + (uint)codeLen;
        // Pass 2: final code with the real data base.
        var codeBytes = AssembleCode(dataBase);

        // Fix any length drift (should be zero): pad/truncate is not expected.
        if (codeBytes.Length != codeLen)
            throw new InvalidOperationException("Code length changed between passes.");

        // --- Patch the import directory, ILT and IAT with real RVAs. ---
        uint DataRva(int offsetInData) => TextRva + (uint)codeLen + (uint)offsetInData;

        void PatchU32(byte[] buf, int offset, uint value)
        {
            buf[offset] = (byte)value;
            buf[offset + 1] = (byte)(value >> 8);
            buf[offset + 2] = (byte)(value >> 16);
            buf[offset + 3] = (byte)(value >> 24);
        }

        // Import descriptor: OriginalFirstThunk (ILT RVA), TimeDateStamp, ForwarderChain,
        // Name (DLL name RVA), FirstThunk (IAT RVA).
        PatchU32(dataBytes, importDirOffset + 0, DataRva(iltOffset));
        PatchU32(dataBytes, importDirOffset + 12, DataRva(dllNameOffset));
        PatchU32(dataBytes, importDirOffset + 16, DataRva(iatOffset));

        // ILT and IAT: RVA of each hint/name entry; both tables identical before loading.
        for (int i = 0; i < functions.Length; i++)
        {
            PatchU32(dataBytes, iltOffset + i * 4, DataRva(nameOffsets[i]));
            PatchU32(dataBytes, iatOffset + i * 4, DataRva(nameOffsets[i]));
        }

        // --- Compose the full section (code + data). ---
        w.Write(codeBytes);
        w.Write(dataBytes);
        var sectionBytes = section.ToArray();

        uint sectionRawSize = Align((uint)sectionBytes.Length, FileAlignment);
        uint sectionVirtualSize = (uint)sectionBytes.Length;
        uint sizeOfImage = Align(TextRva + sectionVirtualSize, SectionAlignment);
        uint entryPointRva = TextRva + (uint)codeOffset;
        uint importDirRva = DataRva(importDirOffset);
        uint importDirSize = 2 * 20;
        uint iatRva = DataRva(iatOffset);
        uint iatSize = (uint)((functions.Length + 1) * 4);

        const uint headersRaw = 0x200;
        const uint textFileOffset = 0x200;

        return WritePe(
            sectionBytes, sectionRawSize, sectionVirtualSize,
            entryPointRva, sizeOfImage,
            importDirRva, importDirSize, iatRva, iatSize,
            headersRaw, textFileOffset);
    }

    private static uint Align(uint value, uint alignment) =>
        (value + alignment - 1) / alignment * alignment;

    private static byte[] WritePe(
        byte[] sectionBytes, uint sectionRawSize, uint sectionVirtualSize,
        uint entryPointRva, uint sizeOfImage,
        uint importDirRva, uint importDirSize, uint iatRva, uint iatSize,
        uint headersRaw, uint textFileOffset)
    {
        var ms = new MemoryStream();
        var w = new BinaryWriter(ms);

        // DOS header: 'MZ' + e_lfanew at 0x3C pointing to the PE header at 0x40.
        w.Write((byte)'M');
        w.Write((byte)'Z');
        while (ms.Length < 0x3C) w.Write((byte)0);
        w.Write(0x40u); // e_lfanew

        // PE signature at 0x40.
        w.Write((byte)'P'); w.Write((byte)'E'); w.Write((byte)0); w.Write((byte)0);

        // COFF file header.
        w.Write((ushort)0x014C);     // Machine: i386
        w.Write((ushort)1);          // NumberOfSections
        w.Write(0u);                 // TimeDateStamp
        w.Write(0u);                 // PointerToSymbolTable
        w.Write(0u);                 // NumberOfSymbols
        w.Write((ushort)0xE0);       // SizeOfOptionalHeader
        w.Write((ushort)0x0103);     // Characteristics: RELOCS_STRIPPED | EXECUTABLE_IMAGE | 32BIT

        // Optional header (PE32).
        w.Write((ushort)0x010B);     // Magic PE32
        w.Write((byte)0);            // MajorLinkerVersion
        w.Write((byte)0);            // MinorLinkerVersion
        w.Write(sectionRawSize);     // SizeOfCode
        w.Write(0u);                 // SizeOfInitializedData
        w.Write(0u);                 // SizeOfUninitializedData
        w.Write(entryPointRva);      // AddressOfEntryPoint
        w.Write(TextRva);            // BaseOfCode
        w.Write(TextRva + sectionVirtualSize); // BaseOfData (PE32 only)
        w.Write(ImageBase);          // ImageBase
        w.Write(SectionAlignment);   // SectionAlignment
        w.Write(FileAlignment);      // FileAlignment
        w.Write((ushort)4);          // MajorOperatingSystemVersion
        w.Write((ushort)0);          // MinorOperatingSystemVersion
        w.Write((ushort)0);          // MajorImageVersion
        w.Write((ushort)0);          // MinorImageVersion
        w.Write((ushort)4);          // MajorSubsystemVersion
        w.Write((ushort)0);          // MinorSubsystemVersion
        w.Write(0u);                 // Win32VersionValue
        w.Write(sizeOfImage);        // SizeOfImage
        w.Write(headersRaw);         // SizeOfHeaders
        w.Write(0u);                 // CheckSum
        w.Write((ushort)3);          // Subsystem: console
        w.Write((ushort)0);          // DllCharacteristics
        w.Write(0x00100000u);        // SizeOfStackReserve
        w.Write(0x00001000u);        // SizeOfStackCommit
        w.Write(0x00100000u);        // SizeOfHeapReserve
        w.Write(0x00001000u);        // SizeOfHeapCommit
        w.Write(0u);                 // LoaderFlags
        w.Write(16u);                // NumberOfRvaAndSizes

        // Data directories (16). Only Import (1) and IAT (12) are set.
        for (int i = 0; i < 16; i++)
        {
            switch (i)
            {
                case 1: w.Write(importDirRva); w.Write(importDirSize); break;
                case 12: w.Write(iatRva); w.Write(iatSize); break;
                default: w.Write(0u); w.Write(0u); break;
            }
        }

        // Section header: ".text".
        var name = new byte[8];
        Encoding.ASCII.GetBytes(".text").CopyTo(name, 0);
        w.Write(name);
        w.Write(sectionVirtualSize);     // VirtualSize
        w.Write(TextRva);                // VirtualAddress
        w.Write(sectionRawSize);         // SizeOfRawData
        w.Write(textFileOffset);         // PointerToRawData
        w.Write(0u);                     // PointerToRelocations
        w.Write(0u);                     // PointerToLinenumbers
        w.Write((ushort)0);              // NumberOfRelocations
        w.Write((ushort)0);              // NumberOfLinenumbers
        w.Write(0xE0000020u);            // CODE | EXECUTE | READ | WRITE

        // Pad to the start of the section raw data.
        while (ms.Length < textFileOffset) w.Write((byte)0);

        // Section raw data, padded to FileAlignment.
        w.Write(sectionBytes);
        while (ms.Length < textFileOffset + sectionRawSize) w.Write((byte)0);

        return ms.ToArray();
    }
}
