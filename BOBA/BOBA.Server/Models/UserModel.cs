namespace BOBA.Server.Models;

public class UserModel
{
    public string Email { get; set; }
    public string Password { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string[] TeamIds { get; set; } = Array.Empty<string>();
}

