using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages
{
    [Authorize(Roles= "Member")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ApplicationUser currentUser=new ApplicationUser();
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit = new Audit();
        public IndexModel(ILogger<IndexModel> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
             AuditLogServices auditLogServices)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _auditLogServices = auditLogServices;
        }

        public string securityStamp="null";

        public async Task<IActionResult> OnGetAsync()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            securityStamp = HttpContext.Session.GetString("SecurityStamp");
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            currentUser = user;
            if (currentUser.SecurityStamp != securityStamp)
            {
                //create audit
                var errid = Guid.NewGuid().ToString();
                userAudit.AuditId = errid;
                userAudit.UserId = userid;
                userAudit.Action = "Logout";
                userAudit.TimeStamp = DateTime.Now;
                userAudit.AreaAccessed = "Multiple Sessions Detected";
                userAudit.UserEmail = user.Email;
                _auditLogServices.AddAudit(userAudit);

                HttpContext.Session.Clear();
                await _signInManager.SignOutAsync();
                return RedirectToPage("Login");
            }
            //Password Max Age is 8
            if (DateTime.Now > user.LastPasswordChangedDate.AddMinutes(8))
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Password needs to be changed.");
                return Redirect("/ChangePassword");
            }
            return Page();
        }
    }
    
}