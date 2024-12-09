namespace Blazor.WinOld.Components;

[AttributeUsage(AttributeTargets.Property)]
public class CategoryAttribute : Attribute
{
    public CategoryAttribute(string category)
    {
        Category = category;
    }
    public string Category { get; }
}

public static class CategoryTypes
{
    public static class Button
    {
        public const string Appearance = "Appearance";
    }
}