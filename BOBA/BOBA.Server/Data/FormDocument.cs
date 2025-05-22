using System.ComponentModel.DataAnnotations;

namespace BOBA.Server.Data;

public class FormDocument
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string TaskId { get; set; }

    [Required]
    public string FileName { get; set; } = null!;

    [Required]
    public string ContentType { get; set; } = null!;

    [Required]
    public byte[] Data { get; set; } = null!;

    [Required]
    public DateTime UploadeddAt { get; set; }

    [Required]
    public string UploaderId { get; set; }
}
