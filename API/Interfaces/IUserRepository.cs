using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void UpdateUser(AppUser user); // to update a user
        Task<bool> SaveAllAsync(); // an async task to save all changes
        Task<IEnumerable<AppUser>> GetUsersAsync(); // return list of all users
        Task<AppUser> GetUserByIdAsync(int id); // get a specific user using the user id
        Task<AppUser> GetUserByUsernameAsync(string username); // get a specific user using the user username
        Task<IEnumerable<MemberDto>> GetMembersAsync(); // we will be using our MemberDto to get users directly from our db instead of getting all data from db and then converting them to dto
        Task<MemberDto> GetMemberAsync(string username); // same as line 15
    }
}
