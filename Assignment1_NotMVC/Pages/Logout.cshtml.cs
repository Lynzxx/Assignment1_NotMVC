using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUser currentUser;
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit=new Audit();
        public LogoutModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuditLogServices auditLogServices)
        {
            this.signInManager = signInManager;
            this._userManager = userManager;
            this._auditLogServices = auditLogServices;

        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = await _userManager.FindByIdAsync(userid);

            //create audit
            var id = Guid.NewGuid().ToString();
            userAudit.AuditId = id;
            userAudit.UserId = userid;
            userAudit.Action = "Logout";
            userAudit.TimeStamp = DateTime.Now;
            userAudit.AreaAccessed = "";
            userAudit.UserEmail = user.Email;
            _auditLogServices.AddAudit(userAudit);

            HttpContext.Session.Clear();
            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }
        public IActionResult OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
