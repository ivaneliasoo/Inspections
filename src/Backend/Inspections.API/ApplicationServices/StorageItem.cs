namespace Inspections.API.ApplicationServices
{
    public class StorageItem
    {
        public string Path { get; init; } = default!;
        public string? CloudId { get; init; }
    }
}
