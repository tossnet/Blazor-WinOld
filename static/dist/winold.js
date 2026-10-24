/*!
 * Blazor.WinOld v3.1.3 - static JS (for non-Blazor sites)
 * https://github.com/tossnet/Blazor-WinOld - MIT License
 *
 * Makes the WinOld HTML components interactive without Blazor:
 * tabs, list boxes, info bars, windows (close / maximize / drag), Win 3.1 sliders,
 * menus and context menus, number boxes and navigation panes, plus message / input / dialog boxes:
 *   WinOld.messageBox({ appearance, title, message, icon, okButtonText, cancelButtonText }) -> Promise<boolean>
 *   WinOld.inputBox({ appearance, title, message, value, inputType, min, max, step })       -> Promise<value | null>
 *   WinOld.dialog({ appearance, title, content, maxButton, width, height, onClose })        -> Promise<boolean>
 * No configuration: include this file after winold.css, it works on the HTML of the components.
 *
 * Events (bubbling, dispatched on the component root):
 *   winold:tabchange   detail { tab, panel }          on <section> of the tabs
 *   winold:select      detail { item, value }         on <ul role="listbox">
 *   winold:close       (cancelable)                   on .infobar-win / .win-window
 *   winold:windowstate detail { state }               on .win-window ("maximized" | "normal")
 *   winold:moved       detail { left, top }           on .win-window after a drag
 *   winold:menuclick   detail { item, label, value }  on the menu bar / context menu
 *   winold:select      detail { item, value }         on <nav class="navpane-win"> (item click)
 *   winold:groupchange detail { group, expanded }     on <nav class="navpane-win">
 *   winold:collapse    detail { collapsed }           on <nav class="navpane-win">
 *
 * Source: static/src/winold.js. static/dist/winold.js is GENERATED on each build of
 * Blazor.WinOld.Components (draggable.js and navpane.js of the library are included below): do not edit it.
 */
(function () {
    'use strict';
    function clampContextMenu(el) {
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
    function positionSubmenu(el) {
        if (!el) return;
        el.classList.remove('flip-h', 'flip-v');
        const rect = el.getBoundingClientRect();
        const vw = window.innerWidth;
        const vh = window.innerHeight;
        if (rect.right > vw) el.classList.add('flip-h');
        if (rect.bottom > vh) el.classList.add('flip-v');
    }
    function resetPosition(el) {
        el.style.position = '';
        el.style.left = '';
        el.style.top = '';
        el.style.margin = '';
        el.style.zIndex = '';
    }
    const VV_VARS = ['--winold-vv-left', '--winold-vv-top', '--winold-vv-width', '--winold-vv-height'];
    let vvUsers = 0;
    function updateVisualViewport() {
        const vv = window.visualViewport;
        if (!vv) return;
        const s = document.documentElement.style;
        s.setProperty('--winold-vv-left', vv.offsetLeft + 'px');
        s.setProperty('--winold-vv-top', vv.offsetTop + 'px');
        s.setProperty('--winold-vv-width', vv.width + 'px');
        s.setProperty('--winold-vv-height', vv.height + 'px');
    }
    function trackVisualViewport() {
        if (!window.visualViewport) return;
        if (vvUsers++ === 0) {
            updateVisualViewport();
            window.visualViewport.addEventListener('resize', updateVisualViewport);
            window.visualViewport.addEventListener('scroll', updateVisualViewport);
        }
    }
    function untrackVisualViewport() {
        if (!window.visualViewport || vvUsers === 0) return;
        if (--vvUsers === 0) {
            window.visualViewport.removeEventListener('resize', updateVisualViewport);
            window.visualViewport.removeEventListener('scroll', updateVisualViewport);
            VV_VARS.forEach(v => document.documentElement.style.removeProperty(v));
        }
    }
    function visibleArea() {
        const vv = window.visualViewport;
        return vv
            ? { left: vv.offsetLeft, top: vv.offsetTop, width: vv.width, height: vv.height }
            : { left: 0, top: 0, width: window.innerWidth, height: window.innerHeight };
    }
    function initDraggable(windowEl, titleBarEl, options) {
        const withinParent = options?.withinParent === true;
        const dotNetRef = options?.dotNetRef ?? null;
        titleBarEl.style.touchAction = 'none';
        const hasFinePointer = window.matchMedia('(pointer: fine)').matches;
        if (hasFinePointer) titleBarEl.style.cursor = 'grab';
        let isDragging = false, offsetX = 0, offsetY = 0;
        function onDown(e) {
            if (e.target.closest('button, a, input, select')) return;
            const r = windowEl.getBoundingClientRect();
            if (withinParent) {
                if (windowEl.classList.contains('win-window-maximized')) return;
                if (getComputedStyle(windowEl).position === 'static') windowEl.style.position = 'absolute';
                windowEl.style.left = windowEl.offsetLeft + 'px';
                windowEl.style.top = windowEl.offsetTop + 'px';
            } else {
                windowEl.style.position = 'fixed';
                windowEl.style.zIndex = '999999';
                windowEl.style.left = r.left + 'px';
                windowEl.style.top = r.top + 'px';
                windowEl.style.margin = '0';
            }
            isDragging = true;
            windowEl.classList.add('winold-dragging');
            titleBarEl.setPointerCapture(e.pointerId);
            if (hasFinePointer) titleBarEl.style.cursor = 'grabbing';
            offsetX = e.clientX - r.left;
            offsetY = e.clientY - r.top;
            e.preventDefault();
        }
        function onMove(e) {
            if (!isDragging) return;
            const w = windowEl.offsetWidth, h = windowEl.offsetHeight;
            if (withinParent) {
                const parent = windowEl.offsetParent ?? document.body;
                const p = parent.getBoundingClientRect();
                const maxLeft = Math.max(0, parent.clientWidth - w);
                const maxTop = Math.max(0, parent.clientHeight - h);
                const left = e.clientX - offsetX - p.left - parent.clientLeft + parent.scrollLeft;
                const top = e.clientY - offsetY - p.top - parent.clientTop + parent.scrollTop;
                windowEl.style.left = Math.max(0, Math.min(left, maxLeft)) + 'px';
                windowEl.style.top = Math.max(0, Math.min(top, maxTop)) + 'px';
                return;
            }
            const a = visibleArea();
            windowEl.style.left = Math.max(a.left, Math.min(e.clientX - offsetX, a.left + a.width - w)) + 'px';
            windowEl.style.top = Math.max(a.top, Math.min(e.clientY - offsetY, a.top + a.height - h)) + 'px';
        }
        function onUp() {
            if (!isDragging) return;
            isDragging = false;
            windowEl.classList.remove('winold-dragging');
            if (hasFinePointer) titleBarEl.style.cursor = 'grab';
            dotNetRef?.invokeMethodAsync('OnDragEnd', parseFloat(windowEl.style.left) || 0, parseFloat(windowEl.style.top) || 0);
        }
        titleBarEl.addEventListener('pointerdown', onDown);
        titleBarEl.addEventListener('pointermove', onMove);
        titleBarEl.addEventListener('pointerup', onUp);
        titleBarEl.addEventListener('pointercancel', onUp);
        titleBarEl.addEventListener('lostpointercapture', onUp);
        return {
            dispose: () => {
                titleBarEl.removeEventListener('pointerdown', onDown);
                titleBarEl.removeEventListener('pointermove', onMove);
                titleBarEl.removeEventListener('pointerup', onUp);
                titleBarEl.removeEventListener('pointercancel', onUp);
                titleBarEl.removeEventListener('lostpointercapture', onUp);
                windowEl.classList.remove('winold-dragging');
            }
        };
    }
    const SCROLLER_SELECTOR = '.navpane-group-scroller';
    const ITEMS_SELECTOR = ':scope > .navpane-group-items';
    const BUTTON_SELECTOR = '.navpane-scroll-btn';
    const EPSILON = 1; // subpixel tolerance at both ends of the scroll range
    function initNavPaneScroll(paneEl) {
        let scheduled = false;
        function schedule() {
            if (scheduled) return;
            scheduled = true;
            requestAnimationFrame(() => {
                scheduled = false;
                update();
            });
        }
        function update() {
            for (const scroller of paneEl.querySelectorAll(SCROLLER_SELECTOR)) {
                const items = scroller.querySelector(ITEMS_SELECTOR);
                const up = scroller.querySelector(':scope > .navpane-scroll-up');
                const down = scroller.querySelector(':scope > .navpane-scroll-down');
                if (!items || !up || !down) continue;
                resizeObserver.observe(items);
                up.hidden = items.scrollTop <= EPSILON;
                down.hidden = items.scrollTop + items.clientHeight >= items.scrollHeight - EPSILON;
            }
        }
        function onClick(e) {
            const btn = e.target.closest(BUTTON_SELECTOR);
            if (!btn || !paneEl.contains(btn)) return;
            const items = btn.parentElement.querySelector(ITEMS_SELECTOR);
            if (!items) return;
            const top = items.getBoundingClientRect().top + (parseFloat(getComputedStyle(items).paddingTop) || 0);
            const offsets = Array.from(items.children, child => child.getBoundingClientRect().top - top);
            const offset = btn.classList.contains('navpane-scroll-up')
                ? offsets.filter(o => o < -EPSILON).pop()
                : offsets.find(o => o > EPSILON);
            if (offset !== undefined) items.scrollBy({ top: offset, behavior: 'smooth' });
        }
        const resizeObserver = new ResizeObserver(() => schedule());
        resizeObserver.observe(paneEl);
        const mutationObserver = new MutationObserver(() => schedule());
        mutationObserver.observe(paneEl, { childList: true, subtree: true, characterData: true });
        paneEl.addEventListener('scroll', schedule, true);
        paneEl.addEventListener('click', onClick);
        document.fonts?.ready.then(schedule);
        schedule();
        return {
            dispose: () => {
                resizeObserver.disconnect();
                mutationObserver.disconnect();
                paneEl.removeEventListener('scroll', schedule, true);
                paneEl.removeEventListener('click', onClick);
            }
        };
    }
    function initNavPaneAutoCollapse(dotNetRef, maxWidthPx) {
        const mql = window.matchMedia(`(max-width: ${maxWidthPx - 0.02}px)`);
        const onChange = e => dotNetRef.invokeMethodAsync('OnBreakpointChanged', e.matches);
        mql.addEventListener('change', onChange);
        return {
            matches: () => mql.matches,
            dispose: () => mql.removeEventListener('change', onChange)
        };
    }
    function themeClass(el, prefix) {
        for (var i = 0; i < el.classList.length; i++) {
            if (el.classList[i].indexOf(prefix) === 0) return el.classList[i];
        }
        return null;
    }
    function emit(el, name, detail, cancelable) {
        return el.dispatchEvent(new CustomEvent(name, { bubbles: true, cancelable: !!cancelable, detail: detail || {} }));
    }
    function activeTabClass(tab) {
        var cls = themeClass(tab, 'tabs-win-');               // tabs-win-98
        return cls ? cls.replace('tabs-win-', 'active-tab-win-') : null; // active-tab-win-98
    }
    function selectTab(tab) {
        var list = tab.closest('[role="tablist"]');
        if (!list) return;
        var tabs = list.querySelectorAll('[role="tab"]');
        var panel = null;
        tabs.forEach(function (t) {
            var selected = t === tab;
            var active = activeTabClass(t);
            if (active) t.classList.toggle(active, selected);
            t.setAttribute('aria-selected', selected ? 'true' : 'false');
            var p = document.getElementById(t.getAttribute('aria-controls'));
            if (p) {
                p.hidden = !selected;
                p.style.display = selected ? '' : 'none'; // in case the theme CSS sets a display
                if (selected) panel = p;
            }
        });
        return panel;
    }
    function initTabs(root) {
        root.querySelectorAll('[role="tablist"]').forEach(function (list) {
            var tabs = list.querySelectorAll('[role="tab"]');
            var current = null;
            tabs.forEach(function (t) {
                var active = activeTabClass(t);
                if (!current && active && t.classList.contains(active)) current = t;
            });
            current = current || list.querySelector('[role="tab"]:not([disabled])');
            if (current) selectTab(current);
        });
    }
    function selectListItem(item) {
        var list = item.closest('[role="listbox"]');
        if (!list) return;
        list.querySelectorAll('[role="option"]').forEach(function (li) {
            var selected = li === item;
            var cls = themeClass(li, 'list-item-win-');
            if (cls) li.classList.toggle('active-' + cls, selected);
            li.setAttribute('aria-selected', selected ? 'true' : 'false');
        });
        emit(list, 'winold:select', { item: item, value: item.dataset.value !== undefined ? item.dataset.value : item.textContent.trim() });
    }
    function close(el) {
        if (emit(el, 'winold:close', null, true)) el.style.display = 'none';
    }
    function toggleMaximize(win) {
        var maximized = !win.classList.contains('win-window-maximized');
        resetPosition(win);
        if (maximized) {
            win.dataset.winoldSize = win.style.width + '|' + win.style.height;
            win.style.width = win.style.height = '';
        } else if (win.dataset.winoldSize !== undefined) {
            var size = win.dataset.winoldSize.split('|');
            win.style.width = size[0];
            win.style.height = size[1];
            delete win.dataset.winoldSize;
        }
        win.classList.toggle('win-window-maximized', maximized);
        emit(win, 'winold:windowstate', { state: maximized ? 'maximized' : 'normal' });
    }
    function initWindows(root) {
        root.querySelectorAll('.win-window[data-draggable]').forEach(function (win) {
            if (win._winoldDrag) return;
            var bar = win.querySelector(':scope > .win-title-bar');
            if (!bar) return;
            win._winoldDrag = initDraggable(win, bar, {
                withinParent: true,
                dotNetRef: {
                    invokeMethodAsync: function (name, left, top) { emit(win, 'winold:moved', { left: left, top: top }); }
                }
            });
        });
    }
    function nudge(button) {
        var input = button.parentElement.querySelector('input[type="range"]');
        if (!input || input.disabled || button.disabled) return;
        var direction = button.classList.contains('sld-31-left') ? -1 : 1;
        var min = input.min === '' ? 0 : parseFloat(input.min);
        var max = input.max === '' ? 100 : parseFloat(input.max);
        var step = parseFloat(input.step);
        if (!(step > 0)) step = 1;
        var current = parseFloat(input.value);
        var next = min + Math.round((current + direction * step - min) / step) * step;
        input.value = Math.min(max, Math.max(min, next));
        input.dispatchEvent(new Event('input', { bubbles: true }));
        input.dispatchEvent(new Event('change', { bubbles: true }));
    }
    var MENU_ROOT = '.menu-win[role="menubar"], .contextmenu-win';
    var MENU_ITEM = '.menu-win-root-item, .menu-sub-win-item';
    function menuRoot(el) {
        return el.closest(MENU_ROOT);
    }
    function submenuOf(item) {
        return item.querySelector(':scope > ul[role="menu"]');
    }
    function isMenuItemDisabled(item) {
        var label = item.querySelector(':scope > [role="menuitem"]');
        return item.classList.contains('disabled') || (label && label.getAttribute('aria-disabled') === 'true');
    }
    function setMenuItemOpen(item, open) {
        item.classList.toggle('open', open);
        var sub = submenuOf(item);
        if (sub) sub.classList.toggle('open', open);
        var label = item.querySelector(':scope > [role="menuitem"]');
        if (label && sub) label.setAttribute('aria-expanded', open ? 'true' : 'false');
        if (!open) item.querySelectorAll('.open').forEach(function (el) {
            el.classList.remove('open');
            if (el.matches(MENU_ITEM)) setMenuItemOpen(el, false);
        });
    }
    function openMenuItem(item) {
        var parent = item.parentElement;
        Array.prototype.forEach.call(parent.children, function (sibling) {
            if (sibling !== item && sibling.classList.contains('open')) setMenuItemOpen(sibling, false);
        });
        setMenuItemOpen(item, true);
        var sub = submenuOf(item);
        if (sub && !item.classList.contains('menu-win-root-item')) positionSubmenu(sub);
        ensureMenuBackdrop(menuRoot(item));
    }
    function closeOpenSiblings(item) {
        Array.prototype.forEach.call(item.parentElement.children, function (sibling) {
            if (sibling !== item && sibling.classList.contains('open')) setMenuItemOpen(sibling, false);
        });
    }
    function isMenuOpen(root) {
        return root.classList.contains('contextmenu-win')
            ? root.classList.contains('open')
            : !!root.querySelector(':scope > .menu-win-root-item.open');
    }
    function ensureMenuBackdrop(root) {
        if (!root || root._winoldBackdrop) return;
        var backdrop = document.createElement('div');
        backdrop.className = 'menu-backdrop';
        backdrop.addEventListener('click', function (e) { e.stopPropagation(); closeMenu(root); });
        backdrop.addEventListener('contextmenu', function (e) {
            e.preventDefault();
            e.stopPropagation();
            if (root.classList.contains('contextmenu-win')) showContextMenu(root, e.clientX, e.clientY);
            else closeMenu(root);
        });
        root.parentNode.insertBefore(backdrop, root);
        root._winoldBackdrop = backdrop;
    }
    function closeMenu(root) {
        root.querySelectorAll(':scope > ' + MENU_ITEM.split(', ').join(', :scope > ')).forEach(function (item) {
            if (item.classList.contains('open')) setMenuItemOpen(item, false);
        });
        if (root.classList.contains('contextmenu-win')) root.classList.remove('open');
        if (root._winoldBackdrop) {
            root._winoldBackdrop.remove();
            root._winoldBackdrop = null;
        }
    }
    function closeAllMenus() {
        document.querySelectorAll(MENU_ROOT).forEach(function (root) {
            if (isMenuOpen(root) || root._winoldBackdrop) closeMenu(root);
        });
    }
    function showContextMenu(menu, x, y) {
        if (typeof menu === 'string') menu = document.getElementById(menu);
        if (!menu) return;
        document.querySelectorAll('.contextmenu-win.open').forEach(function (other) {
            if (other !== menu) closeMenu(other);
        });
        closeMenu(menu);
        menu.style.left = Math.round(x) + 'px';
        menu.style.top = Math.round(y) + 'px';
        menu.classList.add('open');
        ensureMenuBackdrop(menu);
        clampContextMenu(menu);
    }
    function onMenuLabelClick(label) {
        var item = label.closest(MENU_ITEM);
        if (!item || isMenuItemDisabled(item)) return;
        var root = menuRoot(item);
        if (submenuOf(item)) {
            if (item.classList.contains('open') && !recentTouch) closeMenu(root);
            else if (!item.classList.contains('open')) openMenuItem(item);
            return;
        }
        closeMenu(root);
        emit(root, 'winold:menuclick', {
            item: item,
            label: label.textContent.trim(),
            value: item.dataset.value !== undefined ? item.dataset.value : label.textContent.trim()
        });
    }
    function onMenuItemEnter(item) {
        if (recentTouch || isMenuItemDisabled(item)) return;
        var root = menuRoot(item);
        if (!root) return;
        var hasSub = !!submenuOf(item);
        if (item.classList.contains('menu-win-root-item')) {
            if (hasSub && isMenuOpen(root) && !item.classList.contains('open')) openMenuItem(item);
            return;
        }
        if (hasSub) {
            if (!item.classList.contains('open')) openMenuItem(item);
        } else {
            closeOpenSiblings(item);
        }
    }
    var recentTouch = false, recentTouchTimer = null;
    document.addEventListener('touchend', function () {
        recentTouch = true;
        clearTimeout(recentTouchTimer);
        recentTouchTimer = setTimeout(function () { recentTouch = false; }, 600);
    }, true);
    document.addEventListener('mouseover', function (e) {
        if (!(e.target instanceof Element)) return;
        var item = e.target.closest(MENU_ITEM);
        if (item && !(e.relatedTarget instanceof Element && item.contains(e.relatedTarget))) onMenuItemEnter(item);
    });
    document.addEventListener('contextmenu', function (e) {
        if (!(e.target instanceof Element)) return;
        var trigger = e.target.closest('[data-contextmenu]');
        if (!trigger) return;
        e.preventDefault();
        showContextMenu(trigger.getAttribute('data-contextmenu'), e.clientX, e.clientY);
    });
    var longPressTimer = null;
    function cancelLongPress() {
        clearTimeout(longPressTimer);
        longPressTimer = null;
    }
    document.addEventListener('touchstart', function (e) {
        if (!(e.target instanceof Element)) return;
        var trigger = e.target.closest('[data-contextmenu]');
        if (!trigger || e.touches.length === 0) return;
        cancelLongPress();
        var x = e.touches[0].clientX, y = e.touches[0].clientY;
        longPressTimer = setTimeout(function () {
            showContextMenu(trigger.getAttribute('data-contextmenu'), x, y);
        }, 500);
    }, { passive: true });
    document.addEventListener('touchend', cancelLongPress, true);
    document.addEventListener('touchmove', cancelLongPress, { passive: true });
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') closeAllMenus();
    });
    function decimalsOf(step) {
        var dot = step.indexOf('.');
        return dot < 0 ? 0 : step.length - dot - 1;
    }
    function spin(button) {
        var wrap = button.closest('.numbox-wrap');
        var input = wrap && wrap.querySelector('input.numbox-input, input[type="number"]');
        if (!input || input.disabled || button.disabled) return;
        var min = input.min === '' ? null : parseFloat(input.min);
        var max = input.max === '' ? null : parseFloat(input.max);
        var step = input.step === '' || input.step === 'any' ? 1 : parseFloat(input.step);
        var current = input.value === '' ? (min !== null ? min : 0) : parseFloat(input.value);
        var next = current + (button.classList.contains('numbox-spin-down') ? -step : step);
        if (min !== null) next = Math.max(next, min);
        if (max !== null) next = Math.min(next, max);
        var text = next.toFixed(decimalsOf(input.step === 'any' ? '' : input.step));
        if (text === input.value) return; // at Min / Max: nothing changes
        input.value = text;
        input.dispatchEvent(new Event('input', { bubbles: true }));
        input.dispatchEvent(new Event('change', { bubbles: true }));
    }
    function groupContent(group) {
        return group.querySelector(':scope > .navpane-group-scroller, :scope > .navpane-group-items');
    }
    function setGroupExpanded(group, expanded) {
        group.classList.toggle('navpane-group-expanded', expanded);
        group.classList.toggle('navpane-group-collapsed', !expanded);
        var header = group.querySelector(':scope > button.navpane-group-header');
        if (header) header.setAttribute('aria-expanded', expanded ? 'true' : 'false');
        var content = groupContent(group);
        if (content) {
            content.hidden = !expanded;
            content.style.display = expanded ? '' : 'none';
        }
    }
    function isPaneCollapsed(nav) {
        return nav.classList.contains('navpane-collapsed');
    }
    function toggleGroup(group) {
        var nav = group.closest('.navpane-win');
        if (!nav || isPaneCollapsed(nav)) return;
        var expanded;
        if (nav.classList.contains('navpane-accordion')) {
            if (group.classList.contains('navpane-group-expanded')) return;
            nav.querySelectorAll('.navpane-group').forEach(function (g) {
                if (g !== group && g.classList.contains('navpane-group-expanded')) {
                    setGroupExpanded(g, false);
                    emit(nav, 'winold:groupchange', { group: g, expanded: false });
                }
            });
            expanded = true;
        } else {
            expanded = !group.classList.contains('navpane-group-expanded');
        }
        setGroupExpanded(group, expanded);
        emit(nav, 'winold:groupchange', { group: group, expanded: expanded });
    }
    function setGroupCompact(group, compact, scrollClass) {
        var button = group.querySelector(':scope > button.navpane-group-header');
        if (button) {
            if (compact && !group._winoldStaticHeader) {
                var header = document.createElement('div');
                header.className = 'navpane-group-header';
                Array.prototype.forEach.call(button.children, function (child) {
                    if (!child.classList.contains('navpane-group-chevron')) header.appendChild(child.cloneNode(true));
                });
                button.parentNode.insertBefore(header, button);
                group._winoldStaticHeader = header;
            } else if (!compact && group._winoldStaticHeader) {
                group._winoldStaticHeader.remove();
                group._winoldStaticHeader = null;
            }
            button.style.display = compact ? 'none' : '';
        }
        var scroller = group.querySelector(':scope > .navpane-group-scroller');
        if (compact && scroller) {
            var items = scroller.querySelector(':scope > .navpane-group-items');
            if (items) {
                group.insertBefore(items, scroller);
                if (scrollClass) items.classList.add(scrollClass);
                group._winoldScroller = scroller;
            }
            scroller.remove();
        } else if (!compact && group._winoldScroller) {
            var moved = group.querySelector(':scope > .navpane-group-items');
            if (moved) {
                if (scrollClass) moved.classList.remove(scrollClass);
                group._winoldScroller.insertBefore(moved, group._winoldScroller.firstChild);
                moved.hidden = false;
                moved.style.display = '';
            }
            group.appendChild(group._winoldScroller);
            group._winoldScroller = null;
        }
    }
    function setPaneCollapsed(nav, collapsed) {
        if (isPaneCollapsed(nav) === collapsed) return;
        nav.classList.toggle('navpane-collapsed', collapsed);
        if (collapsed) nav._winoldAccordion = nav.classList.contains('navpane-accordion');
        if (nav._winoldAccordion) nav.classList.toggle('navpane-accordion', !collapsed);
        if (collapsed) {
            nav.dataset.winoldWidth = nav.style.width;
            nav.style.width = '';
        } else if (nav.dataset.winoldWidth !== undefined) {
            nav.style.width = nav.dataset.winoldWidth;
            delete nav.dataset.winoldWidth;
        }
        var toggle = nav.querySelector('.navpane-header > .navpane-toggle');
        if (toggle) {
            toggle.title = collapsed ? 'Expand' : 'Collapse';
            toggle.setAttribute('aria-expanded', collapsed ? 'false' : 'true');
            var dosArrow = toggle.querySelector('.navpane-toggle-dos-arrow');
            if (dosArrow) dosArrow.textContent = collapsed ? '→' : '←';
        }
        var headerContent = nav.querySelector(':scope > .navpane-header-content');
        if (headerContent) headerContent.style.display = collapsed ? 'none' : '';
        var body = nav.querySelector(':scope > .navpane-body');
        var scrollClass = body ? themeClass(body, 'scroll-') : null;
        nav.querySelectorAll('.navpane-group').forEach(function (group) {
            group.classList.toggle('navpane-group-compact', collapsed);
            setGroupCompact(group, collapsed, scrollClass);
            if (nav._winoldAccordion) group.classList.toggle('navpane-group-accordion', !collapsed);
            if (collapsed) {
                group._winoldExpanded = group.classList.contains('navpane-group-expanded');
                setGroupExpanded(group, true);
            } else if (group._winoldExpanded !== undefined) {
                setGroupExpanded(group, group._winoldExpanded);
            }
        });
        nav.querySelectorAll('.navpane-item').forEach(function (item) {
            item.classList.toggle('navpane-item-compact', collapsed);
            if (collapsed) {
                var text = item.querySelector('.navpane-item-text');
                if (text && !item.title) { item.title = text.textContent.trim(); item._winoldTooltip = true; }
            } else if (item._winoldTooltip) {
                item.removeAttribute('title');
                item._winoldTooltip = false;
            }
        });
        nav.querySelectorAll('.navpane-separator-label > span').forEach(function (span) {
            span.style.display = collapsed ? 'none' : '';
        });
        emit(nav, 'winold:collapse', { collapsed: collapsed });
    }
    function selectNavItem(item) {
        var nav = item.closest('.navpane-win');
        if (!nav || item.disabled || item.getAttribute('aria-disabled') === 'true') return;
        nav.querySelectorAll('.navpane-item').forEach(function (i) {
            var active = i === item;
            i.classList.toggle('navpane-item-active', active);
            if (active) i.setAttribute('aria-current', i.tagName === 'A' ? 'page' : 'true');
            else i.removeAttribute('aria-current');
        });
        var text = item.querySelector('.navpane-item-text');
        var value = item.dataset.value !== undefined ? item.dataset.value
            : item.getAttribute('href') !== null ? item.getAttribute('href')
            : (text ? text.textContent.trim() : '');
        emit(nav, 'winold:select', { item: item, value: value });
    }
    function initNavPanes(root) {
        root.querySelectorAll('.navpane-win').forEach(function (nav) {
            if (nav._winoldInit) return;
            nav._winoldInit = true;
            nav.querySelectorAll('.navpane-group').forEach(function (group) {
                if (group.classList.contains('navpane-group-collapsed')) setGroupExpanded(group, false);
            });
            if (nav.querySelector('.navpane-group-scroller')) initNavPaneScroll(nav);
            var breakpoint = parseInt(nav.getAttribute('data-autocollapse-below'), 10);
            if (breakpoint > 0) {
                var handle = initNavPaneAutoCollapse({
                    invokeMethodAsync: function (name, narrow) { setPaneCollapsed(nav, narrow); }
                }, breakpoint);
                if (handle.matches()) setPaneCollapsed(nav, true);
            }
        });
    }
    var zIndexCounter = 99999; // same as the Blazor DialogService
    var boxCounter = 0;
    function normalizeAppearance(a) {
        a = String(a == null ? '98' : a).toLowerCase().replace(/^win/, '');
        return ['dos', '31', '98', 'xp', '7', '10'].indexOf(a) >= 0 ? a : '10';
    }
    function el(tag, className, attrs) {
        var e = document.createElement(tag);
        if (className) e.className = className;
        if (attrs) Object.keys(attrs).forEach(function (k) { if (attrs[k] != null) e.setAttribute(k, attrs[k]); });
        return e;
    }
    function winSuffix(a) {
        return a === 'dos' ? 'dos' : 'win-' + a;
    }
    function dosColorName(c) {
        return c ? String(c).toLowerCase() : 'default';
    }
    function dosButtonColor(o, boxColor) {
        return o.dosButtonColor ? dosColorName(o.dosButtonColor) : (boxColor === 'default' ? 'green' : 'default');
    }
    function makeButton(a, text, isDefault, dosColor) {
        var base = a === 'dos' ? 'btn-dos' : 'btn-win-' + a;
        var cls = 'btn-win ' + base + (isDefault ? ' ' + base + '-default' : '');
        if (a === 'dos' && dosColor && dosColor !== 'default') cls += ' btn-dos-' + dosColor;
        var b = el('button', cls);
        b.textContent = text;
        return b;
    }
    function makeTitleBar(a, o, titleId, showMaximize) {
        var bar = el('div', 'title-bar-' + winSuffix(a) + ' active' + (o.touchMode ? ' win-touch' : ''));
        if (o.titleBarColor && (a === '31' || a === '10'))
            bar.style.setProperty(a === '31' ? '--dlg-win31-title-bar-background-color-active' : '--dlg-win10-title-bar-background-color-active', o.titleBarColor);
        var text = el('div', 'title-bar-text-' + winSuffix(a), { id: titleId });
        text.textContent = o.title || '';
        bar.appendChild(text);
        var controls = el('div', 'title-bar-controls-' + winSuffix(a));
        var max = null;
        if (showMaximize) {
            max = el('button', null, { 'aria-label': 'Maximize' });
            var glyph = el('span', 'title-bar-dos-glyph', { 'aria-hidden': 'true' });
            glyph.textContent = '↕';
            max.appendChild(glyph);
            controls.appendChild(max);
        }
        var closeButton = el('button', null, { 'aria-label': 'Close' });
        controls.appendChild(closeButton);
        bar.appendChild(controls);
        return { bar: bar, close: closeButton, maximize: max };
    }
    function openBox(backdropClass, win, titleBar, onCancel) {
        var backdrop = el('div', backdropClass);
        backdrop.style.zIndex = ++zIndexCounter;
        backdrop.appendChild(win);
        document.body.appendChild(backdrop);
        trackVisualViewport();
        var drag = initDraggable(win, titleBar.bar);
        titleBar.close.addEventListener('click', onCancel);
        return function () {
            drag.dispose();
            untrackVisualViewport();
            backdrop.remove();
        };
    }
    var ICON_CLASS = { information: 'info', info: 'info', question: 'question', alert: 'alert', warning: 'alert', critical: 'critical', error: 'critical' };
    function messageBox(options) {
        var o = options || {};
        var a = normalizeAppearance(o.appearance);
        var icon = ICON_CLASS[String(o.icon || '').toLowerCase()] || null;
        var boxColor = o.dosColor ? dosColorName(o.dosColor)
            : ({ info: 'green', question: 'cyan', alert: 'brown', critical: 'red' })[icon] || 'default';
        var btnColor = dosButtonColor(o, boxColor);
        var titleId = 'msg-title-' + (++boxCounter);
        return new Promise(function (resolve) {
            var win = el('div', a === 'dos' ? 'msg-dos msg-dos-' + boxColor : 'msg-win-' + a,
                { role: 'alertdialog', 'aria-modal': 'true', 'aria-labelledby': titleId });
            if (o.titleBarColor && a === '10') win.style.setProperty('--dlg-win10-title-bar-border-color-active', o.titleBarColor);
            var titleBar = makeTitleBar(a, o, titleId, false);
            win.appendChild(titleBar.bar);
            var body = el('div', 'message-body message-body-' + winSuffix(a));
            if (icon && a !== 'dos')
                body.appendChild(el('span', 'icon-win icon-' + icon + '-win-' + a, { style: 'width: 32px; height: 32px;', 'aria-hidden': 'true' }));
            var content = el('div', 'msg-content-' + winSuffix(a));
            content.textContent = o.message || '';
            body.appendChild(content);
            win.appendChild(body);
            var actions = el('div', 'actions');
            var ok = makeButton(a, o.okButtonText || 'OK', false, btnColor);
            actions.appendChild(ok);
            var cancel = null;
            if (o.cancelButtonText) {
                cancel = makeButton(a, o.cancelButtonText, false, btnColor);
                cancel.style.marginLeft = '8px';
                actions.appendChild(cancel);
            }
            win.appendChild(actions);
            var close;
            function done(result) { close(); resolve(result); }
            close = openBox('msg-win-backdrop', win, titleBar, function () { done(false); });
            ok.addEventListener('click', function () { done(true); });
            if (cancel) cancel.addEventListener('click', function () { done(false); });
        });
    }
    function inputBox(options) {
        var o = options || {};
        var a = normalizeAppearance(o.appearance);
        var type = String(o.inputType || 'text').toLowerCase();
        var boxColor = dosColorName(o.dosColor);
        var btnColor = dosButtonColor(o, boxColor);
        var titleId = 'inp-title-' + (++boxCounter);
        var inputId = 'inp-input-' + boxCounter;
        return new Promise(function (resolve) {
            var win = el('div', 'inp-win ' + (a === 'dos' ? 'inp-dos inp-dos-' + boxColor : 'inp-win-' + a),
                { role: 'dialog', 'aria-modal': 'true', 'aria-labelledby': titleId });
            if (o.titleBarColor && a === '10') win.style.setProperty('--dlg-win10-title-bar-border-color-active', o.titleBarColor);
            var titleBar = makeTitleBar(a, o, titleId, false);
            win.appendChild(titleBar.bar);
            var container = el('div', 'inp-win-container input-body-' + winSuffix(a));
            var row = el('div', 'inputbox-row');
            var content = el('div', 'inp-content-' + winSuffix(a));
            content.textContent = o.message || '';
            row.appendChild(content);
            var actions = el('div', 'actions');
            var ok = makeButton(a, o.okButtonText || 'OK', true, btnColor);
            actions.appendChild(ok);
            var cancelText = o.cancelButtonText === undefined ? 'Cancel' : o.cancelButtonText;
            var cancel = null;
            if (cancelText) {
                cancel = makeButton(a, cancelText, false, btnColor);
                actions.appendChild(cancel);
            }
            row.appendChild(actions);
            container.appendChild(row);
            var field = el('div', 'input-text');
            var wrapper = el('div');
            wrapper.appendChild(el('label', 'label-txt-win label-txt-hide', { 'for': inputId }));
            var input;
            var txtClass = 'txt-win-' + (a === 'dos' ? 'dos' : a);
            if (type === 'text') {
                input = el('input', 'txt-win ' + txtClass, { type: 'text', id: inputId, style: 'width: 100%' });
                input.value = o.value == null ? '' : String(o.value);
                wrapper.appendChild(input);
            } else {
                var step = o.step || (type === 'integer' ? '1' : '0.01');
                var spinBtn = 'numbox-spin-btn numbox-spin-btn-' + a + (a === 'dos' && btnColor !== 'default' ? ' numbox-spin-btn-dos-' + btnColor : '');
                var wrap = el('div', 'numbox-wrap numbox-wrap-' + a);
                input = el('input', 'txt-win num-win numbox-input ' + txtClass,
                    { type: 'number', id: inputId, style: 'width: 100%', min: o.min, max: o.max, step: step });
                var initial = parseFloat(o.value);
                input.value = isNaN(initial) ? '0' : String(type === 'integer' ? Math.trunc(initial) : initial);
                wrap.appendChild(input);
                var spinBox = el('div', 'numbox-spin numbox-spin-' + a);
                spinBox.appendChild(el('button', spinBtn + ' numbox-spin-up', { type: 'button', tabindex: '-1', 'aria-hidden': 'true' }));
                spinBox.appendChild(el('button', spinBtn + ' numbox-spin-down', { type: 'button', tabindex: '-1', 'aria-hidden': 'true' }));
                wrap.appendChild(spinBox);
                wrapper.appendChild(wrap);
            }
            field.appendChild(wrapper);
            container.appendChild(field);
            win.appendChild(container);
            var close;
            function done(result) { close(); resolve(result); }
            close = openBox('inp-win-backdrop', win, titleBar, function () { done(null); });
            ok.addEventListener('click', function () {
                if (type === 'text') return done(input.value);
                var n = parseFloat(input.value);
                if (isNaN(n)) n = 0;
                done(type === 'integer' ? Math.trunc(n) : n);
            });
            if (cancel) cancel.addEventListener('click', function () { done(null); });
            setTimeout(function () { input.focus(); }, 100);
        });
    }
    function dialog(options) {
        var o = options || {};
        var a = normalizeAppearance(o.appearance);
        var boxColor = dosColorName(o.dosColor);
        var btnColor = dosButtonColor(o, boxColor);
        var titleId = 'dlg-title-' + (++boxCounter);
        return new Promise(function (resolve) {
            var win = el('div', 'dlg-win ' + (a === 'dos' ? 'dlg-dos dlg-dos-' + boxColor : 'dlg-win-' + a),
                { role: 'dialog', 'aria-modal': 'true', 'aria-labelledby': titleId });
            if (o.titleBarColor && a === '10') win.style.setProperty('--dlg-win10-title-bar-border-color-active', o.titleBarColor);
            var titleBar = makeTitleBar(a, o, titleId, !!o.maxButton);
            win.appendChild(titleBar.bar);
            var body = el('div', 'dlg-win-body dlg-body-' + winSuffix(a));
            var c = o.content;
            if (typeof c === 'string') body.innerHTML = c;
            else if (c instanceof HTMLTemplateElement) body.appendChild(c.content.cloneNode(true));
            else if (c instanceof Node) body.appendChild(c);
            win.appendChild(body);
            var actions = el('div', 'actions');
            var ok = makeButton(a, o.okButtonText || 'OK', true, btnColor);
            actions.appendChild(ok);
            var cancel = null;
            if (o.cancelButtonText) {
                cancel = makeButton(a, o.cancelButtonText, false, btnColor);
                cancel.style.marginLeft = '8px';
                actions.appendChild(cancel);
            }
            win.appendChild(actions);
            var maximized = String(o.windowState || '').toLowerCase() === 'maximized';
            function applyState() {
                win.classList.toggle('dlg-win-maximized', maximized);
                win.style.width = maximized || !o.width ? '' : o.width;
                win.style.height = maximized || !o.height ? '' : o.height;
            }
            function toggleMaximized() {
                maximized = !maximized;
                resetPosition(win);
                applyState();
            }
            applyState();
            var close;
            function done(result) {
                if (typeof o.onClose === 'function') o.onClose(result, body);
                close();
                resolve(result);
            }
            close = openBox('dlg-win-backdrop', win, titleBar, function () { done(false); });
            if (titleBar.maximize) {
                titleBar.maximize.addEventListener('click', toggleMaximized);
                titleBar.bar.addEventListener('dblclick', function (e) { if (!e.target.closest('button')) toggleMaximized(); });
            }
            ok.addEventListener('click', function () { done(true); });
            if (cancel) cancel.addEventListener('click', function () { done(false); });
            if (typeof o.onOpen === 'function') o.onOpen(body);
        });
    }
    document.addEventListener('click', function (e) {
        var target = e.target;
        if (!(target instanceof Element)) return;
        var menuLabel = target.closest('.menu-win [role="menuitem"], .contextmenu-win [role="menuitem"]');
        if (menuLabel) {
            onMenuLabelClick(menuLabel);
            return;
        }
        var spinButton = target.closest('.numbox-spin-btn');
        if (spinButton) {
            spin(spinButton);
            return;
        }
        var navToggle = target.closest('.navpane-win .navpane-toggle');
        if (navToggle) {
            if (!navToggle.disabled) {
                var pane = navToggle.closest('.navpane-win');
                setPaneCollapsed(pane, !isPaneCollapsed(pane));
            }
            return;
        }
        var groupHeader = target.closest('.navpane-win button.navpane-group-header');
        if (groupHeader) {
            if (!groupHeader.disabled) toggleGroup(groupHeader.closest('.navpane-group'));
            return;
        }
        var navItem = target.closest('.navpane-win .navpane-item');
        if (navItem) {
            selectNavItem(navItem);
            return;
        }
        var tab = target.closest('[role="tablist"] [role="tab"]');
        if (tab) {
            if (tab.disabled) return;
            var panel = selectTab(tab);
            emit(tab.closest('section') || tab.parentElement, 'winold:tabchange', { tab: tab, panel: panel });
            return;
        }
        var item = target.closest('[role="listbox"] [role="option"]');
        if (item) {
            selectListItem(item);
            return;
        }
        var infobarClose = target.closest('.infobar-win button[aria-label="Close"]');
        if (infobarClose) {
            close(infobarClose.closest('.infobar-win'));
            return;
        }
        var windowButton = target.closest('.win-window > .win-title-bar button');
        if (windowButton) {
            var win = windowButton.closest('.win-window');
            var action = windowButton.getAttribute('aria-label');
            if (action === 'Close') close(win);
            else if (action === 'Maximize') toggleMaximize(win);
            return;
        }
        var sliderButton = target.closest('.sld-31-btn');
        if (sliderButton) nudge(sliderButton);
    });
    document.addEventListener('dblclick', function (e) {
        if (!(e.target instanceof Element) || e.target.closest('button')) return;
        var bar = e.target.closest('.win-window > .win-title-bar.active');
        if (bar && bar.querySelector('button[aria-label="Maximize"]')) toggleMaximize(bar.parentElement);
    });
    function init(root) {
        root = root || document;
        initTabs(root);
        initWindows(root);
        initNavPanes(root);
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', function () { init(); });
    else init();
    window.WinOld = {
        init: init,
        selectTab: selectTab,
        selectListItem: selectListItem,
        toggleMaximize: toggleMaximize,
        close: close,
        showContextMenu: showContextMenu,
        closeMenus: closeAllMenus,
        setNavPaneCollapsed: setPaneCollapsed,
        toggleNavPaneGroup: toggleGroup,
        selectNavPaneItem: selectNavItem,
        messageBox: messageBox,
        inputBox: inputBox,
        dialog: dialog
    };
})();
