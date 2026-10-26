# Changelog

All notable changes to **BlazorWinOld** are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- `WinOldNavPaneItem`: new CSS variables for the Win10 style: `--winold-navpane-accent-color` (vertical bar of the selected item and badge), `--winold-navpane-active-background` (background of the selected item) and `--winold-navpane-hover-background` (background on hover), e.g. `:root { --winold-navpane-accent-color: #c42b1c; }`, or set them on the pane's `Class`. Without them, the colors are unchanged. Also available in the static version.
- Test project `Blazor.WinOld.Tests` (xUnit + bUnit), with a test that fails if the library contains an anonymous type (they break JS interop once the app is trimmed).
- Demo: the **Turbo GERARD** screen (`/tg`) is now a toy IDE. Type `writeln('...')` lines in a window, then **Compiler** / `F9` runs a fake compiler, builds a tiny real Windows console `.exe` (a PE written by hand, no toolchain needed) that prints the text, and downloads it zipped with its source. **Exécuter** / `Ctrl+F9` runs the program in a fake console drawn in the page, so it works on any OS.

### Fixed
- `WinOldMessageBox`, `WinOldInputBox`, `WinOldDialog`, `WinOldWindow`: `NotSupportedException` (`ConstructorContainsNullParameterNames`) when opening a box in a published (trimmed) application. The drag options were sent to `draggable.js` as an anonymous type, whose constructor parameter names are removed by trimming; they are now sent as a dictionary.

## [3.1.1] - 2026-10-24

### Added
- Static version for sites without Blazor (plain HTML, PHP, WordPress...), in the `static/dist` folder of the repository or from the jsDelivr CDN. Guide: [static/README.md](static/README.md).
  - `winold.css`: all the styles of the components in one file, with `fonts/` and `icon/`. It is generated on each build of the library from the components' CSS, so it always matches the Blazor version (the CI fails if the committed `static/dist` is not up to date).
  - `winold.js` (optional, vanilla JavaScript): tabs, list boxes, info bars, windows (close, maximize, drag), Win 3.1 sliders, menus and context menus (`data-contextmenu`), number boxes and navigation panes, plus `WinOld.messageBox()`, `WinOld.inputBox()` and `WinOld.dialog()`, which return a `Promise` and build the same boxes as the `DialogService`. The drag and menu positioning code is the one of the library (`draggable.js`, `navpane.js`).
  - Touch mode: the `wo-touch` class on a container (e.g. `<body>`) enlarges every component inside it, like `<CascadingValue Name="TouchMode">`.
- Demo site: new **Static sites** page (installation guide, usage, live demo of every component in every style), announced on the home page.

### Changed
- `WinOldNavPaneGroup`: in Win10 style, the items of a group are indented by 24 px under their header when the pane is expanded (no indent in compact mode).

### Fixed
- `WinOldNavPane`: `ObjectDisposedException` (`DotNetObjectReference<WinOldNavPane>`) when the pane was disposed while `navpane.js` was still loading, ex: a layout replaced by a redirect right after its first render with `AutoCollapseBelow` set. Interop handles obtained after disposal are now released.

### Fixed
- `WinOldNumberBox`: the margin (and the Win 3.1 font) of its label are limited to its own label. No change in Blazor, where CSS isolation already did it; needed by the static version.
- `WinOldMessageBox`, `WinOldInputBox`, `WinOldDialog`, `WinOldSelect`: CSS rules that could never apply (they targeted the buttons or the `<option>` elements rendered by other components) are disabled. No change in Blazor; in the static version they turned the WinXP dialog buttons into white text on blue, and the Win 3.1 / DOS options into bold.

## [3.1.0] - 2026-10-12

### Added
- `WinOldDialog`, `WinOldMessageBox`, `WinOldInputBox`, `WinOldWindow`: in DOS style, while the title bar is held down (to move the window), the double-line frame becomes a single line in the color of the close box, like Turbo Vision. It returns to a double line when released.
- `WinOldWindow`: new `Active` parameter (default `true`). An inactive window, in DOS style, has no Close or Maximize button, a single gray line instead of the double line, and a gray title; in Win31, a white title bar with a black title; in Win98, a gray gradient title bar with a light gray title; in WinXP, a pastel blue title bar and frame, a light title and faded buttons; in Win7 and Win10 its title bar takes the inactive colors. `WinOldTitleBar` gets the same `Active` parameter. Demo added to the **DialogBox** page.
- `WinOldWindow`: new `DosColor` parameter (DOS only) for the window background, same palette as the DOS dialog.
- `WinOldWindow`: new `WindowState` / `WindowStateChanged` parameters (`@bind-WindowState`) to maximize or restore the window from code.
- `WinOldWindow`: new `Draggable` parameter to move the window by its title bar, inside its positioned container (it does not leave it, and keeps its `z-index`). A maximized window can't be dragged. New `OnMoved` callback (`WindowMovedEventArgs` with `Left` / `Top`): store the position and render it back in `Style`, otherwise a later change of `Style` puts the window back where it was.
- `WinOldMenu`, `WinOldMenuItem`: new CSS variable `--winold-menu-color` for the text color of the menu bar, the drop-down menus and the context menus, in every style (e.g. `:root { --winold-menu-color: #333; }`, or set it on the menu's `Class`). Without it, each style keeps its own color. Hover and disabled colors are unchanged.

### Fixed
- `WinOldMenu`, `WinOldMenuItem`: a generic rule of the host app such as `li { color: ... }` no longer changes the text color of the drop-down and context menu items; they take the color of their menu again. Any rule of the app that uses a class (e.g. `.my-menu li`) still applies.

## [3.0.0] - 2026-10-11

### Added
- `WinOldMessageBox`, `WinOldInputBox`, `WinOldDialog`: new `TouchMode` option (`MessageBoxOptions`, `InputBoxOptions`, `DialogOptions`; a cascaded `TouchMode` around the hosts works too). It enlarges the title bar and its buttons: Win10, Win7 , WinXP , Win98. DOS and Win 3.1 are unchanged. Test buttons added to the **TouchMode** demo page.
- `WinOldDialog`, `WinOldMessageBox`, `WinOldInputBox`: new `TitleBarColor` option (`DialogOptions`, `MessageBoxOptions`, `InputBoxOptions`) to change the title bar color of one window (any CSS color). Supported by Win 3.1 and Win10; in Win10 the window border follows it. Examples added to the **DialogBox** demo page.

### Fixed
- `WinOldCheckBox`, `WinOldOptionButton`, `WinOldLabel`, `WinOldHeading` and the labels of `WinOldTextBox`, `WinOldNumberBox`, `WinOldSlider`: in Win98 style, the text stays black when the device is in dark mode (seen on Android). The dark-mode rules that turned it white on the gray Win98 background were removed.

## [2.22.0] - 2026-10-05

### Added
- `WinOldNavPane`: new `AutoCollapseBelow` parameter (viewport width in px). Below it, the pane starts collapsed (e.g. on a phone in portrait), and it collapses or expands again when the viewport crosses it (rotation, resize). A manual toggle is kept until then. Used by the immersive demo (`AutoCollapseBelow="600"`).

### Fixed
- `WinOldDialog`, `WinOldMessageBox`, `WinOldInputBox`: on smartphones, the window is now centered on the visible screen. When the page overflowed horizontally, mobile browsers widened the layout viewport and the window opened shifted to the right (and partly off-screen). The maximized dialog and the drag area are also limited to the visible screen.
- `WinOldToolbarButton`: with `Flat="true"` in WinXP and Win7 styles, the label is no longer white on a light page when the device is in dark mode (seen on Android). At rest it now takes the text color of its container.
- Demo site: on mobile, the documentation tables scroll horizontally instead of widening the page.

## [2.21.0] - 2026-10-01

### Added
- New component `WinOldHeading`: semantic heading (`h1` to `h6` with `Level`, default `h2`) styled like the titles of each appearance (DOS, Win 3.1, Win98, WinXP, Win7, Win10), with disabled state and a new **Heading** demo page.

### Changed
- `WinOldTabs`: `Appearance` is now nullable and falls back to the Appearance of the hosting `WinOldWindow` / `WinOldAppShell` (default still Win10). Its effective Appearance is cascaded to the tab content, so a `WinOldToolbar`, `WinOldMenu`, `WinOldStatusBar` or `WinOldNavPane` placed in a tab panel now inherits the style of the tabs.
- `WinOldMenu`: `TouchMode` (parameter or cascaded) now enlarges the menu bar items and the drop-down / context menu items (32px minimum height), submenus included.
- `WinOldLabel`: disabled style for Win 3.1, Win98, WinXP, Win7 and Win10 (previously DOS only). The **Label** demo page now shows every appearance, the disabled state, rich content and Touch Mode.

### Fixed
- `WinOldCheckBox`, `WinOldTextBox`, `WinOldLabel`: in Win98 style, the label text is now white when the browser is in dark mode.

## [2.20.0] - 2026-10-01

### Added
- Scrollbar button styles for Windows XP and Windows 7.

### Changed
- CSS refactored to use `var(--win31-background)` for consistent theming.

## [2.19.0] - 2026-09-30

### Added
- New component `WinOldAppShell`: application layout with `Menu`, `Toolbar`, `SidePane` and `StatusBar` areas, sections for the pages, dialog hosts included.
- New component `WinOldNavPane` with `WinOldNavPaneGroup`, `WinOldNavPaneItem` and `WinOldNavPaneSeparator` (side navigation pane): Office 97 Outlook bar accordion with arrow scroll buttons for Win98, task pane for WinXP, Explorer pane for Win7, Groove / Windows 11 pane collapsible to an icon rail for Win10. Items with `Href` follow the current URL.
- New component `WinOldStatusBar` with `WinOldStatusBarPanel`: bar always at the bottom of its container, panels with optional width, icon and alignment, always filling the full width.
- `WinOldWindow`: new `StatusBar` slot, rendered flush against the bottom edge of the window.
- New scrollbar styles for every appearance (`scroll-win-*` classes, `Appearance.ToScrollbarClass()`, `IJSRuntime.SetScrollbarAppearanceAsync()` for the whole page), with a new **Scrollbar** demo page.
- `WinOldListBox`: new DOS, Win 3.1 and Win10 styles.
- Demo: new full-screen **Immersive demo** (`/immersive`), built with `WinOldAppShell`, where the toolbar switches the whole application between the 6 styles.

### Changed
- `WinOldMenu` and `WinOldToolbar`: `Appearance` is now nullable and falls back to the Appearance of the hosting `WinOldWindow` / `WinOldAppShell` (default still Win10).

### Fixed
- `WinOldListBox`: the initial `SelectedValue` is now highlighted and follows external changes.

## [2.18.0] - 2026-09-29

### Added
- New component `WinOldIcon` (system icons Information, Question, Alert, Critical, usable anywhere).
- New component `WinOldInfoBar` (inline closable message bar with icon, title, message and actions).

## [2.17.0] - 2026-09-29

### Added
- New component `WinOldProgressBar`.

## [2.16.0] - 2026-09-28

### Changed
- Several improvements.

## [2.15.0] - 2026-09-26

### Added
- `WinOldNumberBox`: added `Change` event and `Id` parameter.

### Changed
- The Win10 appearance is now the default if you forget to set the `Appearance` parameter on any component.
- Improved dark mode support.

## [2.14.0] - 2026-09-25

### Added
- **Windows 3.1** appearance: `WinOldWindow`, `WinOldDialogHost`, `WinOldMessageBoxHost`, `WinOldInputBoxHost`.

## [2.13.0] - 2026-09-22

### Added
- DOS appearance: InputBox, SelectBox, NumberBox.

## [2.12.0] - 2026-09-21

### Added
- `Format` parameter on `WinOldNumberBox` (e.g. `Format="F2"`).

## [2.11.0] - 2026-09-20

### Added
- New **DOS** components: `WinOldCheckBox`, `WinOldOptionButton`, `WinOldLabel`, `WinOldTextBox`, `WinOldTabs`, `WinOldMenu`, `WinOldMessageBox`.

## [2.10.0] - 2026-09-13

### Added
- Maximize button (with double-click-to-fullscreen) on `WinOldWindow` and `WinOldDialogHost`.

### Changed
- Extracted a shared `WinOldTitleBar` component to remove duplicated title-bar code across Window/Dialog/InputBox/MessageBox.
- Consolidated duplicated title-bar CSS into a single shared stylesheet, cutting ~60 KB from the package.
- Improved documentation and examples for `WinOldDialog`.

## [2.9.0] - 2026-09-05

### Changed
- Replaced `WinOldNumberBox`'s native spin arrows with custom theme-accurate up/down buttons (Win98/WinXP/Win7/Win10). WinXP and Win10 styles still need polishing.

### Fixed
- Win98 spin buttons not scaling in `TouchMode`.

## [2.8.8] - 2026-09-04

### Fixed
- `WinOldSlider` thumb snapping back to a stale position during fast drags, caused by redundant event handling forcing an unnecessary re-render on every pointer move.

## [2.8.7] - 2026-07-29

### Fixed
- MessageBox/Dialog/InputBox z-index stacking: the most recently opened one always renders on top.
- `WinOldDialog` mis-centering: the position-freeze in `draggable.js` is now deferred until the user actually starts dragging, instead of being captured on first render before scrollable content settles its final height.

## [2.8.6] - 2026-07-28

### Fixed
- `input[type="date"]` (and related date/time types) on `WinOldTextBox` no longer stacks its internal fields vertically on iOS/WebKit Safari.

## [2.8.4] - 2026-07-28

### Fixed
- Improvements and bug fixes (`WinOldToolbarButton`, `WinOldTextBox` with `type="date"` on WebKit…).

## [2.8.3] - 2026-07-27

### Fixed
- `WinOldToolbar`: Collapse mode clipping button labels mid-word instead of collapsing to icon-only, by preventing native flexbox shrinking on toolbar buttons.

## [2.8.2] - 2026-07-26

### Changed
- `WinOldToolbar`: Collapse mode now shrinks buttons to icon-only one at a time from the right as space runs out, instead of all at once.

### Removed
- `WinOldToolbar`: `CollapseWidth` parameter (collapsing is now fully automatic).

## [2.8.1] - 2026-07-26

### Fixed
- `WinOldToolbarButton`: flat-mode buttons never showed the "active" (toggled-on) style, since the flat-mode CSS override took precedence over `.active`.

## [2.8.0] - 2026-07-26

### Added
- New component `WinOldToolbar`, to build a toolbar with buttons and separators.

## [2.7.3] - 2026-07-19

### Fixed
- `WinOldMenu`: submenus opening off-screen are now repositioned to stay within the viewport when near an edge.

## [2.7.2] - 2026-07-16

### Fixed
- `WinOldButton`: Win10 buttons had no persistent "pressed" state, only the transient `:active` pseudo-class (added `.btn-win-10:not(:disabled).active`).

## [2.7.1] - 2026-07-13

### Fixed
- `WinOldTextArea`: label above the field.

## [2.7.0] - 2026-07-13

### Added
- New component `WinOldTextArea`, a multiline text input with the same look and feel as `WinOldTextBox` and a configurable `Rows` parameter.

## [2.6.7] - 2026-07-13

### Added
- `TouchMode` support for `WinOldTabs`, increasing tab height for easier touch interaction.

### Fixed
- Pixelated "MS Sans Serif" font rendering in WinXP/Win98 touch mode for `WinOldTextBox`, by using the system font instead.

## [2.6.6] - 2026-07-11

### Added
- `WinOldTextBox.SelectAllAsync()` to focus and select the entire content of a text box.

## [2.6.5] - 2026-07-10

### Fixed
- Message box text rendering: `\n` line breaks in `MessageBoxOptions.Message` are preserved (`white-space: pre-line`).

## [2.6.4] - 2026-07-03

### Added
- `ChildContent` support on `WinOldOptionButton`, allowing custom markup (not just plain text) as the option's label.

## [2.6.3] - 2026-07-01

### Changed
- Improved touch support for `WinOldSelect` (WinXP and Win98) and checkbox UI.

## [2.6.2] - 2026-06-30

### Added
- Win10 label style.

### Changed
- Improved touch support and checkbox UI.

## [2.6.1] - 2026-06-30

### Added
- Button demo page: custom style example.

### Changed
- Touch mode font sizes are now scaled up to 13px for Win7/Win10 themes; Win98/WinXP themes keep their native 11px pixel font with antialiasing disabled to preserve crisp rendering.

## [2.6.0] - 2026-06-30

### Added
- `TouchMode` property on `WinOldButton`, `WinOldCheckBox`, `WinOldOptionButton`, `WinOldTextBox`, `WinOldNumberBox` and `WinOldSelect`: enlarges controls for easier finger interaction on touch screens. Usable per component or cascaded via `<CascadingValue Name="TouchMode" Value="true">`.

## [2.5.6]

### Changed
- Context menu now automatically repositions itself to stay within the browser viewport when it would overflow the right or bottom edge.

## [2.5.5] - 2026-06-11

### Added
- CSS shadow on the Win10 submenu.

## [2.5.4] - 2026-06-11

### Changed
- Multiple context menus on the same page now automatically close each other when a new one is opened.

## [2.5.3] - 2026-06-11

### Added
- `ShowContextMenu(double clientX, double clientY)` overload on `WinOldMenu`, to open the context menu programmatically from code-behind without a `MouseEventArgs`.

## [2.5.2] - 2026-06-10

### Fixed
- Dialog overflow on small screens: the dialog width is now capped at the viewport width.

## [2.5.1] - 2026-06-09

### Added
- Generic `ShowDialog<TComponent>(options, parameters)` extension method to render any Blazor component as dialog content without requiring a `RenderFragment`.

## [2.5.0] - 2026-04-23

### Added
- New component `WinOldSlider`.

## [2.4.0] - 2026-04-14

### Added
- `WinOldMenuItem`: new `IconCssClass` and `IconTemplate` parameters. Display icons from any external library (Bootstrap Icons, Font Awesome, custom SVG…) without bundling any icon dependency in the package.

## [2.3.0] - 2026-04-12

### Added
- `WinOldMenu` can be used as a context menu by setting `IsContextMenu` to `true`.

## [2.2.1]

### Added
- Accessibility: ARIA attributes (`role`, `aria-expanded`, `aria-selected`, `aria-modal`, `aria-labelledby`…) on all components.

## [2.2.0] - 2026-04-11

### Added
- New component `WinOldMenu`.
- Button: default style for WinXP and Win7.

## [2.1.0] - 2026-04-05

### Added
- New component `WinOldNumberBox` for numeric input, with support for generic types.
- `WinOldInputBox`: support for numeric types `int`, `decimal` and `double`.
- New extension methods `ShowInputBox<int?>()`, `ShowInputBox<decimal?>()` and `ShowInputBox<double?>()`.

## [2.0.0] - 2026-04-03

### Added
- New Windows 10 style theme.

### Changed
- Windows 10 is now the default style applied to all components.

## [1.5.0] - 2026-03-30

### Added
- New component `WinOldDialog`: a fully draggable dialog window supporting custom child content.

## [1.4.0] - 2026-03-15

### Added
- Drag-and-drop support for MessageBox and InputBox, allowing users to move the window freely across the screen.

## [1.3.1] - 2026-02-12

### Added
- `WinOldTabs`: a tab can be disabled.

### Changed
- Improved disabled style for some components (Frame, Options, Tab).

## [1.3.0] - 2026-02-10

### ⚠️ Breaking changes
- `DialogOptions` has been renamed to `MessageBoxOptions` for `WinOldMessageBox`.
  Migration: replace `new DialogOptions` with `new MessageBoxOptions` (simple Find/Replace).

### Added
- New component `WinOldInputBox`.
- Button: `Default` property style for the Win98 button.

## [1.2.9] - 2026-02-04

### Fixed
- Height of Tabs.
- Disabled style of `WinOldButton`.

## [1.2.8] - 2026-01-29

### Changed
- Win7: font size separated from font family.

## [1.2.7] - 2026-01-26

### Changed
- Improved checkbox component rendering.

### Fixed
- Button and selectbox label colors on Safari.

## [1.2.6] - 2026-01-23

### Added
- `disabled` attribute support on `WinOldSelect`.
- Select page in the demo site.

## [1.2.2] - 2025-03-16

### Added
- `.AddWinOldComponents()` to simplify service registration.
