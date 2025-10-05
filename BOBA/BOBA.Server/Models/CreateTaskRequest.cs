using System.ComponentModel.DataAnnotations;

namespace BOBA.Server.Models;

public class CreateTaskRequest
{
    [Required]
    public string TaskTypeId { get; set; }

    [Required] 
    public string TeamId { get; set; }
}
