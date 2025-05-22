namespace BOBA.Server.Models;

public class Field
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Label { get; set; }
    public string Placeholder { get; set; }
    public bool Required { get; set; }
    public bool Disabled { get; set; }
    public int? Rows { get; set; }
    public StyleClasses StyleClasses { get; set; }
}
