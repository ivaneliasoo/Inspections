using Ardalis.GuardClauses;
using Inspections.API.Features.Users.Models;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Users;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly InspectionsContext _context;

    public UsersController(InspectionsContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return await _context.Users.Select(x => new UserDto(x)).ToListAsync().ConfigureAwait(false);
    }

    // GET: api/Users/username
    [HttpGet("{userName}", Name = "GetUserByUserName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<UserDto>> GetUserByUserName(string userName)
    {
        var user = await _context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (user == null)
        {
            return NotFound();
        }

        return new UserDto(user);
    }

    // GET: api/Users/username
    [HttpGet("active", Name = "GetActiveUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<UserDto>> GetActiveUser()
    {
        var userName = HttpContext.User.Identity?.Name;

        if (userName is null)
            return BadRequest();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName)
            .ConfigureAwait(false);

        if (user == null)
        {
            return NotFound();
        }

        return new UserDto(user);
    }

    // PUT: api/Users/demo
    [HttpPut("{userName}", Name = "UpdateUser")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> PutUser(string userName, [FromBody] UserDto user)
    {
        Guard.Against.Null(user, nameof(user));
        if (userName != user.UserName || string.IsNullOrWhiteSpace(userName))
        {
            return BadRequest();
        }

        try
        {
            var editedUser = await _context.Set<User>().Where(u => u.UserName == userName).SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (editedUser == null) return NotFound("user not found");
            //editedUser.UserName = user.UserName;
            editedUser.Name = user.Name;
            editedUser.LastName = user.LastName;
            editedUser.IsAdmin = user.IsAdmin;
            editedUser.LastEditedReport = user.LastEditedReport;
            _context.Entry(editedUser).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(userName))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Users
    [HttpPost(Name = "AddUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<User>> PostUser([FromBody] UserDto user)
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

        return CreatedAtAction("PostUser", new {id = user.UserName}, user);
    }

    // DELETE: api/Users/5
    [HttpDelete("{userName}", Name = "DeleteUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<User>> DeleteUser(string? userName)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return user;
    }

    /// <summary>
    /// Change User Password after validated
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="passwordDto"></param>
    /// <returns></returns>
    [HttpPatch("{userName}", Name = "ChangePassword")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> ChangePassword(string userName, ChangePasswordDto passwordDto)
    {
        Guard.Against.Null(passwordDto, nameof(passwordDto));

        if (userName != passwordDto.UserName)
        {
            return BadRequest();
        }

        // TODO: && u.Password == passwordDTO.CurrentPassword
        var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

        if (user == null || passwordDto.NewPassword != passwordDto.NewPasswordConfirmation)
        {
            return BadRequest();
        }

        user.Password = passwordDto.NewPasswordConfirmation;
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return NoContent();
    }

    private bool UserExists(string userName)
    {
        return _context.Users.Any(e => e.UserName == userName);
    }
}
