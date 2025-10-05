namespace BOBA.Server.Models.Dto;

public class TaskFlowSummaryDto
{
    public string Id { get; set; }
    public List<NextStateDto> NextState { get; set; }
    public string? EditRoleId { get; set; }
    public List<string> ReadOnlyRole { get; set; }
    public List<FormJsonDto> FormFields { get; set; }
}
