# Blazor.WinOld

[![NuGet](https://img.shields.io/nuget/v/BlazorWinOld.svg)](https://www.nuget.org/packages/BlazorWinOld/)  ![BlazorWinOld Nuget Package](https://img.shields.io/nuget/dt/BlazorWinOld)
[![GitHub](https://img.shields.io/github/license/tossnet/Blazor-WinOld?color=594ae2&logo=github&style=flat-square)](https://github.com/tossnet/Blazor-WinOld/blob/master/LICENSE.txt)


For Blazor Server or Blazor WebAssembly

DEMO and DOCS : https://tossnet.github.io/Blazor-WinOld/

## Hello, world!
Welcome to my new delirium


https://github.com/user-attachments/assets/157fbd5f-dcab-4e76-b7b2-d240c697082a




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

In Program.cs add this line to use messagebox
```csharp
builder.Services.AddWinOldComponents();
```
and in the bottom of your MainLayout.razor add this line to use dialog
```html
<WinOldMessageBoxHost />
```


## <a name="ReleaseNotes"></a>Release Notes

<details open="open"><summary>Version 1.3.0</summary>

>- ⚠️ BREAKING CHANGE:
- DialogOptions has been renamed to MessageBoxOptions for WinOldMessageBox
- Migration: Replace 'new DialogOptions' with 'new MessageBoxOptions' (simple Find/Replace)
- New WinOldInputBox component
- Button : Add "Default" Property style for win98 button
</details>

<details><summary>Version 1.2.9</summary>

>- Fix height of Tabs
 - Fix disabled style of WinOldButton
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
