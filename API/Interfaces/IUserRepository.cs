using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        bool UserExists(int id);
        Task<IEnumerable<AppUser>> GetAllUserAsync();
     
        Task<AppUser> GetUserByIdAsync(int Id);
        
        Task<AppUser> GetUserByUserNameAsync(string UserName);
       
        void DeleteUserAsync(AppUser user);

    }
}
