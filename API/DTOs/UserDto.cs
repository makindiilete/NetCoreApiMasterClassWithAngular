namespace API.DTOs
{
    //ds dto represents properties we want to send back to the user after they register or login
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
