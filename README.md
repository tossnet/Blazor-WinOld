# Blazor.WinOld

[![NuGet](https://img.shields.io/nuget/v/BlazorWinOld.svg)](https://www.nuget.org/packages/BlazorWinOld/)  ![BlazorWinOld Nuget Package](https://img.shields.io/nuget/dt/BlazorWinOld)
[![GitHub](https://img.shields.io/github/license/tossnet/Blazor-WinOld?color=594ae2&logo=github&style=flat-square)](https://github.com/tossnet/Blazor-WinOld/blob/master/LICENSE.txt)

A Blazor UI component library that brings back the nostalgic look and feel of classic Windows operating systems (DOS, Windows 3.1, 98, XP, 7 and 10). Create retro-styled web applications with authentic Windows UI components.

> 🪶 **Lightweight** the NuGet package is only ~300 KB.

**Compatible with Blazor Server and Blazor WebAssembly**

DEMO and DOCS : https://tossnet.github.io/Blazor-WinOld/

<img width="1280" height="640" alt="Blazor WinOld UI component library" src="https://github.com/user-attachments/assets/7f29e88a-c368-4413-9fc9-52f94c53a5cd" />

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

### CSS utilities

`blazorwinold.css` also includes a set of layout utility classes (flexbox, grid, gap, responsive `sm:` / `md:` / `lg:` / `xl:` variants), all prefixed with `wo-` to avoid conflicts with other CSS frameworks. Nothing to declare:

```html
<div class="wo-col sm:wo-row wo-gap-16">
    <div>Column 1</div>
    <div class="wo-col wo-gap-8">Column 2</div>
</div>
```

See the **CSS Utilities** page of the demo for the full list.

### Application layout

`WinOldAppShell` lays out a classic application in your own `MainLayout`: menu, toolbar, side pane, content and status bar. Only the side pane and the content scroll. Its `Appearance` is inherited by the `WinOldMenu`, `WinOldToolbar`, `WinOldNavPane` and `WinOldStatusBar` placed inside, and it already renders the MessageBox / InputBox / Dialog hosts (`IncludeHosts="false"` if you declare them yourself):

```razor
@inherits LayoutComponentBase

<WinOldAppShell Appearance="Appearance.Win98">
    <Menu><WinOldMenu>...</WinOldMenu></Menu>
    <Toolbar><WinOldToolbar>...</WinOldToolbar></Toolbar>
    <SidePane><WinOldNavPane>...</WinOldNavPane></SidePane>
    <StatusBar><WinOldStatusBar>...</WinOldStatusBar></StatusBar>
    <ChildContent>@Body</ChildContent>
</WinOldAppShell>
```

A page can add its own toolbar or status bar with `<SectionContent SectionName="@WinOldAppShell.ToolbarSection">` (or `StatusBarSection`). See the **AppShell** page of the demo.

### Scrollbars

Each appearance has its own scrollbar style: `scroll-win-dos`, `scroll-win-31`, `scroll-win-98`, `scroll-win-xp`, `scroll-win-7`, `scroll-win-10`. Add the class to any scrollable element, or get it with `Appearance.ToScrollbarClass()`:

```html
<div class="@Appearance.Win98.ToScrollbarClass()" style="height: 100px; overflow: auto;">...</div>
```

For the whole page, put the class on `<html>` in `index.html`, or change it at runtime with `await JS.SetScrollbarAppearanceAsync(Appearance.Win7);` (`IJSRuntime`). Firefox only gets the colors. See the **Scrollbar** page of the demo.


## <a name="ReleaseNotes"></a>Release Notes

📜 **[Full changelog](https://github.com/tossnet/Blazor-WinOld/blob/master/CHANGELOG.md)**

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
- https://www.windows93.net
- https://brave-plant-0409a8603.2.azurestaticapps.net/ 
- https://github.com/Futur3Sn0w/web31
