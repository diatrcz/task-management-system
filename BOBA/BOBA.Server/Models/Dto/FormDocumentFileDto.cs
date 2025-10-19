namespace BOBA.Server.Models.Dto;

public class FormDocumentFileDto
{
    public string? Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Data { get; set; }
}
