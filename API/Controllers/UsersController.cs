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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        // ds return the username of the logged in user
        public string GetUsername()
        {
            var username = User.FindFirst(claim => claim.Type == "username")?.Value;
            return username;
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
        // we give this route a "Name" so that we can use this name inside our UploadPhoto endpoint to return this GetUser route as a string..
        [HttpGet("{username}", Name = "GetUser")]
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
            // var username = User.FindFirst(claim => claim.Type == "username")?.Value;

            var username = GetUsername();

            //using the username gtin from d token, we find the user details from d db
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

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = GetUsername();

            //using the username gtin from d token, we find the user details from d db
            var user = await _userRepository.GetUserByUsernameAsync(username);
            var result = await _photoService.AddPhotoAsync(file);
            // result.Error & result.Error.Message comes from cloudinary so there can be error during upload and cloudinary returns d error msg
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            // if there are no error, we fill each field of our PhotoDto with their respective field from Cloudinary imageUploadResult
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,

            };
            //If the user does not av any photo at the moment, we set this photo the user is uploading to the main photo
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            // we then add the photo to the user's photo list
            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
            {
                // return  _mapper.Map<PhotoDto>(photo);

                // now ds returns a 201 response specifying our GetUser endpoint as where to get the uploaded photos and then also send our photoDto object
                // bcos GetUser endpoint expects a username parameter, ds is not going to work
                // return CreatedAtRoute("GetUser", _mapper.Map<PhotoDto>(photo));

                // here we using the 3rd overload : - d routeName, value for the parameter the route is expecting (username), and the object to return as response
                return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem Adding Photo!");
        }

        [HttpPut("set-main-photo/{id}")]
        // we pass d photoId of the photo we want to set as d main photo as argument
        public async Task<ActionResult> SetMainPhoto(int id)
        {
            var username = GetUsername();

            //using the username gtin from d token, we find the user details from d db
            var user = await _userRepository.GetUserByUsernameAsync(username);
            // using d photoId argument, we fetch the photo dt owns the photoId passed.. ds will b d photo to be set as main
            var photo = user.Photos.FirstOrDefault(p => p.Id == id);
            // we check if d photo to b set as main is already d main photo
            if (photo != null && photo.IsMain)
            {
                return BadRequest("This is already your main photo");
            }
            // we gt the photo dt is currently set as d main photo
            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentMain != null)
            {
                // we remove the current main photo from being d main photo
                currentMain.IsMain = false;
                // we set the photo with the passed photoId as d main photo
                photo.IsMain = true;
            }

            // if d changes saved successfully
            if (await _userRepository.SaveAllAsync())
            {
                // we return 204
                return NoContent();
            }

            // else if something failed, we return 400 bad request
            return BadRequest("Failed To Set Main Photo");

        }

        [HttpDelete("delete-photo/{id}")]
        public async Task<ActionResult> DeletePhoto(int id)
        {
            var username = GetUsername();

            //using the username gtin from d token, we find the user details from d db
            var user = await _userRepository.GetUserByUsernameAsync(username);
            // using d photoId argument, we fetch the photo dt owns the photoId passed.. ds will b d photo to be set as main
            var photo = user.Photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            if (photo.IsMain)
            {
                return BadRequest("You cannot delete your main photo");
            }

            // we confirm if the photo contains a publicId (which means it was uploaded to cloudinary bcos its nt all our photos dt r on cloud)
            if (photo.PublicId != null)
            {
             var result = await _photoService.DeletePhotoAsync(photo.PublicId);
             // if deletion fails
             if (result.Error != null)
             {
                 // we return the error message
                 return BadRequest(result.Error.Message);
             }
            }

            // we remove the photo from d database
            user.Photos.Remove(photo);
            // if successful
            if (await _userRepository.SaveAllAsync())
            {
                return Ok();
            }

            //if failed
            return BadRequest("Failed to delete the photo");
        }
    }
}
