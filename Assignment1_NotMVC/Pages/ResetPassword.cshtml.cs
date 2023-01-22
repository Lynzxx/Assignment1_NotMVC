using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Assignment1_NotMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages.ForgotPassword
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public ResetPassword RPModel { get; set; }
        public string TOKEN { get; set; }
        public string EMAIL { get; set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit = new Audit();

        public ResetPasswordModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuditLogServices auditLogServices)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _auditLogServices=auditLogServices;
        }
        public IActionResult OnGet(string token,string email)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            else
            {
                TOKEN = token;
                EMAIL = email;
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("token",token);
            }
            
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(RPModel.Email);
                if(user != null)
                {
                    var password1 = user.PastPassword1;
                    var password2 = user.PastPassword2;
      
                    PasswordVerificationResult passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, password1, RPModel.Password);
                    PasswordVerificationResult passwordVerificationResult2 = _userManager.PasswordHasher.VerifyHashedPassword(user, password2, RPModel.Password);

                    if (passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult2 == PasswordVerificationResult.Success)
                    {
                        TempData["FlashMessage.Type"] = "danger";
                        TempData["FlashMessage.Text"] = string.Format("Password is same as your old passwords.");
                    }
                    else
                    {
                        //var result = await _userManager.ResetPasswordAsync(user, RPModel.Token, RPModel.Password);
                        //if (result.Succeeded)
                        //{
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, RPModel.Password);
                        user.LastPasswordChangedDate = DateTime.Now;
                            if (user.PastPassword2 == null)
                            {
                                user.PastPassword2 = user.PasswordHash;
                            }
                            else
                            {
                                user.PastPassword1 = user.PastPassword2;
                                user.PastPassword2 = user.PasswordHash;
                            }
                            await _userManager.UpdateAsync(user);

                            //var result=await _userManager.ResetPasswordAsync(user,)
                            if (await _userManager.IsLockedOutAsync(user))
                            {
                                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                            }

                            //create audit
                            var id = Guid.NewGuid().ToString();
                            userAudit.AuditId = id;
                            userAudit.UserId = user.Id;
                            userAudit.Action = "Reset Password";
                            userAudit.TimeStamp = DateTime.Now;
                            userAudit.AreaAccessed = "";
                            userAudit.UserEmail = user.Email;
                            _auditLogServices.AddAudit(userAudit);

                            HttpContext.Session.Clear();

                            TempData["FlashMessage.Type"] = "success";
                            TempData["FlashMessage.Text"] = string.Format("Password has been reset");

                            return Redirect("/Login");
                        //}
                        //foreach (var err in result.Errors)
                        //{
                        //    ModelState.AddModelError("", err.Description);

                        //}
                    }
                    
                }

            }
            return Page();
        }
    }
}
