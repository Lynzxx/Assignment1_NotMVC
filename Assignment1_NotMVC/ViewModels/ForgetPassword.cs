using System.ComponentModel.DataAnnotations;

namespace Assignment1_NotMVC.ViewModels
{
    public class ForgetPassword
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
