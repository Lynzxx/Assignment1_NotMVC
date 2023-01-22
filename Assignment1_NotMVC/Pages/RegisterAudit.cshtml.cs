using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages
{
    public class RegisterAuditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUser currentUser;
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit = new Audit();
        public RegisterAuditModel(UserManager<ApplicationUser> userManager, AuditLogServices auditLogServices)
        {

            this._userManager = userManager;
            this._auditLogServices = auditLogServices;
        }
        public async Task<IActionResult> OnGet()
        {
            //HttpContext.Session.SetString("contextUser", HttpContext.User.ToString());
            var userid = _userManager.GetUserId(HttpContext.User);

            ApplicationUser user = await _userManager.FindByIdAsync(userid);

            //var newSecurityStamp = Guid.NewGuid().ToString("D");
            //currentUser.SecurityStamp = newSecurityStamp;

            //create audit
            var id = Guid.NewGuid().ToString();
            userAudit.AuditId = id;
            userAudit.UserId = userid;
            userAudit.Action = "Register";
            userAudit.TimeStamp = DateTime.Now;
            userAudit.AreaAccessed = "/Index";
            userAudit.UserEmail = user.Email;
            _auditLogServices.AddAudit(userAudit);

            //await _userManager.UpdateSecurityStampAsync(user);
            HttpContext.Session.SetString("SecurityStamp", user.SecurityStamp);

            return RedirectToPage("Index");
        }
    }
}
