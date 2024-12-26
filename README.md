# Blazor.WinOld

[![NuGet](https://img.shields.io/nuget/v/BlazorWinOld.svg)](https://www.nuget.org/packages/BlazorWinOld/)  ![BlazorCalendar Nuget Package](https://img.shields.io/nuget/dt/BlazorWinOld)
[![GitHub](https://img.shields.io/github/license/tossnet/Blazor-WinOld?color=594ae2&logo=github&style=flat-square)](https://github.com/tossnet/Blazor-WinOld/blob/master/LICENSE.txt)

For Blazor Server or Blazor WebAssembly

DEMO : https://tossnet.github.io/Blazor-WinOld/

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
builder.Services.AddSingleton<IDialogService, DialogService>();
```
and in the bottom of your MainLayout.razor add this line to use taskbar
```html
<WinOldMessageBoxHost />
```

## Thanks

I used these repo for most of the css and icons:
- https://github.com/botoxparty/XP.css
- https://github.com/khang-nd/7.css
- https://win98icons.alexmeub.com/
- https://github.com/softwarehistorysociety/XPIcons/tree/main
