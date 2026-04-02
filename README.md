# Blazor.WinOld

[![NuGet](https://img.shields.io/nuget/v/BlazorWinOld.svg)](https://www.nuget.org/packages/BlazorWinOld/)  ![BlazorWinOld Nuget Package](https://img.shields.io/nuget/dt/BlazorWinOld)
[![GitHub](https://img.shields.io/github/license/tossnet/Blazor-WinOld?color=594ae2&logo=github&style=flat-square)](https://github.com/tossnet/Blazor-WinOld/blob/master/LICENSE.txt)

A Blazor UI component library that brings back the nostalgic look and feel of classic Windows operating systems (Windows 98, XP, 7 and 10). Create retro-styled web applications with authentic Windows UI components.

**Compatible with Blazor Server and Blazor WebAssembly**

DEMO and DOCS : https://tossnet.github.io/Blazor-WinOld/

## Overview

BlazorWinOld provides a collection of Blazor components styled to match the iconic Windows interfaces from the late 90s and 2000s. Whether you're building a nostalgia-driven project or need that classic Windows aesthetic, this library delivers pixel-perfect components including buttons, message boxes, windows, tabs, and more.



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
and in the bottom of your MainLayout.razor add these lines (optional, only if you use MessageBox or InputBox)
```html
<WinOldMessageBoxHost />  <!-- Only if you use MessageBox -->
<WinOldInputBoxHost />    <!-- Only if you use InputBox -->
<WinOldDialogHost /> <!-- Only if you use DialogBox -->
```


## <a name="ReleaseNotes"></a>Release Notes


<details open="open"><summary>Version 1.4.0</summary>

>- Added WinOldDialog component: a fully draggable dialog window supporting custom child content
</details>

<details><summary>Version 1.4.0</summary>

>- Added drag-and-drop support for MessageBox and InputBox, allowing users to move the window freely across the screen.
</details>

<details><summary>Version 1.3.1</summary>

>- Improve disabled style for some components (Frame, Options, Tab)
>- WinOldTabs : a Tab can be disabled.
</details>

<details open="open"><summary>Version 1.3.0</summary>

>- ⚠️ BREAKING CHANGE:
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

## Thanks

I used these repo for most of the css and icons:
- https://github.com/botoxparty/XP.css
- https://github.com/khang-nd/7.css
- https://win98icons.alexmeub.com/
- https://github.com/softwarehistorysociety/XPIcons/tree/main
