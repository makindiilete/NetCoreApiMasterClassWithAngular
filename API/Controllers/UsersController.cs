using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //  api/users
    public class UsersController : ControllerBase
    {
        // We init our dbcontext class here to cater for data fetching from the backend
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        //Get api to return all users =>  api/users
        [HttpGet]
        // ds method will return a List of users
         public async Task<ActionResult<List<AppUser>>> GetUsers()
        {
            //_context field now points to our 'Users' table defined as a dbset inside DataContext.cs so return all users and convert them to list
            // return _context.Users.ToList();
            return await _context.Users.ToListAsync();
        }

        //    api/users/3
        [HttpGet("{id}")]
        // ds method will return the user with the given id
        public async Task<ActionResult<AppUser>>  GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                return await _context.Users.FindAsync(id);

            }

            return NotFound();
        }
    }
}
