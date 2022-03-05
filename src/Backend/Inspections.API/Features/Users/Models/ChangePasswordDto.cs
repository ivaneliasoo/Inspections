namespace Inspections.API.Features.Users.Models;

public class ChangePasswordDto
{
    public string UserName { get; set; } = default!;
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string NewPasswordConfirmation { get; set; } = default!;
}
