using API.DTO.PhotoDtos;
using API.Entities;
using API.Interfaces.RepoInterfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public PhotoRepository(DataContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

   

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await context.Photos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PhotoApprovalResponseDto>> GetUnApprovedPhotosAsync()
        {
          var  data =await context.Photos.IgnoreQueryFilters()
                .Where(x => !x.IsApproved)
                .Include(x => x.AppUser)
                
                .ProjectTo<PhotoApprovalResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return data;
        }

        public void DeletePhoto(Photo photo)
        {
             context.Photos.Remove(photo);
        }

        public async Task<Photo> GetMainPhotoByAppUserIdAsync(int userId)
        {
           return await context.Photos.Where(x=>x.AppuserId == userId&& x.IsMain).FirstOrDefaultAsync();
        }
    }
}
