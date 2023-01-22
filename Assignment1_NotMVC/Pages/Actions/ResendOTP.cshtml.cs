using Assignment1_NotMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;

namespace Assignment1_NotMVC.Pages.Actions
{
    public class ResendOTPModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResendOTPModel(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return Redirect("/Login");
            }
            Random r = new Random();
            string otp = r.Next(10001, 99999).ToString();
            user.OTP = otp;
            user.LastOTPGeneratedDate = DateTime.Now;

            await _userManager.UpdateAsync(user);

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(user.Email));
            mail.From = new MailAddress("testingproject811@gmail.com");
            mail.Subject = "Testing Project Mail";
            mail.Body = "Your OTP is " + otp;
            mail.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Port = 587; // 25 465
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("testingproject811@gmail.com", "uchkfzqnojxaztdh");
            smtp.Send(mail);

            return Redirect("/OTP");
        }
    }
}
