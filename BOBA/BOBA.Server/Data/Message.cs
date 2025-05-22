using System.ComponentModel.DataAnnotations;

namespace BOBA.Server.Data;

public class Message
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string MessageString { get; set; }

    [Required]
    public string TaskId { get; set; }

    [Required]
    public string SenderId { get; set; }

}
