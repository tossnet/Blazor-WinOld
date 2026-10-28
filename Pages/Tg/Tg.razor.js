// Turbo GERARD demo: file download and keyboard handling.

// Downloads bytes streamed from .NET as a file, without base64 round-tripping.
export async function downloadFile(fileName, streamRef) {
    const buffer = await streamRef.arrayBuffer();
    const blob = new Blob([buffer], { type: "application/octet-stream" });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
}

// Prevents the browser's default only for the app shortcuts (F1..F12 and Alt+key),
// so typing in the textarea keeps working.
export function initKeys(screen) {
    if (!screen) return;
    screen.addEventListener("keydown", (e) => {
        const isFunctionKey = /^F\d{1,2}$/.test(e.key);
        if (isFunctionKey || e.altKey) {
            e.preventDefault();
        }
    });
}
