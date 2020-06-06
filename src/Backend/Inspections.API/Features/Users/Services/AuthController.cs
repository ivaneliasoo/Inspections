using Inspections.API.Features.Users.Services;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly InspectionsContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(InspectionsContext context, ITokenService tokenService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        /// <summary>
        /// Generates a JWT token for use in api call
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public IActionResult CreateToken(string username, string password)
        {
            var user = _context.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();

            if (user == null)
            {
                return BadRequest();
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(token);
        }
    }
}
