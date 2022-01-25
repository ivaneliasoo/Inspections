using Ardalis.GuardClauses;
using Inspections.Core.Domain;

namespace Inspections.API.Features.Users.Models
{
    public class UserDTO
    {
        public string UserName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int? LastEditedReport { get; set; }
        public bool IsAdmin { get; set; }
        public UserDTO()
        {

        }
        public UserDTO(User user)
        {
            Guard.Against.Null(user, nameof(user));

            UserName = user.UserName;
            Name = user.Name;
            LastName = user.LastName;
            LastEditedReport = user.LastEditedReport;
            IsAdmin = user.IsAdmin;
        }
    }
}
