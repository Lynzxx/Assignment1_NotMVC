using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;
using Assignment1_NotMVC.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Assignment1_NotMVC.Pages
{
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
       private readonly IHttpContextAccessor contxt;

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private RoleManager<IdentityRole> roleManager { get; }
        private IWebHostEnvironment _environment;
        private readonly IDataProtector _dataProtector;
        private readonly AuditLogServices _auditLogServices;
        public Audit userAudit = new Audit();

        [BindProperty]
        public Register RModel { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }
        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole>rolesManager,IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment environment,IDataProtectionProvider dataProtectionProvider,DataProtectionPurposeStrings dataProtectionPurposeStrings,
        AuditLogServices auditLogServices)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = rolesManager;
            this.contxt = httpContextAccessor;
            this._environment = environment;
            this._dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.key);
            this._auditLogServices = auditLogServices;
            
        }

        public void OnGet()
        {
        }

        //Save data into the database

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var DontExist = true;
                var users = userManager.Users.ToList();
                for (int i = 0; i < users.Count; i++)
                {
                    var currentUser = users[i];
                    if (currentUser.Email == RModel.Email)
                    {
                        DontExist = false;
                        ModelState.AddModelError("emailExists", "Email Exists. ");
                    }
                }
                if (DontExist)
                {
                    if (Upload != null)
                    {
                        if (Upload.Length > 2 * 1024 * 1024)
                        {
                            ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                            return Page();
                        }
                        var uploadsFolder = "uploads";
                        var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                        var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                        using var fileStream = new FileStream(imagePath, FileMode.Create);
                        await Upload.CopyToAsync(fileStream);
                        RModel.Photo = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                    }
                    else
                    {
                        RModel.Photo = "/uploads/user.png";
                    }

                    var creditCardNumber = _dataProtector.Protect(RModel.CreditCardNo);
                    var sanitizer = new HtmlSanitizer();
                    var sanitizedName=sanitizer.Sanitize(RModel.Name);
                    var sanitizedAboutMe=sanitizer.Sanitize(RModel.AboutMe);
                    var sanitizedDelivery = sanitizer.Sanitize(RModel.DeliveryAddr);
                    var encodedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(sanitizedName));
                    var encodedAboutMe = Convert.ToBase64String(Encoding.UTF8.GetBytes(sanitizedAboutMe));
                    var encodedDelivery = Convert.ToBase64String(Encoding.UTF8.GetBytes(sanitizedDelivery));

                    var user = new ApplicationUser()
                    {
                        UserName = RModel.Email,
                        Email = RModel.Email,
                        Name = encodedName,
                        DeliveryAddr = encodedDelivery,
                        PhoneNumber = RModel.MobileNo,
                        CreditCardNo = creditCardNumber,
                        AboutMe = encodedAboutMe,
                        Gender = RModel.Gender,
                        Photo = RModel.Photo,
                    TwoFactorEnabled =true,
                        LastPasswordChangedDate=DateTime.Now
                    };
                    user.PastPassword1 = userManager.PasswordHasher.HashPassword(user, RModel.Password);

                    //Create the Member role if NOT exist
                    IdentityRole role = await roleManager.FindByIdAsync("Member");
                    if (role == null)
                    {
                        IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Member"));
                        if (!result2.Succeeded)
                        {
                            ModelState.AddModelError("", "Create role member failed");
                        }
                    }

                    
                    var result = await userManager.CreateAsync(user, RModel.Password);
                    if (result.Succeeded)
                    {

                        //Add users to Admin Role
                        result = await userManager.AddToRoleAsync(user, "Member");

                        await signInManager.SignInAsync(user, false);
                        return Redirect("/RegisterAudit");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return Page();
        }

    }
}
