using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context; // to query d database
            _tokenService = tokenService; // to generate jwt tokens
        }

        //api/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //We first run a check to see if the username is already in the database
            if (await IsUserNameInUse(registerDto.UserName))
            {
                return BadRequest( "UserName Already Exist");
            }
            //ds provides us with the hashing algorithm for the password.. d 'using' statement tell d class to dispose when we r done
            using var hmac = new HMACSHA512();
            //create a new user
            var user = new AppUser
            {
                // we convert d passed username to lowercase b4 storing it in d db so dt when we check for it later we can check with lower case
                UserName = registerDto.UserName.ToLower(),
                //compute d string password to a hash
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            //add the new user to our Users table using our _context class
             await _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();
             //Now we simply return the UserDto specify values for its fields
             return new UserDto
             {
                 Token = _tokenService.CreateToken(user),
                 Username = user.UserName
             };
        }

        //api/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //We can also use 'FirstOrDefault()' here. d difference is dt 'SingleOrDefault' throws an exception if we have more than one match, 'FirstOrDefault' does not throw an error but returns d first match or null if no match is found
            var user = await _context.Users
                .SingleOrDefaultAsync(appUser => appUser.UserName == loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            //computing and confirming the password harsh using the password salt as key
            using var hmac = new HMACSHA512(user.PasswordSalt);

            //confirming if the passed password is the same as the one used for the harsh
            var computedHarsh = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //if the password sent does not match the one used for the harsh
            for (int i = 0; i < computedHarsh.Length; i++)
            {
                if (computedHarsh[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            //if they eventually passed all checks, we simply return the UserDto specify values for its fields
            return new UserDto
            {
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }

        private async Task<bool> IsUserNameInUse(string username)
        {
            // check d database to see if there is a userName with the passed string
            return await _context.Users.AnyAsync(appUser => appUser.UserName == username.ToLower());

        }

    }
}
