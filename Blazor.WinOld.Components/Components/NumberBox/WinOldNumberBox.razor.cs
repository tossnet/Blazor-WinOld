using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldNumberBox<TValue> : WinOldComponentBase
{
    private ElementReference InputElement { get; set; }

    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public TValue? Value { get; set; }

    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    [Parameter]
    public string? InputStyle { get; set; }

    [Parameter]
    public string? Min { get; set; }

    [Parameter]
    public string? Max { get; set; }

    [Parameter]
    public string? Step { get; set; }

    private Guid ElementId { get; set; } = Guid.NewGuid();

    protected TValue? CurrentValue
    {
        get => Value;
        set
        {
            if (!EqualityComparer<TValue?>.Default.Equals(value, Value))
            {
                Value = value;
                _ = ValueChanged.InvokeAsync(value);
            }
        }
    }

    public async Task FocusAsync()
    {
        await InputElement.FocusAsync();
    }

    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win7 => "txt-win-7",
            Appearance.WinXP => "txt-win-xp",
            Appearance.Win98 => "txt-win-98",
            Appearance.Win10 => "txt-win-10",
            _ => "txt-win-10"
        };
    }

    private string GetComponentLabelClass()
    {
        if (string.IsNullOrEmpty(Label))
            return "label-txt-hide";

        return Appearance switch
        {
            Appearance.Win7 => "lbtxt-win-7",
            Appearance.WinXP => "lbtxt-win-xp",
            Appearance.Win98 => "lbtxt-win-98",
            Appearance.Win10 => "lbtxt-win-10",
            _ => "lbtxt-win-10"
        };
    }
}
