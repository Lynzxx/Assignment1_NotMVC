using System.ComponentModel.DataAnnotations;

namespace Assignment1_NotMVC.ViewModels
{
    public class OTPform
    {
        [Required(ErrorMessage = "Invalid OTP.")]
        public string OTP { get; set; }
    }
}
