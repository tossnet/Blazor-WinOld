export function initDraggable(windowEl, titleBarEl) {
    const rect = windowEl.getBoundingClientRect();
    windowEl.style.position = 'fixed';
    windowEl.style.zIndex = '999999';
    windowEl.style.left = rect.left + 'px';
    windowEl.style.top = rect.top + 'px';
    windowEl.style.margin = '0';

    titleBarEl.style.cursor = 'grab';

    let isDragging = false, offsetX = 0, offsetY = 0;

    function onDown(e) {
        if (e.target.closest('button, a, input, select')) return;

        isDragging = true;
        titleBarEl.setPointerCapture(e.pointerId);
        titleBarEl.style.cursor = 'grabbing';
        const r = windowEl.getBoundingClientRect();
        offsetX = e.clientX - r.left;
        offsetY = e.clientY - r.top;
        e.preventDefault();
    }

    function onMove(e) {
        if (!isDragging) return;
        const vw = window.innerWidth, vh = window.innerHeight;
        const w = windowEl.offsetWidth, h = windowEl.offsetHeight;
        windowEl.style.left = Math.max(0, Math.min(e.clientX - offsetX, vw - w)) + 'px';
        windowEl.style.top = Math.max(0, Math.min(e.clientY - offsetY, vh - h)) + 'px';
    }

    function onUp() {
        isDragging = false;
        titleBarEl.style.cursor = 'grab';
    }

    titleBarEl.addEventListener('pointerdown', onDown);
    titleBarEl.addEventListener('pointermove', onMove);
    titleBarEl.addEventListener('pointerup', onUp);
    titleBarEl.addEventListener('pointercancel', onUp);

    return {
        dispose: () => {
            titleBarEl.removeEventListener('pointerdown', onDown);
            titleBarEl.removeEventListener('pointermove', onMove);
            titleBarEl.removeEventListener('pointerup', onUp);
            titleBarEl.removeEventListener('pointercancel', onUp);
        }
    };
}