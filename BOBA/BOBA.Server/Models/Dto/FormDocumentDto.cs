namespace BOBA.Server.Models.Dto;

public class FormDocumentDto
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string DocTypeId { get; set; }
    public string FileName { get; set; }
    public DateTime UploadedAt { get; set; }
    public string UploaderName { get; set; }
}
