/*!
 * Blazor.WinOld v{{VERSION}} - static JS (for non-Blazor sites)
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

    // ---------------------------------------------------------------------
    // draggable.js from Blazor.WinOld.Components (same code as the Blazor version)
    // ---------------------------------------------------------------------

    /* @include draggable.js */

    // ---------------------------------------------------------------------
    // navpane.js from Blazor.WinOld.Components (same code as the Blazor version)
    // ---------------------------------------------------------------------

    /* @include navpane.js */

    // ---------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------

    // Theme class of an element: the class starting with prefix (e.g. "tabs-win-" -> "tabs-win-98").
    function themeClass(el, prefix) {
        for (var i = 0; i < el.classList.length; i++) {
            if (el.classList[i].indexOf(prefix) === 0) return el.classList[i];
        }
        return null;
    }

    function emit(el, name, detail, cancelable) {
        return el.dispatchEvent(new CustomEvent(name, { bubbles: true, cancelable: !!cancelable, detail: detail || {} }));
    }

    // ---------------------------------------------------------------------
    // Tabs: <section><menu role="tablist"><button role="tab" aria-controls="panel-x">...
    // ---------------------------------------------------------------------

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

    // ---------------------------------------------------------------------
    // ListBox: <ul role="listbox" class="list-win ..."><li role="option" class="list-item list-item-win-98">
    // ---------------------------------------------------------------------

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

    // ---------------------------------------------------------------------
    // Close (InfoBar and Window): cancelable "winold:close", then hidden
    // ---------------------------------------------------------------------

    function close(el) {
        if (emit(el, 'winold:close', null, true)) el.style.display = 'none';
    }

    // ---------------------------------------------------------------------
    // Window: maximize / restore, drag (data-draggable)
    // ---------------------------------------------------------------------

    function toggleMaximize(win) {
        var maximized = !win.classList.contains('win-window-maximized');
        // Same as the Blazor version: clear the position left by a drag before (un)maximizing
        resetPosition(win);
        // An inline width/height (style="width: 320px") would win over the maximized CSS: set it aside meanwhile
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

    // ---------------------------------------------------------------------
    // Win 3.1 slider: buttons on each side of the range input
    // ---------------------------------------------------------------------

    function nudge(button) {
        var input = button.parentElement.querySelector('input[type="range"]');
        if (!input || input.disabled || button.disabled) return;
        var direction = button.classList.contains('sld-31-left') ? -1 : 1;
        var min = input.min === '' ? 0 : parseFloat(input.min);
        var max = input.max === '' ? 100 : parseFloat(input.max);
        var step = parseFloat(input.step);
        if (!(step > 0)) step = 1;
        var current = parseFloat(input.value);
        // Same rule as the Blazor version: snap to the Step grid anchored on Min
        var next = min + Math.round((current + direction * step - min) / step) * step;
        input.value = Math.min(max, Math.max(min, next));
        input.dispatchEvent(new Event('input', { bubbles: true }));
        input.dispatchEvent(new Event('change', { bubbles: true }));
    }

    // ---------------------------------------------------------------------
    // Menus: <div role="menubar" class="menu-win"> and context menus <ul role="menu" class="contextmenu-win">
    // An item is open when it has the "open" class, and so is its submenu (<ul class="... open">).
    // ---------------------------------------------------------------------

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

    // Opens an item and closes its open siblings (and their descendants), like WinOldMenu.Open
    function openMenuItem(item) {
        var parent = item.parentElement;
        Array.prototype.forEach.call(parent.children, function (sibling) {
            if (sibling !== item && sibling.classList.contains('open')) setMenuItemOpen(sibling, false);
        });
        setMenuItemOpen(item, true);
        var sub = submenuOf(item);
        // Same as the Blazor version: flip the submenu when it overflows the screen
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

    // Transparent full-screen layer behind the open menu (same as Blazor): a click on it closes the menu
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
        // Only one context menu open at a time (as with the Blazor ContextMenuService)
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
            // Menu bar: hovering another title only switches menus once a menu is open
            if (hasSub && isMenuOpen(root) && !item.classList.contains('open')) openMenuItem(item);
            return;
        }
        if (hasSub) {
            if (!item.classList.contains('open')) openMenuItem(item);
        } else {
            closeOpenSiblings(item);
        }
    }

    // Ignore the mouseover synthesized by the browser after a touch (as in Blazor)
    var recentTouch = false, recentTouchTimer = null;
    document.addEventListener('touchend', function () {
        recentTouch = true;
        clearTimeout(recentTouchTimer);
        recentTouchTimer = setTimeout(function () { recentTouch = false; }, 600);
    }, true);

    document.addEventListener('mouseover', function (e) {
        if (!(e.target instanceof Element)) return;
        var item = e.target.closest(MENU_ITEM);
        // mouseover bubbles from the children: only react when entering the item itself
        if (item && !(e.relatedTarget instanceof Element && item.contains(e.relatedTarget))) onMenuItemEnter(item);
    });

    // Context menu trigger: <div data-contextmenu="id-of-the-menu">, right-click or long press (500 ms)
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

    // ---------------------------------------------------------------------
    // NumberBox: <div class="numbox-wrap"><input type="number"> + spin buttons
    // ---------------------------------------------------------------------

    function decimalsOf(step) {
        var dot = step.indexOf('.');
        return dot < 0 ? 0 : step.length - dot - 1;
    }

    function spin(button) {
        var wrap = button.closest('.numbox-wrap');
        var input = wrap && wrap.querySelector('input.numbox-input, input[type="number"]');
        if (!input || input.disabled || button.disabled) return;
        // Same rule as the Blazor version: step from the value (or Min, or 0), clamp to Min/Max, round to the decimals of Step
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

    // ---------------------------------------------------------------------
    // NavPane: <nav class="navpane-win"> with groups (.navpane-group) and items (.navpane-item)
    // ---------------------------------------------------------------------

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
            // Accordion (Win 98 Outlook bar): opening a group closes the others
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

    // Collapsed pane, same DOM as Blazor: a non-clickable header (no chevron) and the items without
    // the scroll arrows wrapper (.navpane-group-scroller of the Win 98 accordion)
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

        // Accordion mode is off while the pane is collapsed (all icons visible)
        if (collapsed) nav._winoldAccordion = nav.classList.contains('navpane-accordion');
        if (nav._winoldAccordion) nav.classList.toggle('navpane-accordion', !collapsed);

        // An inline width would win over the collapsed CSS: set it aside meanwhile
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
                // Collapsed pane: every group shows its items (icons only)
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
        // Labeled separators become plain lines in a collapsed pane
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
            // Groups rendered collapsed in the HTML: hide their items
            nav.querySelectorAll('.navpane-group').forEach(function (group) {
                if (group.classList.contains('navpane-group-collapsed')) setGroupExpanded(group, false);
            });
            // Office 97 scroll arrows (Win 98 accordion): same code as the Blazor version
            if (nav.querySelector('.navpane-group-scroller')) initNavPaneScroll(nav);
            // data-autocollapse-below="600": collapses the pane below this viewport width
            var breakpoint = parseInt(nav.getAttribute('data-autocollapse-below'), 10);
            if (breakpoint > 0) {
                var handle = initNavPaneAutoCollapse({
                    invokeMethodAsync: function (name, narrow) { setPaneCollapsed(nav, narrow); }
                }, breakpoint);
                if (handle.matches()) setPaneCollapsed(nav, true);
            }
        });
    }

    // ---------------------------------------------------------------------
    // Dialog boxes: WinOld.messageBox / WinOld.inputBox / WinOld.dialog (Promise based)
    // Same HTML and classes as WinOldMessageBoxHost / WinOldInputBoxHost / WinOldDialogHost.
    // Appearances: "dos", "31", "98" (default, as in Blazor), "xp", "7", "10".
    // ---------------------------------------------------------------------

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

    // "-win-98" style suffix used by most classes, "-dos" for DOS
    function winSuffix(a) {
        return a === 'dos' ? 'dos' : 'win-' + a;
    }

    function dosColorName(c) {
        return c ? String(c).toLowerCase() : 'default';
    }

    // Same rule as the hosts: green buttons on a default (gray) box, default buttons on a colored box
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

    // Title bar of the dialog boxes (WinOldTitleBar)
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

    // Opens a box: backdrop + window, drag by the title bar (draggable.js), returns a close function
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
        // DOS: the box color follows the icon unless dosColor is given
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

            // Same markup as WinOldTextBox / WinOldNumberBox (without label)
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
            // Same as Blazor: focus the field once displayed
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

            // An inline width/height would win over the maximized CSS class: only set while not maximized
            var maximized = String(o.windowState || '').toLowerCase() === 'maximized';
            function applyState() {
                win.classList.toggle('dlg-win-maximized', maximized);
                win.style.width = maximized || !o.width ? '' : o.width;
                win.style.height = maximized || !o.height ? '' : o.height;
            }
            function toggleMaximized() {
                maximized = !maximized;
                // A drag freezes the position with inline styles that would override the maximized class.
                // Cleared in both directions: Blazor rewrites the style attribute on each render, so a
                // restored dialog goes back to the center there too.
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

    // ---------------------------------------------------------------------
    // Event delegation: works for HTML added later too
    // ---------------------------------------------------------------------

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

    // Double-click on the title bar of an active window with a Maximize button (as in Blazor)
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

    // Public API: call WinOld.init(element) after adding tabs or draggable windows dynamically.
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
