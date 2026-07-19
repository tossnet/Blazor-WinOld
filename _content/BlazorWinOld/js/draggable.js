export function clampContextMenu(el) {
    const vw = window.innerWidth;
    const vh = window.innerHeight;
    const w = el.offsetWidth;
    const h = el.offsetHeight;
    let left = parseFloat(el.style.left) || 0;
    let top = parseFloat(el.style.top) || 0;
    if (left + w > vw) left = Math.max(0, vw - w);
    if (top + h > vh) top = Math.max(0, vh - h);
    el.style.left = left + 'px';
    el.style.top = top + 'px';
}

export function positionSubmenu(el) {
    if (!el) return;

    // Repart d'un état neutre à chaque appel pour re-mesurer la position "naturelle" :
    // évite qu'une classe flip posée avant un redimensionnement de fenêtre ne reste
    // "collée" lors d'une réouverture ultérieure.
    el.classList.remove('flip-h', 'flip-v');

    const rect = el.getBoundingClientRect();
    const vw = window.innerWidth;
    const vh = window.innerHeight;

    if (rect.right > vw) el.classList.add('flip-h');
    if (rect.bottom > vh) el.classList.add('flip-v');
}

export function initDraggable(windowEl, titleBarEl) {
    const rect = windowEl.getBoundingClientRect();
    windowEl.style.position = 'fixed';
    windowEl.style.zIndex = '999999';
    windowEl.style.left = rect.left + 'px';
    windowEl.style.top = rect.top + 'px';
    windowEl.style.margin = '0';

    // Bloque le scroll natif sur la barre de titre (pour tactile)
    titleBarEl.style.touchAction = 'none';

    const hasFinePointer = window.matchMedia('(pointer: fine)').matches;
    if (hasFinePointer) titleBarEl.style.cursor = 'grab';

    let isDragging = false, offsetX = 0, offsetY = 0;

    function onDown(e) {
        if (e.target.closest('button, a, input, select')) return;

        isDragging = true;
        titleBarEl.setPointerCapture(e.pointerId);
        if (hasFinePointer) titleBarEl.style.cursor = 'grabbing';
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
        if (hasFinePointer) titleBarEl.style.cursor = 'grab';
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