using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // ds dto represents the content of the request body a user will use to register to the app
    public class RegisterDto
    {
        // we will be mapping this properties to our model.. d 'UserName' here can also be 'Username', it doesnt mean since at the end, we will map anything passed here to the right property of the AppUser entity..
        //ds fields are required..
        [Required] public string UserName { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }
        [Required]
        // Adding a minimum length validation on the register request body
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
