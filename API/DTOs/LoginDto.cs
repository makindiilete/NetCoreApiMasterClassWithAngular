namespace API.DTOs
{
    // ds dto represents the content of the request body a user will use to login to the app
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
