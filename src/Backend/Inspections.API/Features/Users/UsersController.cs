using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Inspections.API.Features.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Ardalis.GuardClauses;

namespace Inspections.API.Features.Users
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly InspectionsContext _context;

        public UsersController(InspectionsContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Select(x => new UserDTO(x)).ToListAsync().ConfigureAwait(false);
        }

        // GET: api/Users/username
        [HttpGet("{userName}")]
        public async Task<ActionResult<UserDTO>> GetUserByUserName(string userName)
        {
            var user = await _context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync().ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDTO(user);
        }

        // GET: api/Users/username
        [HttpGet("active")]
        public async Task<ActionResult<UserDTO>> GetActiveUser()
        {
            var userName = HttpContext?.User?.Identity?.Name;

            if (userName is null)
                return BadRequest();

            var user = await _context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync().ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDTO(user);
        }

        // PUT: api/Users/demo
        [HttpPut("{userName}")]
        public async Task<IActionResult> PutUser(string userName, [FromBody] UserDTO user)
        {
            Guard.Against.Null(user, nameof(user));
            if (userName != user.UserName || string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest();
            }

            try
            {
                var editedUser = await _context.Set<User>().Where(u => u.UserName == userName).SingleOrDefaultAsync().ConfigureAwait(false);

                if (editedUser != null)
                {
                    //editedUser.UserName = user.UserName;
                    editedUser.Name = user.Name;
                    editedUser.LastName = user.LastName;
                    editedUser.IsAdmin = user.IsAdmin;
                    editedUser.LastEditedReport = user.LastEditedReport;
                }

                _context.Entry(editedUser).State = EntityState.Modified;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserDTO user)
        {
            Guard.Against.Null(user, nameof(user));
            var newUser = new User()
            {
                UserName = user.UserName,
                Name = user.Name,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                LastEditedReport = user.LastEditedReport,
                Password = string.Empty
            };
            _context.Users.Add(newUser);
            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("PostUser", new { id = user.UserName }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{userName}")]
        public async Task<ActionResult<User>> DeleteUser(string userName = null)
        {
            var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return user;
        }

        /// <summary>
        /// Change User Password afeter validated
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordDTO"></param>
        /// <returns></returns>
        [HttpPatch("{userName}")]
        public async Task<IActionResult> ChangePassword(string userName, ChangePasswordDTO passwordDTO)
        {
            Guard.Against.Null(passwordDTO, nameof(passwordDTO));

            if (userName != passwordDTO.UserName)
            {
                return BadRequest();
            }

            // TODO: && u.Password == passwordDTO.CurrentPassword
            var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            if (user == null || passwordDTO.NewPassword != passwordDTO.NewPasswordConfirmation)
            {
                return BadRequest();
            }
            user.Password = passwordDTO.NewPasswordConfirmation;
            _context.Entry(user).State = EntityState.Modified;

            var result = await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }

        private bool UserExists(string userName)
        {
            return _context.Users.Any(e => e.UserName == userName);
        }
    }
}
