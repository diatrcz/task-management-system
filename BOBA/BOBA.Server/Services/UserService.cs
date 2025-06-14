﻿using BOBA.Server.Data;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BOBA.Server.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserService(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<string?> GetUserName(string userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.FirstName)
            .FirstAsync();
    }

    public async Task<string?> GetUserNameById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.UserName;
    }

    public async Task<Role?> GetUserType(string userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Type)
            .FirstAsync();
    }

    public async Task<UserDto?> GetUserInfo(string userId)
    {
        var user = await _context.Users
            .Include(u => u.Teams)
            .FirstAsync(u => u.Id == userId);


        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Type.ToString(),
            Teams = user.Teams.Select(t => t.Id).ToList()
        };
    }
}
