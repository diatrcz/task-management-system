namespace BOBA.Server.Models;

public class FormFieldItem
{
    public string Id { get; set; }
    public Layout Layout { get; set; }
    public List<Field> Fields { get; set; }
    public List<FormFieldItem> SubSections { get; set; } // Used for nested sections like "cityStateZip"
}
