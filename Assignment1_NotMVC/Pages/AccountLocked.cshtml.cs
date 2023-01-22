using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages
{
    public class AccountLockedModel : PageModel
    {

        public string Email { get; set; }
        public void OnGet(string email)
        {
            Email = email;
            HttpContext.Session.SetString("accountLock", Email);
        }
    }
}
