﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Users.Models
{
    public class ChangePasswordDTO
    {
        public string UserName { get; set; } = default!;
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string NewPasswordConfirmation { get; set; } = default!;
    }
}
