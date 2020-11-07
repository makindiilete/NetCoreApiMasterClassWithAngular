using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        // api/buggy/auth : - Manage 401 Unauthorized
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        // api/buggy/not-found : - Manage 404 Not Found error
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            // We find a user id dt we no definitely does not exist
            var userNotFound = _context.Users.Find(-1);
            if (userNotFound == null)
            {
                return NotFound();
            }

            return Ok(userNotFound);
        }

        // api/buggy/server-error : - Manage 500 server error
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {

                var userNotFound = _context.Users.Find(-1);
                // we know we are not going to get any user so 'userNotFound' is going to be null and converting null to string will return a null ref exception (500)
                var nullUserToString = userNotFound.ToString();
                return nullUserToString;
        }

        // api/buggy/bad-request : - Manage 400 bad request
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}
