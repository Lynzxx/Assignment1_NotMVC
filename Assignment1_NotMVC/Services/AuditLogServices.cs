using Assignment1_NotMVC.Models;

namespace Assignment1_NotMVC.Services
{
    public class AuditLogServices
    {
        private readonly AuthDbContext _context;
        public AuditLogServices(AuthDbContext context)
        {
            _context = context;
        }
        public List<Audit> GetAll()
        {
            return _context.Audits.OrderBy(d => d.AuditId).ToList();
        }

        public void AddAudit(Audit audit)
        {
            _context.Audits.Add(audit);
            _context.SaveChanges();
        }

        public void DeleteAudit(Audit audit)
        {
            _context.Audits.Remove(audit);
            _context.SaveChanges();
        }
    }
}
