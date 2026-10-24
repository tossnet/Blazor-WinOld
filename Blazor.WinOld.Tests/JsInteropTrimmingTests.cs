using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Blazor.WinOld.Components;
using Bunit;
using Xunit;
using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Tests;

public class JsInteropTrimmingTests : TestContext
{
    // Anonymous types passed to JS interop break once the app is trimmed (publish):
    // their constructor parameter names are removed and System.Text.Json throws
    // ConstructorContainsNullParameterNames (opening a dialog crashed in v3.1.x).
    [Fact]
    public void Library_DoesNotContainAnonymousTypes()
    {
        var anonymousTypes = typeof(DraggableWindow).Assembly
            .GetTypes()
            .Where(t => t.IsDefined(typeof(CompilerGeneratedAttribute), false) && t.Name.Contains("AnonymousType"))
            .Select(t => t.FullName)
            .ToList();

        Assert.Empty(anonymousTypes);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task InitAsync_SendsSerializableOptionsToInitDraggable(bool withinParent)
    {
        var module = JSInterop.SetupModule("./_content/BlazorWinOld/js/draggable.js");
        module.SetupVoid("trackVisualViewport").SetVoidResult();
        module.SetupModule("initDraggable", _ => true);

        var cut = RenderComponent<DraggableWindow>(p => p.Add(x => x.ChildContent, (RenderFragment)(_ => { })));
        await cut.InvokeAsync(() => cut.Instance.InitAsync(default, default, withinParent));

        var invocation = module.VerifyInvoke("initDraggable");
        var options = invocation.Arguments[2];
        Assert.NotNull(options);
        Assert.False(options!.GetType().IsDefined(typeof(CompilerGeneratedAttribute), false));

        // Same shape as expected by initDraggable(windowEl, titleBarEl, options) in draggable.js
        var json = JsonDocument.Parse(JsonSerializer.Serialize(options)).RootElement;
        Assert.Equal(withinParent, json.GetProperty("withinParent").GetBoolean());
        Assert.Equal(JsonValueKind.Null, json.GetProperty("dotNetRef").ValueKind);
    }
}
