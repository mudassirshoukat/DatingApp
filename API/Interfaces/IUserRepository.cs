using API.DTO.MemberDtos;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        bool UserExists(int id);
        Task<PagedList<AppUser>> GetAllUserAsync(UserParams prms);
     
        Task<AppUser> GetUserByIdAsync(int Id);
        
        Task<AppUser> GetUserByUserNameAsync(string UserName);
       
        void DeleteUserAsync(AppUser user);

    }
}
