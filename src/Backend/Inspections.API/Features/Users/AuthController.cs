using Inspections.API.Features.Users.Services;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Inspections.API.Features.Users
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("token", Name ="Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult CreateToken([FromBody] LoginModel model)
        {
            var user = _context.Users.Where(u => u.UserName == model.username && u.Password == model.password).FirstOrDefault();

            if (user == null)
            {
                return BadRequest();
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(token);
        }
    }

    public class LoginModel
    {
        public string username { get; set; } = default!;
        public string password { get; set; } = default!;
    }

}
