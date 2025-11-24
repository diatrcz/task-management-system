namespace BOBA.Server.Models;

public class RegisterResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
