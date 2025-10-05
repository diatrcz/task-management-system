namespace BOBA.Server.Models;

public class Field
{
    public string FieldId { get; set; }
    public bool Required { get; set; }
    public bool Disabled { get; set; }
    public int? Rows { get; set; }
    public StyleClasses StyleClasses { get; set; }
}
