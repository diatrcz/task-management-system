using Microsoft.Identity.Client;

namespace BOBA.Server.Models.Dto;

public class UserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public List<string> Teams { get; set; }

}
