using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // ds dto represents the content of the request body a user will use to register to the app
    public class RegisterDto
    {
        // we will be mapping this two properties to our model.. d 'UserName' here can also be 'Username', it doesnt mean since at the end, we will map anything passed here to the right property of the AppUser entity..
        //ds fields are required..
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
