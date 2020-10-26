using API.Entities;

namespace API.Interfaces
{
    //ds interface will create a token for the AppUser it receives..
    // it will now be the job of the class that implements ds interface to write the logic for generating the token maybe the classes choose to use JWT token or any other type of token
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
