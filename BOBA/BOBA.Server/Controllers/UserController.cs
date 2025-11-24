using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Data.model;
using BOBA.Server.Models;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace BOBA.Server.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    public UserController(ApplicationDbContext context, SignInManager<User> signInManager, UserManager<User> userManager, IUserService userService)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] UserModel model)
    {

        var result = await _userService.Register(model);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }



    [HttpGet("is-loggedin")]
    public IActionResult IsLoggedIn()
    {
        return Ok(true);
    }

    [HttpGet("name")]
    public async Task<ActionResult<string>> GetUserName()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userName = _userService.GetUserName(userId);

        if (userName == null)
        {
            return NotFound();
        }

        return Ok(new { userName });
    }

    [HttpGet("name-by-id")]
    public async Task<ActionResult<string>> GetUserNameByID([FromQuery] string userId)
    {
        var userName = _userService.GetUserNameById(userId);

        return Ok(new { userName });
    }

    [HttpGet("type")]
    public async Task<ActionResult<string>> GetUserType()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userType = _userService.GetUserType(userId);

        return Ok(new { userType });
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userInfo = _userService.GetUserInfo(userId);

        return Ok(new { user = userInfo });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok();
    }

    [HttpGet("teams/userid")]
    public async Task<ActionResult<List<TeamSummaryDto>>> GetUserTeamsById()
    {
        var user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var teams = await _userService.GetTeamsByUserId(user_id);

        return Ok(teams);
    }

    [HttpGet("teams")]
    [AllowAnonymous]
    public async Task<ActionResult<List<TeamSummaryDto>>> GetTeams() 
    {
        var teams = await _userService.GetTeams();

        return Ok(teams);
    }

}
