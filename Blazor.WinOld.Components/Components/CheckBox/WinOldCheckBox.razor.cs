﻿using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldCheckBox : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    public bool Checked { get; set; } 

    /// </summary>
    [Parameter]
    public EventCallback<bool> CheckedChanged { get; set; }

    /// </summary>
    private Guid ElementId { get; set; } = Guid.NewGuid();

    /// </summary>
    private async Task CheckboxChanged(ChangeEventArgs e)
    {
        if (e.Value == null)
        {
            return;
        }

        // get the checkbox state
        Checked = (bool)e.Value;

        if (CheckedChanged.HasDelegate)
        {
            await CheckedChanged.InvokeAsync(Checked);
        }
    }

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win7 => "chk-win-7",
            Appearance.WinXP => "chk-win-xp",
            Appearance.Win98 => "chk-win-98",
            _ => "chk-win-98"
        };
    }
}
