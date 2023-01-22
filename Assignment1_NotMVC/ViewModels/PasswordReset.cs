using System.ComponentModel.DataAnnotations;

namespace Assignment1_NotMVC.ViewModels
{
    public class PasswordReset
    {
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
    }
}
