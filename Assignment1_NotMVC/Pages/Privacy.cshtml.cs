using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Assignment1_NotMVC.Pages
{
    [Authorize(Roles = "Member")]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataProtector _dataProtector;
        private readonly AuditLogServices _auditLogServices;
        public ApplicationUser currentUser;
        public string creditCardNum { get; set; }
        public string deliveryAddr { get; set; }
        public string name { get; set; }

        public string aboutMe { get; set; }
        public Audit userAudit = new Audit();

        public PrivacyModel(AuditLogServices auditLogServices,ILogger<PrivacyModel> logger, SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _logger = logger;
            _userManager = userManager;
            _dataProtector= dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.key);
            _signInManager= signInManager;
            _auditLogServices= auditLogServices;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userid=_userManager.GetUserId(HttpContext.User);
            if(userid == null)
            {
                return Redirect("/Login");
            }
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            currentUser = user;
            var currentSecurityStamp= HttpContext.Session.GetString("SecurityStamp");
            if (currentUser.SecurityStamp != currentSecurityStamp)
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

            creditCardNum = _dataProtector.Unprotect(currentUser.CreditCardNo);
           deliveryAddr=Encoding.UTF8.GetString(Convert.FromBase64String(currentUser.DeliveryAddr));
            name = Encoding.UTF8.GetString(Convert.FromBase64String(currentUser.Name));
            aboutMe = Encoding.UTF8.GetString(Convert.FromBase64String(currentUser.AboutMe));

            //create audit
            var id = Guid.NewGuid().ToString();
            userAudit.AuditId = id;
            userAudit.UserId = userid;
            userAudit.Action = "GET";
            userAudit.TimeStamp = DateTime.Now;
            userAudit.AreaAccessed = "/Privacy";
            userAudit.UserEmail = user.Email;
            _auditLogServices.AddAudit(userAudit);

            return Page();
        }
    }
}