namespace Inspections.API.ApplicationServices
{
    public class StorageItem
    {
        public string Path { get; set; } = default!;
        public string? CloudId { get; set; }
    }
}
