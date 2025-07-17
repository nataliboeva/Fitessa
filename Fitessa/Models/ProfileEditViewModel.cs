using System.ComponentModel.DataAnnotations;

namespace Fitessa.Models
{
    public class ProfileEditViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Display(Name = "Profile Picture URL")]
        [Url]
        public string? ProfilePictureUrl { get; set; }
    }
} 