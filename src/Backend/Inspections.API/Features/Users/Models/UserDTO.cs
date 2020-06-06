using Ardalis.GuardClauses;
using Inspections.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Users.Models
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public UserDTO()
        {

        }
        public UserDTO(User user)
        {
            Guard.Against.Null(user, nameof(user));

            UserName = user.UserName;
            Name = user.Name;
            LastName = user.LastName;
        }
    }
}
