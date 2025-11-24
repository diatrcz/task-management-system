using BOBA.Server.Data.model;
using BOBA.Server.Models;
using BOBA.Server.Models.Dto;

namespace BOBA.Server.Services.Interfaces;

public interface IUserService
{
    Task<RegisterResponse> Register(UserModel model);
    Task<string?> GetUserName(string userId);
    Task<string?> GetUserNameById(string userId);
    Task<Role?> GetUserType(string userId);
    Task<UserDto?> GetUserInfo(string userId);
    Task<List<TeamSummaryDto>> GetTeamsByUserId(string userId);
    Task<List<TeamSummaryDto>> GetTeams();
}
