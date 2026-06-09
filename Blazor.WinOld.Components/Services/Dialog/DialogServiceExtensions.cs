using Blazor.WinOld;
using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public static class DialogServiceExtensions
{
    /// <summary>
    /// Shows a dialog with a component of type <typeparamref name="TComponent"/> as its content.
    /// </summary>
    public static Task<bool?> ShowDialog<TComponent>(
        this IDialogService service,
        DialogOptions options,
        Dictionary<string, object>? parameters = null)
        where TComponent : IComponent
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenComponent<TComponent>(0);
            if (parameters is not null)
            {
                foreach (var (key, value) in parameters)
                    builder.AddAttribute(1, key, value);
            }
            builder.CloseComponent();
        };

        return service.ShowDialog(options with { ChildContent = childContent });
    }


    /// <summary>
    /// Shows an input box and returns the entered value of the specified type
    /// </summary>
    /// <typeparam name="T">The type of value to return (string, int, decimal, or double)</typeparam>
    public static async Task<T?> ShowInputBox<T>(
        this IDialogService service,
        InputBoxOptions options)
    {
        // Get underlying type if T is nullable
        var underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        // Determine InputType based on underlying type
        var inputType = underlyingType switch
        {
            Type t when t == typeof(string) => InputBoxType.Text,
            Type t when t == typeof(int) => InputBoxType.Integer,
            Type t when t == typeof(decimal) => InputBoxType.Decimal,
            Type t when t == typeof(double) => InputBoxType.Double,
            _ => throw new ArgumentException($"Type {underlyingType.Name} is not supported. Use string, int, decimal, or double.")
        };

        var result = await service.ShowInputBoxCore(options with { InputType = inputType });

        if (result is null)
            return default;

        return underlyingType switch
        {
            Type t when t == typeof(string) => (T?)(object?)result.ToString(),
            Type t when t == typeof(int) => (T?)(object?)(result as int?),
            Type t when t == typeof(decimal) => (T?)(object?)(result as decimal?),
            Type t when t == typeof(double) => (T?)(object?)(result as double?),
            _ => default
        };
    }

    /// <summary>
    /// Shows an input box for text input and returns the entered string
    /// </summary>
    public static Task<string?> ShowInputBox(
        this IDialogService service,
        InputBoxOptions options)
    {
        return ShowInputBox<string>(service, options);
    }
}
