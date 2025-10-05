namespace BOBA.Server.Models.Dto;

public class FieldDto
{
    public string FieldId { get; set; }
    public bool Required { get; set; }
    public bool Disabled { get; set; }
    public int? Rows { get; set; }
    public StyleClassesDto StyleClasses { get; set; }
}
