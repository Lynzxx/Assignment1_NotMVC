using System.ComponentModel.DataAnnotations;

namespace Assignment1_NotMVC.Models
{
    public class Audit
    {
        [Key]
        public string AuditId { get; set; }
        public string Action { get; set; }
        public string AreaAccessed { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
    }
}
