using System.ComponentModel.DataAnnotations;

namespace Assignment1_NotMVC.ViewModels
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="No Google ReCaptcha Token.")]
        public string Token { get; set; }
        public bool RememberMe { get; set; }



    }
}
