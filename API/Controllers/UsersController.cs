using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //Get api to return all users =>  api/users
        [HttpGet]
        // ds method will return a List of users
         public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //we get our users
            var users = await _userRepository.GetMembersAsync();
            // we then returned the shaped user
            return Ok(users);
        }

        //    api/users/mavis
        [HttpGet("{username}")]
        // ds method will return the user with the given username
        public async Task<ActionResult<MemberDto>>  GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);
            if (user != null)
            {
                return Ok(user);

            }

            return NotFound("No User Found With That Username");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // if u store d username in the token using : - new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // if u store d username in the token using : - new Claim("username", user.UserName),
            var username = User.FindFirst(claim => claim.Type == "username")?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            // without autoMapper we will av to do this
            // user.City = memberUpdateDto.City;
            // user.Country = memberUpdateDto.Country;
            // user.Introduction = memberUpdateDto.Introduction;
            // user.LookingFor = memberUpdateDto.LookingFor;
            // user.Interest = memberUpdateDto.Interest;

            //With AutoMapper, ds is all we need to do
            _mapper.Map(memberUpdateDto, user);

            //we call d user repository to update the user details
            _userRepository.UpdateUser(user);
            if (await _userRepository.SaveAllAsync())
            {
                // we return 204 reponse so we do not need to return any data to the user....
                return NoContent();
            }

            return BadRequest("Failed to update user");
        }
    }
}
