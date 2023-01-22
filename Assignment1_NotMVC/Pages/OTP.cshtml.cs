using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Assignment1_NotMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages
{
    public class OTPModel : PageModel
    {

        [BindProperty]
        public OTPform oTPform { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit = new Audit();

        //testing
        public string userOTP { get; set; }
        public string Email { get; set; }

        public OTPModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuditLogServices auditLogServices)
        {
            this.signInManager = signInManager;
            this._userManager = userManager;
            this._auditLogServices = auditLogServices;
        }

        public async Task OnGet(string email)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            if(user == null)
            {
                 user = await _userManager.FindByEmailAsync(email);
            }
            userOTP=user.OTP;
            Email = email;
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
                //OTP is valid for 10 mins
                if(DateTime.Now < user.LastOTPGeneratedDate?.AddMinutes(10))
                {
                    if (oTPform.OTP == user.OTP)
                    {
                        TempData["FlashMessage.Type"] = "success";
                        TempData["FlashMessage.Text"] = string.Format("Successful Login");
                        return Redirect("/changeSecurityStamp");
                    }
                    else
                    {
                        TempData["FlashMessage.Type"] = "danger";
                        TempData["FlashMessage.Text"] = string.Format("OTP is invalid.");
                    }
             
                }
                else
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("OTP has expired. Please click resend OTP.");
                }
                
            }
            return RedirectToPage("/OTP", new {email= Email });
            

        }
    }
}
