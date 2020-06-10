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
            return await _context.Users.Select(x=> new UserDTO(x)).ToListAsync().ConfigureAwait(false);
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

        // PUT: api/Users/5
        [HttpPut("{userName}")]
        public async Task<IActionResult> PutUser(string userName, UserDTO user)
        {
            if (userName != user.UserName || string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest();
            }

            var editedUser = new User()
            {
                UserName = user.UserName,
                Name = user.Name,
                LastName = user.LastName
            };

            _context.Entry(editedUser).State = EntityState.Modified;

            try
            {
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
        public async Task<ActionResult<User>> PostUser(UserDTO user)
        {
            var newUser = new User()
            {
                UserName = user.UserName,
                Name = user.Name,
                LastName = user.LastName
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
        public async Task<ActionResult<User>> DeleteUser(string userName)
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

            var user = _context.Users.Where(u => u.UserName == userName && u.Password == passwordDTO.CurrentPassword).FirstOrDefault();

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
