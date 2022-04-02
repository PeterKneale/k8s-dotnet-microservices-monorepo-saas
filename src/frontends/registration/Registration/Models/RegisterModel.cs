using System.ComponentModel.DataAnnotations;

namespace Registration.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Required]
        public string Reference { get; set; }
    }
}