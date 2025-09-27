using BOBA.Server.Data.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data.implementation;

public class FormDocument
{
    [Key]
    public string Id { get; set; }

    [Required]
    [ForeignKey("Task")]
    public string TaskId { get; set; }
    public Task Task { get; set; }

    [Required]
    [ForeignKey("DocType")]
    public string DocTypeId { get; set; }
    public TaskDocType DocType { get; set; }

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
