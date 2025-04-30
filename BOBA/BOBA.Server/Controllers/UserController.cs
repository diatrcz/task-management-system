using BOBA.Server.Data;
using BOBA.Server.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace BOBA.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    public UserController(ApplicationDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    /*[HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                // Address = model.Address,
                Type = UserType.LoggedInUser
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { Message = "Registration successful!!!!" });
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(result.Errors);
        }

        return BadRequest(ModelState);
    }*/

    [HttpGet("is-loggedin")]
    public IActionResult IsLoggedIn()
    {
        return Ok(true);
    }

    [HttpGet("name")]
    public async Task<IActionResult> GetUserName()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userName = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.FirstName)
            .FirstAsync();

        if (userName == null)
        {
            return NotFound();
        }

        return Ok(new { userName });
    }

    [HttpGet("name-by-id")]
    public async Task<IActionResult> GetUserNameByID([FromQuery] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return Ok(user?.UserName);
    }

    [HttpGet("type")]
    public async Task<IActionResult> GetUserType()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userType = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Type)
            .FirstAsync();

        return Ok(new { userType });
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _context.Users.FirstAsync(u => u.Id == userId);

        var userInfo = new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Type.ToString(),
            Teams = user.Teams.Select(team => team.Id).ToList()
        };

        return Ok(new { user = userInfo });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok();
    }
}
