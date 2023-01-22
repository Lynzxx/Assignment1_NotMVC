using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1_NotMVC.Pages.Actions
{
    public class GoogleLoginModel : PageModel
    {
        public async Task OnGet()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri=Url.Page("/GoogleResponse")
            });
        }
    }
}
