using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Assignment1_NotMVC.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using System.Net.Mail;

namespace Assignment1_NotMVC.Pages
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {

        [BindProperty]
        public Login LModel { get; set; }

        public MimeMessage message { get; set; }

        private readonly GoogleCaptchaService _captchaService;

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUser currentUser;
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit = new Audit();

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,GoogleCaptchaService googleCaptchaService)
        {
            this.signInManager = signInManager;
            this._userManager = userManager;
            this._captchaService = googleCaptchaService;
        }
        public void OnGet()
        {
            //test
            //MailMessage mail = new MailMessage();
            //mail.To.Add(new MailAddress("aaralynchow2004@gmail.com"));
            //mail.From = new MailAddress("testingproject811@gmail.com");
            //mail.Subject = "Testing Project Mail";
            //mail.Body = "Just testing to make sure it works.";
            //mail.IsBodyHtml = true;

            //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            //smtp.Port = 587; // 25 465
            //smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = false;
            //smtp.Host = "smtp.gmail.com";
            //smtp.Credentials = new System.Net.NetworkCredential("testingproject811@gmail.com", "uchkfzqnojxaztdh");
            // smtp.Send(mail);   

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Verify response token with Google
                var captchaResult=await _captchaService.VerifyToken(LModel.Token);
                if (captchaResult==false)
                {
                    return Page();
                }

                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
                LModel.RememberMe,true);
                if (identityResult.Succeeded)
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(LModel.Email);
                    if (user.TwoFactorEnabled)
                    {
                        Random r = new Random();
                        string otp=r.Next(10001,99999).ToString();
                        user.OTP = otp;
                        user.LastOTPGeneratedDate = DateTime.Now;

                        await _userManager.UpdateAsync(user);

                        MailMessage mail = new MailMessage();
                        mail.To.Add(new MailAddress(user.Email));
                        mail.From = new MailAddress("testingproject811@gmail.com");
                        mail.Subject = "Testing Project Mail";
                        mail.Body = "Your OTP is "+otp ;
                        mail.IsBodyHtml = true;

                        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                        smtp.Port = 587; // 25 465
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Credentials = new System.Net.NetworkCredential("testingproject811@gmail.com", "uchkfzqnojxaztdh");
                        smtp.Send(mail);

                        return RedirectToPage("OTP", new {email=LModel.Email});

                    }
                    
                    return RedirectToPage("changeSecurityStamp");

                }
                if (identityResult.IsLockedOut)
                {
                    return RedirectToPage("/AccountLocked",new { email=LModel.Email});
                }
               
                ModelState.AddModelError("", "Username or Password incorrect");
                

              
            }
            return Page();
        }
    }
}
