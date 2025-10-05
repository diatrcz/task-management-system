namespace BOBA.Server.Models.Dto
{
    public class FormFieldDto
    {
        public string Id { get; set; }
        public string ModelId { get; set; }
        public string TaskId { get; set; }
        public string Value { get; set; }
        public string? ModifierId { get; set; }
    }
}
