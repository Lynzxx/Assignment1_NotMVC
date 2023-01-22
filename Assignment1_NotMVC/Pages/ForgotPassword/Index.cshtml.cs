using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace Assignment1_NotMVC.Pages.ForgotPassword
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public ForgetPassword FPModel { get; set; }

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager=userManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(FPModel.Email);
                if (user != null)
                {
                    var token=await _userManager.GeneratePasswordResetTokenAsync(user);
                    //fix the password reset link
                    //var passwordResetLink = Url.Action("ResetPassword", "ResetPassword", new { email = FPModel.Email, token = token }, Request.Scheme);
                    var passwordResetLink = "https://localhost:44300/ResetPassword?email=" +FPModel.Email+"&token="+token;
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = string.Format("Email has been sent to reset password.");

                    MailMessage mail = new MailMessage();
                    mail.To.Add(new MailAddress(user.Email));
                    mail.From = new MailAddress("testingproject811@gmail.com");
                    mail.Subject = "Assignment Non-MVC Project";
                    mail.Body = "Click on this link to reset your password. "+passwordResetLink;
                    mail.IsBodyHtml = true;

                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Port = 587; // 25 465
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Credentials = new System.Net.NetworkCredential("testingproject811@gmail.com", "uchkfzqnojxaztdh");
                    smtp.Send(mail);

                    return Redirect("/Login");
                }
            }
            return Page();
        }
    }
}
