using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;

namespace Assignment1_NotMVC.Pages.Actions
{
    public class GoogleResponseModel : PageModel
    {
        public string JSONCLAIMS { get; set; }
        public async Task OnGet()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
            var jsonClaims = JsonConvert.SerializeObject(claims, Formatting.Indented);
            JSONCLAIMS=jsonClaims;
        }

       
    }
}
