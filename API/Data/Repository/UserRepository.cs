using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace API.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper mapper;

        public UserRepository(DataContext context,IMapper mapper)
        {
            this._context = context;
            this.mapper = mapper;
        }



        async Task<IEnumerable<AppUser>> IUserRepository.GetAllUserAsync()
        {
            return await _context.Users.Include(x=>x.Photos).ToListAsync() ;
        }

        async Task<AppUser?> IUserRepository.GetUserByIdAsync(int Id)
        {
            return await _context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x=>x.id== Id);
        }

        async Task<AppUser?> IUserRepository.GetUserByUserNameAsync(string UserName)
        {
             var appUser =await _context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x=>x.UserName.ToLower()==UserName.ToLower());
            return appUser;
        Console.WriteLine("midassir"); }

        void IUserRepository.DeleteUserAsync(AppUser user)
        {
            _context.Users.Remove(user);
        }

        void IUserRepository.Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        async Task<bool> IUserRepository.SaveAllAsync()
        {
            
          return  await _context.SaveChangesAsync() >0;
        }

        public  bool UserExists(int id)
        {
            return  _context.Users.Any(x => x.id == id);
        }

      

       
    }
}
