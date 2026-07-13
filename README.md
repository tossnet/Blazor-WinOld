# Blazor.WinOld

[![NuGet](https://img.shields.io/nuget/v/BlazorWinOld.svg)](https://www.nuget.org/packages/BlazorWinOld/)  ![BlazorWinOld Nuget Package](https://img.shields.io/nuget/dt/BlazorWinOld)
[![GitHub](https://img.shields.io/github/license/tossnet/Blazor-WinOld?color=594ae2&logo=github&style=flat-square)](https://github.com/tossnet/Blazor-WinOld/blob/master/LICENSE.txt)

A Blazor UI component library that brings back the nostalgic look and feel of classic Windows operating systems (Windows 98, XP, 7 and 10). Create retro-styled web applications with authentic Windows UI components.

> 🪶 **Lightweight** the NuGet package is only ~300 KB.

**Compatible with Blazor Server and Blazor WebAssembly**

DEMO and DOCS : https://tossnet.github.io/Blazor-WinOld/

![thumbnail](https://github.com/user-attachments/assets/7d91aebf-b98e-4f4f-b2e8-7f8286fcd4d4)


## Overview

BlazorWinOld provides a collection of Blazor components styled to match the iconic Windows interfaces from the late 90s and 2000s. Whether you're building a nostalgia-driven project or need that classic Windows aesthetic, this library delivers pixel-perfect components including buttons, message boxes, windows, tabs, and more.

> 💡 **Already using a UI framework?** No problem! BlazorWinOld is fully compatible alongside other UI libraries (MudBlazor, Radzen, Fluent UI…). You can adopt just the components you need, such as `WinOldContextMenu` or `WinOldMessageBox`, without any conflict.



https://github.com/user-attachments/assets/f5a8b771-8f57-4c40-8e1e-6ae8b29a05d7



## Installation

```
Install-Package BlazorWinOld
```
or
```
dotnet add package BlazorWinOld
```
For client-side and server-side Blazor - add script section (head section)

```html
 <link href="_content/BlazorWinOld/css/blazorwinold.css" rel="stylesheet" />
```

In Program.cs add this line
```csharp
builder.Services.AddWinOldComponents();
```

in _Imports.razor :
```csharp
@using Blazor.WinOld
@using Blazor.WinOld.Components
```

and in the bottom of your MainLayout.razor add these lines (optional, only if you use MessageBox or InputBox)
```html
<WinOldMessageBoxHost />  <!-- Only if you use MessageBox -->
<WinOldInputBoxHost />    <!-- Only if you use InputBox -->
<WinOldDialogHost /> <!-- Only if you use DialogBox -->
```


## <a name="ReleaseNotes"></a>Release Notes
<details open="open"><summary>Version 2.6.7</summary>

>- Add TouchMode support to WinOldTabs, increasing tab height for easier touch interaction.
>- Fix pixelated "MS Sans Serif" font rendering in WinXP/Win98 touch mode for WinOldTextBox by using the system font instead.
</details>
 <details><summary>Version 2.6.6</summary>

>- Add WinOldTextBox.SelectAllAsync() to focus and select the entire content of a text box.
</details>

<details><summary>Version 2.6.5</summary>

>- Fix message box text rendering: preserve \n line breaks in MessageBoxOptions.Message by adding white-space: pre-line to the message content styles.
</details>

<details><summary>Version 2.6.4</summary>

>- Add ChildContent support to `WinOldOptionButton`, allowing custom markup (not just plain text) as the option's label.
</details>

 <details><summary>Version 2.6.3</summary>

>- improve touch for `WinOldSelect` (winXP and win98) and checkbox UI
</details>

<details><summary>Version 2.6.2</summary>

>- Add Win10 label style, improve touch and checkbox UI
</details>

<details><summary>Version 2.6.1</summary>

>- Touch mode font sizes are now scaled up to 13px for Win7/Win10 themes; Win98/WinXP themes retain their native 11px pixel-font size with antialiasing disabled to preserve crisp rendering.
>- On the button demo page: Add a custom style example.
</details>

<details><summary>Version 2.6.0</summary>

>- Added `TouchMode` boolean property to `WinOldButton`, `WinOldCheckBox`, `WinOldOptionButton`, `WinOldTextBox`, `WinOldNumberBox` and `WinOldSelect` — enlarges controls for easier finger interaction on touch screens. Supports per-component usage or cascade via `<CascadingValue Name="TouchMode" Value="true">`.
</details>

<details><summary>Version 2.5.6</summary>

>- Context menu now automatically repositions itself to stay within the browser viewport when it would overflow the right or bottom edge
</details>

<details><summary>Version 2.5.5</summary>

>- add css shadow to the win10 submenu
</details>

<details><summary>Version 2.5.4</summary>

>- Multiple context menus on the same page now automatically close each other when a new one is opened.
</details>

<details><summary>Version 2.5.3</summary>

>- Added ShowContextMenu(double clientX, double clientY) overload to WinOldMenu, allowing the context menu to be opened programmatically from code-behind without a MouseEventArgs.
</details>

<details><summary>Version 2.5.2</summary>

>- Dialog: Fixed dialog overflow on small screens. The dialog width is now capped at the viewport width.
</details>

<details><summary>Version 2.5.1</summary>

>- Added generic ShowDialog<TComponent>(options, parameters) extension method to render any Blazor component as dialog content without requiring a RenderFragment.'
</details>


<details open="open"><summary>🚧 Upcoming (not yet released)</summary>

>- `WinOldButton`: preview of a new **DOS** appearance style — retro terminal-inspired button with IBM VGA font and classic shadow effect
</details>

<details><summary>Version 2.5.0</summary>

>- New component `WinOldSlider`
</details>

<details><summary>Version 2.4.0</summary>

>- `WinOldMenuItem`: new `IconCssClass` and `IconTemplate` parameters. Display icons from any external library (Bootstrap Icons, Font Awesome, custom SVG…) without bundling any icon dependency in the package
</details>

<details><summary>Version 2.3.0</summary>

>- The `WinOldMenu` component can be used as a Context Menu by setting the `IsContextMenu` property to `true` 
</details>

<details><summary>Version 2.2.1</summary>

>- Accessibility: added ARIA attributes (`role`, `aria-expanded`, `aria-selected`, `aria-modal`, `aria-labelledby`…) to all components
</details>

<details><summary>Version 2.2.0</summary>

>- Added new `WinOldMenu` 
>- Button : winXP and win7 , add default style
</details>

<details><summary>Version 2.1.0</summary>

>- Added new `WinOldNumberBox` component for numeric input with support for generic types
>- Enhanced `WinOldInputBox` with support for numeric types: `int`, `decimal`, and `double`
>- New extension methods: `ShowInputBox<int?>()`, `ShowInputBox<decimal?>()`, and `ShowInputBox<double?>()`
</details>

<details><summary>Version 2.0.0</summary>

>- Added new Windows 10 style theme
>- Windows 10 is now the default style applied to all components
</details>

<details><summary>Version 1.5.0</summary>

>- Added WinOldDialog component: a fully draggable dialog window supporting custom child content
</details>

<details><summary>Version 1.4.0</summary>

>- Added drag-and-drop support for MessageBox and InputBox, allowing users to move the window freely across the screen.
</details>

<details><summary>Version 1.3.1</summary>

>- Improve disabled style for some components (Frame, Options, Tab)
>- WinOldTabs : a Tab can be disabled.
</details>

<details><summary>Version 1.3.0 ⚠️ BREAKING CHANGE</summary>

>- DialogOptions has been renamed to MessageBoxOptions for WinOldMessageBox
>- Migration: Replace 'new DialogOptions' with 'new MessageBoxOptions' (simple Find/Replace)
>- New WinOldInputBox component
>- Button : Add "Default" Property style for win98 button
</details>

<details><summary>Version 1.2.9</summary>

>- Fix height of Tabs
>- Fix disabled style of WinOldButton
</details>

<details><summary>Version 1.2.8</summary>

>- win7 : separation of font size from font family
</details>

<details><summary>Version 1.2.7</summary>

>- Improved checkbox component rendering
>- Fixed button and selectbox label colors on Safari
</details>

<details><summary>Version 1.2.6</summary>

>- Added `disabled` attribute support on `WinOldSelect` component
>- Added Select page in the demo site
</details>

<details><summary>Version 1.2.2</summary>

>- Added .AddWinOldComponents() to simplify declaration
</details>

## Fonts

This project uses the **Ultimate Oldschool PC Font Pack** by VileR (int10h.org).  
Font used: *WebPlus IBM VGA 8x14*

- Source: https://int10h.org/oldschool-pc-fonts/
- License: [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/)
- © 2015–2020 VileR
- 
## Thanks

I used these repo for most of the css and icons:
- https://github.com/botoxparty/XP.css
- https://github.com/khang-nd/7.css
- https://win98icons.alexmeub.com/
- https://github.com/softwarehistorysociety/XPIcons/tree/main
- https://kristopolous.github.io/BOOTSTRA.386/v2.3.1/components.html

OS emulator :
- https://oses.ioblako.com/new.html
