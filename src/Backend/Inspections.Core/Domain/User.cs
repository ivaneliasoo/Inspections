namespace Inspections.Core.Domain;

public class User
{
    public string UserName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int? LastEditedReport { get; set; }
    public bool IsAdmin { get; set; }
}
