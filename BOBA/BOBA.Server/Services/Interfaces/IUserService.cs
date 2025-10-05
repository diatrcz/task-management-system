using BOBA.Server.Models.Dto;
using BOBA.Server.Data.model;

namespace BOBA.Server.Services.Interfaces;

public interface IUserService
{
    Task<string?> GetUserName(string userId);
    Task<string?> GetUserNameById(string userId);
    Task<Role?> GetUserType(string userId);
    Task<UserDto?> GetUserInfo(string userId);
    Task<List<TeamSummaryDto>> GetTeamsByUserId(string userId);
    Task<List<TeamSummaryDto>> GetTeams();
}
