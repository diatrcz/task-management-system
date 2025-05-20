using BOBA.Server.Models.Dto;
using BOBA.Server.Data;

namespace BOBA.Server.Services.Interfaces;

public interface IUserService
{
    Task<string?> GetUserName(string userId);
    Task<string?> GetUserNameById(string userId);
    Task<Role?> GetUserType(string userId);
    Task<UserDto?> GetUserInfo(string userId);
}
