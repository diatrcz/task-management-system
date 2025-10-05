namespace BOBA.Server.Models.Dto;

public class TaskFieldDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Label { get; set; }
    public string? Placeholder { get; set; }
    public string? Validation { get; set; }
    public string? ValidationErrorMessage { get; set; }
    public string? Options { get; set; }
}
