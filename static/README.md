# WinOld for static sites (HTML, PHP...)

The WinOld styles (DOS, Windows 3.1, 98, XP, 7 and 10) packaged as **one plain CSS file**, for any site that does not use Blazor: plain HTML, PHP, WordPress, Hugo, Jekyll, Laravel, Django templates...

Live demo with the HTML of every component: <https://tossnet.github.io/Blazor-WinOld/static-css>

## 1. Get the files

The kit is the [`dist`](dist) folder:

```
dist/
├── winold.css      ← the stylesheet (all components, all themes)
├── winold.js       ← optional: makes tabs, menus, windows, nav panes... interactive
├── fonts/          ← pixel fonts used by DOS / Win 3.1 / 98 / XP
└── icon/           ← images used by the sliders
```

Choose one of these options:

### Option A: CDN (nothing to download)

```html
<link href="https://cdn.jsdelivr.net/gh/tossnet/Blazor-WinOld@master/static/dist/winold.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/gh/tossnet/Blazor-WinOld@master/static/dist/winold.js" defer></script>
```

`@master` always serves the latest version. To freeze a version, replace `master` with a commit hash or a tag (for example `@v3.1.0`).

### Option B: copy the files to your site

1. Download the repository ([Code → Download ZIP](https://github.com/tossnet/Blazor-WinOld/archive/refs/heads/master.zip)) or clone it.
2. Copy the `static/dist` folder into your site, for example as `assets/winold/`.
3. Keep `fonts/` and `icon/` **next to** `winold.css`: the stylesheet references them with relative paths.

```
my-site/
├── index.html (or index.php)
└── assets/
    └── winold/
        ├── winold.css
        ├── winold.js
        ├── fonts/
        └── icon/
```

## 2. Add the stylesheet (and the script)

In the `<head>` of your pages:

```html
<link href="assets/winold/winold.css" rel="stylesheet" />
<script src="assets/winold/winold.js" defer></script>
```

`winold.js` is optional: without it the components are displayed correctly but tabs, list boxes, windows... do not react to clicks. See [Interactivity](#interactivity-winoldjs).

## 3. Write the components

Each component is plain HTML with two classes: a **base** class and a **theme** class.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>My retro page</title>
    <link href="assets/winold/winold.css" rel="stylesheet" />
</head>
<body>
    <div class="win-window win-window-98" style="width: 320px;">
        <div class="win-title-bar win-title-bar-98 active">
            <div class="win-title-text-98">Hello</div>
        </div>
        <div class="win-body win-body-98">
            <div style="padding: 6px;">
                <input type="checkbox" id="remember" class="chk-win chk-win-98">
                <label for="remember">Remember me</label>

                <button class="btn-win btn-win-98 btn-win-98-default">OK</button>
                <button class="btn-win btn-win-98">Cancel</button>
            </div>
        </div>
    </div>
</body>
</html>
```

The [demo](https://tossnet.github.io/Blazor-WinOld/static-css) gives the exact HTML of every component in every theme (click **HTML** under each component).

### Theme classes

| Component    | Base class   | DOS             | Win 3.1          | Win 98           | Win XP           | Win 7           | Win 10           |
|--------------|--------------|-----------------|------------------|------------------|------------------|-----------------|------------------|
| Button       | `btn-win`    | `btn-dos`       | `btn-win-31`     | `btn-win-98`     | `btn-win-xp`     | `btn-win-7`     | `btn-win-10`     |
| CheckBox     | `chk-win`    | `chk-dos`       | `chk-win-31`     | `chk-win-98`     | `chk-win-xp`     | `chk-win-7`     | `chk-win-10`     |
| OptionButton | `opt-win`    | `opt-dos`       | `opt-win-31`     | `opt-win-98`     | `opt-win-xp`     | `opt-win-7`     | `opt-win-10`     |
| TextBox      | `txt-win`    | `txt-win-dos`   | `txt-win-31`     | `txt-win-98`     | `txt-win-xp`     | `txt-win-7`     | `txt-win-10`     |
| Select       | `select-win` | `select-dos`    | `select-win-31`  | `select-win-98`  | `select-win-xp`  | `select-win-7`  | `select-win-10`  |
| ProgressBar  | `prgbar-win` | `prgbar-dos`    | `prgbar-win-31`  | `prgbar-win-98`  | `prgbar-win-xp`  | `prgbar-win-7`  | `prgbar-win-10`  |
| Frame        | `frame-win`  | `frame-dos`     | `frame-win-31`   | `frame-win-98`   | `frame-win-xp`   | `frame-win-7`   | `frame-win-10`   |
| Label        | `lbl-win`    | `lbl-dos`       | `lbl-win-31`     | `lbl-win-98`     | `lbl-win-xp`     | `lbl-win-7`     | `lbl-win-10`     |
| Heading      | `hd-win`     | `hd-dos`        | `hd-win-31`      | `hd-win-98`      | `hd-win-xp`      | `hd-win-7`      | `hd-win-10`      |
| Window       | `win-window` | `win-window-dos`| `win-window-31`  | `win-window-98`  | `win-window-xp`  | `win-window-7`  | `win-window-10`  |

Some components need a few more elements (wrappers, title bar, panels...): copy their HTML from the demo.

Variants: add `-default` to the theme class of a button for the default button (`btn-win-xp-default`), and the `disabled` attribute to disable a control.

## Touch mode

The equivalent of `TouchMode` / `<CascadingValue Name="TouchMode" Value="true">`: the controls are enlarged for touch screens.

- for a whole page or a part of it, add the `wo-touch` class to a container: every component inside it switches to Touch mode (CSS only, no JavaScript needed);
- for a single component, add the `win-touch` class to it, as the Blazor version does.

```html
<body class="wo-touch">
    <button class="btn-win btn-win-xp">OK</button>   <!-- touch size -->
</body>

<!-- Touch mode on touch screens only -->
<script>
    if (matchMedia('(pointer: coarse)').matches) document.body.classList.add('wo-touch');
</script>
```

The dialog boxes opened by `winold.js` inside a `wo-touch` container (e.g. on `<body>`) are in Touch mode too; the `touchMode: true` option only enlarges their title bar, as in Blazor.

## Interactivity (winold.js)

`winold.js` is a small vanilla JavaScript file (no dependency, no configuration) that works directly on the HTML of the components, including HTML added later to the page:

| Component        | Behavior                                                                                       | Event sent (bubbles)                           |
|------------------|------------------------------------------------------------------------------------------------|------------------------------------------------|
| Tabs             | click on a tab shows its panel (`aria-controls` → panel `id`); disabled tabs are ignored         | `winold:tabchange` `{ tab, panel }`            |
| ListBox          | click on an item selects it                                                                    | `winold:select` `{ item, value }`              |
| InfoBar          | the Close button hides the bar                                                                 | `winold:close` (cancelable)                    |
| Window           | Close button hides the window, Maximize button or double-click on the title bar (un)maximizes  | `winold:close` (cancelable), `winold:windowstate` `{ state }` |
| Window           | `data-draggable` attribute: the window can be moved by its title bar inside its container        | `winold:moved` `{ left, top }`                 |
| Slider (Win 3.1) | the arrow buttons change the value by one `step`                                               | native `input` / `change`                      |
| Menu             | click opens a menu, hovering switches menus / opens submenus, click outside or `Esc` closes it  | `winold:menuclick` `{ item, label, value }`    |
| ContextMenu      | `data-contextmenu="id"` on any element: right-click (or long press) opens the menu with this `id` | `winold:menuclick` `{ item, label, value }`    |
| NumberBox        | the spin buttons add / remove `step`, within `min` / `max`                                      | native `input` / `change`                      |
| NavPane          | toggle button collapses the pane, group headers expand groups (one at a time in Win 98), item click selects | `winold:collapse` `{ collapsed }`, `winold:groupchange` `{ group, expanded }`, `winold:select` `{ item, value }` |

Tabs: give every panel an `id` matching the `aria-controls` of its tab, and put `hidden` on the panels that are not selected.
ListBox, Menu, NavPane: the `value` of an item is its `data-value` attribute, or its text (or its `href` for a NavPane link).
Menu items with the `disabled` class (or `aria-disabled="true"` on the label) are ignored.
NavPane: `data-autocollapse-below="600"` collapses the pane when the window is narrower than 600 px; the Win 98 scroll arrows use the same code as the Blazor version.

```html
<div data-contextmenu="my-menu">Right-click here</div>

<ul role="menu" id="my-menu" class="contextmenu-win submenu-win-xp">
    <li role="none" class="menu-sub-win-item">
        <span role="menuitem" class="menu-label-win menu-label-win-xp">Copy</span>
    </li>
    <li role="none" class="menu-sub-win-item" data-value="paste">
        <span role="menuitem" class="menu-label-win menu-label-win-xp">Paste</span>
    </li>
</ul>
```

```html
<ul role="listbox" class="list-win list-win-xp" style="width: 160px;">
    <li role="option" class="list-item list-item-win-xp" data-value="dog">The Dog</li>
    <li role="option" class="list-item list-item-win-xp" data-value="cat">The Cat</li>
</ul>

<script>
    document.addEventListener('winold:select', function (e) {
        console.log('Selected:', e.detail.value);
    });

    // Prevent a window from closing
    document.addEventListener('winold:close', function (e) {
        if (!confirm('Close?')) e.preventDefault();
    });
</script>
```

### Message, input and dialog boxes

The equivalents of `DialogService.ShowMessageBox / ShowInputBox / ShowDialog`: they build the same HTML as the Blazor hosts (modal backdrop, window draggable by its title bar) and return a `Promise`.

```js
// true (OK) / false (Cancel, close button)
const ok = await WinOld.messageBox({
    appearance: 'xp', title: 'Save', icon: 'question',
    message: 'Save the changes?', okButtonText: 'Yes', cancelButtonText: 'No'
});

// the text typed, or null if cancelled
const name = await WinOld.inputBox({ appearance: '98', title: 'Name', message: 'Your name:', value: 'Bob' });

// a number (inputType 'integer', 'decimal' or 'double'), or null
const age = await WinOld.inputBox({
    appearance: '7', title: 'Age', message: 'Your age:',
    inputType: 'integer', value: 30, min: '0', max: '120'
});

// any HTML content: a string, a DOM node or a <template>; onClose(ok, body) runs before the box is removed
const saved = await WinOld.dialog({
    appearance: '10', title: 'Settings', maxButton: true, width: '320px', cancelButtonText: 'Cancel',
    content: '<label>City <input id="city" value="Paris"></label>',
    onClose: (ok, body) => console.log(ok, body.querySelector('#city').value)
});
```

| Option | MessageBox | InputBox | Dialog | |
|---|:-:|:-:|:-:|---|
| `appearance` | ✓ | ✓ | ✓ | `'dos'`, `'31'`, `'98'` (default), `'xp'`, `'7'`, `'10'` |
| `title`, `okButtonText` | ✓ | ✓ | ✓ | `okButtonText` defaults to `'OK'` |
| `cancelButtonText` | ✓ | ✓ | ✓ | no Cancel button when empty (default `'Cancel'` for the InputBox) |
| `message` | ✓ | ✓ | | |
| `icon` | ✓ | | | `'information'`, `'question'`, `'alert'`, `'critical'` |
| `value`, `inputType`, `min`, `max`, `step` | | ✓ | | `inputType`: `'text'` (default), `'integer'`, `'decimal'`, `'double'` |
| `content`, `onOpen(body)`, `onClose(ok, body)` | | | ✓ | |
| `maxButton`, `windowState: 'maximized'`, `width`, `height` | | | ✓ | |
| `dosColor`, `dosButtonColor`, `titleBarColor`, `touchMode` | ✓ | ✓ | ✓ | same meaning as in Blazor |

A small API is also available: `WinOld.init(element)` (to call after adding tabs, draggable windows or nav panes dynamically), `WinOld.selectTab(tab)`, `WinOld.selectListItem(item)`, `WinOld.toggleMaximize(window)`, `WinOld.close(element)`, `WinOld.showContextMenu(menuOrId, x, y)`, `WinOld.closeMenus()`, `WinOld.setNavPaneCollapsed(nav, collapsed)`, `WinOld.toggleNavPaneGroup(group)`, `WinOld.selectNavPaneItem(item)`, `WinOld.messageBox(options)`, `WinOld.inputBox(options)`, `WinOld.dialog(options)`.

## Using it with PHP

Nothing special: `winold.css` is a static file, PHP only generates the HTML. A small helper avoids repeating the classes:

```php
<?php
function winold_button(string $text, string $theme = 'xp', bool $default = false): string
{
    $class = $theme === 'dos' ? 'btn-dos' : "btn-win-$theme";
    $classes = "btn-win $class" . ($default ? " $class-default" : '');
    return '<button class="' . $classes . '">' . htmlspecialchars($text) . '</button>';
}
?>

<?= winold_button('OK', 'xp', true) ?>
<?= winold_button('Cancel', 'xp') ?>
```

## Updating

`dist` is **generated automatically** on each build of the library: `winold.css` from the Blazor components' CSS, `winold.js` from [`src/winold.js`](src/winold.js) (with the drag code of the Blazor version included), so it always matches the Blazor version. To update your site, download the new `dist` folder again (or keep `@master` in the CDN links).

Do not edit the files of `dist`: to customize, add your own stylesheet **after** `winold.css`.

## Limitations

`winold.js` reproduces the behavior of the components, not the Blazor features around them (data binding, `@bind`, forms validation...): read and write the values with the DOM and the events above.
