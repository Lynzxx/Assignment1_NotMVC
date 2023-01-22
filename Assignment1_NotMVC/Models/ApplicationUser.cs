using Microsoft.AspNetCore.Identity;

namespace Assignment1_NotMVC.Models
{
    public class ApplicationUser: IdentityUser
    { 
        public string Name { get; set; }
        public string Gender { get; set; }
        public string AboutMe { get; set; }
        public string Photo { get; set; }
        public string CreditCardNo { get; set; }
        public string DeliveryAddr { get; set; }

        public string? PastPassword1 { get; set; }

        public string? PastPassword2 { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime? LastOTPGeneratedDate { get; set; }

        public string? OTP { get; set; }

    }
}
