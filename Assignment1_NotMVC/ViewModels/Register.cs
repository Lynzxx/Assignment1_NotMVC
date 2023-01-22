using System.ComponentModel.DataAnnotations;

namespace Assignment1_NotMVC.ViewModels
{
    public class Register
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required, MaxLength(1)]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$", ErrorMessage = "Invalid Credit Card Number. Visa, MasterCard, American Express, Diners Club, Discover, and JCB cards")
        [Required, RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$", ErrorMessage = "Invalid Credit Card Number. Visa, MasterCard, American Express, Diners Club, Discover, and JCB cards")]
        [Display(Name = "Credit Card Number")]
        public string CreditCardNo { get; set; }

        [Required, MinLength(8, ErrorMessage = "Enter at least 8 characters."), MaxLength(8, ErrorMessage = "Enter 8 characters.")]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddr { get; set; }

        [Required, RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Invalid Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //Minimum 12 characters, at least one uppercase letter, one lowercase letter, one number and one special character
        [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$", ErrorMessage = "Invalid Password. Minimum 12 characters, at least one uppercase letter, one lowercase letter, one number and one special character."), MinLength(12)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Minimum 12 characters, at least one uppercase letter, one lowercase letter, one number and one special character
        [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$", ErrorMessage = "Invalid Password. Minimum 12 characters, at least one uppercase letter, one lowercase letter, one number and one special character."), MinLength(12)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Photo")]
        public string? Photo { get; set; }

        [MaxLength(200)]
        [Display(Name = "About Me")]
        public string AboutMe { get; set; }
    }
}
