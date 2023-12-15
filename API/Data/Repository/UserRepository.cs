using API.Entities;
using API.Helpers;
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



        //async Task<IEnumerable<AppUser>> IUserRepository.GetAllUserAsync(UserParams prms)
        //{
        //    return await _context.Users.Include(x=>x.Photos).ToListAsync() ;
        //}

        async Task<PagedList<AppUser>> IUserRepository.GetAllUserAsync(UserQueryParams prms)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(x => x.UserName != prms.CurrentUserName);
            query = query.Where(x => x.Gender == prms.Gender);
            var minDob= DateOnly.FromDateTime(DateTime.Today.AddYears(-prms.MaxAge-1));
            var maxDob= DateOnly.FromDateTime(DateTime.Today.AddYears(-prms.MinAge));
            query = query.Where(x => x.DateOfBirth <= maxDob && x.DateOfBirth >= minDob);
            query = prms.OrderBy switch
            {
                "Created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)

            };


            query =query.Include(x => x.Photos).AsNoTracking();
            return await PagedList<AppUser>.CreateAsync(query, prms.PageNumber, prms.PageSize);
        }

        async Task<AppUser?> IUserRepository.GetUserByIdAsync(int Id)
        {
            return await _context.Users.FindAsync(Id);
        }

        async Task<AppUser?> IUserRepository.GetUserByUserNameAsync(string UserName)
        {
             var appUser =await _context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x=>x.UserName.ToLower()==UserName.ToLower());
            return appUser;
       }

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
