using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Assignment1_NotMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages
{
    [Authorize(Roles = "Member")]
    public class ChangePasswordModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit = new Audit();

        [BindProperty]
        public PasswordChange PCModel { get; set; }

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuditLogServices auditLogServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _auditLogServices = auditLogServices;
        }
        public async Task<IActionResult> OnGet()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return Redirect("/Login");
            }
            //password must be minimumly 5 mins old
            if (DateTime.Now < user.LastPasswordChangedDate.AddMinutes(5))
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Password cannot be changed yet.");
                return Redirect("/Index");
            }
            return Page();
        }

            public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userid = _userManager.GetUserId(HttpContext.User);
                ApplicationUser user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                {
                    return Redirect("/Login");
                }
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, PCModel.Password);
                user.LastPasswordChangedDate = DateTime.Now;
                await _userManager.UpdateAsync(user);

                //create audit
                var id = Guid.NewGuid().ToString();
                userAudit.AuditId = id;
                userAudit.UserId = user.Id;
                userAudit.Action = "Changed Password";
                userAudit.TimeStamp = DateTime.Now;
                userAudit.AreaAccessed = "/ChangePassword";
                userAudit.UserEmail = user.Email;
                _auditLogServices.AddAudit(userAudit);

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Password has been changed");

                return Redirect("/Index");
            }
            return Page();
        }
    }
}
