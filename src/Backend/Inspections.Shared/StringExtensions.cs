namespace Inspections.Shared;

public static class StringExtensions
{
    public static bool IsPresent(this string @string)
    {
        return !string.IsNullOrWhiteSpace(@string);
    }
}
