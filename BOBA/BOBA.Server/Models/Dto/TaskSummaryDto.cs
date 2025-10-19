namespace BOBA.Server.Models.Dto
{
    public class TaskSummaryDto
    {
        public string Id { get; set; }
        public string TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }
        public string CreatorId { get; set; }
        public string CurrentStateId { get; set; }
        public string CurrentStateName { get; set; }
        public bool CurrentStateIsFinal { get; set; }
        public string Assignee { get; set; }
        public string TeamId { get; set; }
        public string Team { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedAt { get; set; }
    }
}
