using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    // inherits ITokenService
    public class TokenService : ITokenService
    {
        // ds key will b used to sign our JWT token
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration)
        {
            // ds key will b picked from our appsettings.development.json, so d app will look for a property with the name "TokenKey'
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }
        //Then here it implements the CreateToken()
        public string CreateToken(AppUser user)
        {
            //Here we start defining what we want to store inside the JWT token
            var claims = new List<Claim>
            {
                // we store our userName inside the jwt
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

                // Here we specify the signing algorithm to sign our token
                var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

                // we describe our token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // ds is wat we gonna store inside
                    Subject = new ClaimsIdentity(claims),
                    // the token expiry
                    Expires = DateTime.Now.AddDays(7),
                    // token signing credentials
                    SigningCredentials = creds
                };
                var tokenHandler = new JwtSecurityTokenHandler();

                // we create our token using the 'tokenDescriptor' object
                var token = tokenHandler.CreateToken(tokenDescriptor);
                // we write the token with our tokenHandler and return it
                return tokenHandler.WriteToken(token);
        }
    }
}
