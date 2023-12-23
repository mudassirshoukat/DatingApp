using API.DTO.LikeDtos;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Helpers.QueryParams;
using API.Interfaces.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext context;

        public LikesRepository(DataContext context)
        {
            this.context = context;
        }


        public async Task<UserLike> GetUserLike(int SourceUserId, int TargetUserId)
        {
        return await context.Likes
                .FindAsync(SourceUserId,TargetUserId);
        }

        public async Task<PagedList<AppUser>> GetUserLikes(LikeQueryParams prms)
        {
            var Users = context.Users.OrderBy(x => x.UserName).AsQueryable();
            var Likes = context.Likes.AsQueryable();

            // Include all photos for each user
            Users = Users.Include(x => x.Photos);

            if (prms.Predicate == "Liked")
            {
                Likes = Likes.Where(like => like.SourceUserId == prms.UserId);
                Users = Users.Where(user => Likes.Any(like => like.TargetUserId == user.Id));
            }

            if (prms.Predicate == "LikedBy")
            {
                Likes = Likes.Where(like => like.TargetUserId == prms.UserId);
                Users = Users.Where(user => Likes.Any(like => like.SourceUserId == user.Id));
            }

            var page = await PagedList<AppUser>.CreateAsync(Users, prms.PageNumber, prms.PageSize);
            return page;
        }

        //public async Task<PagedList<AppUser>> GetUserLikes(LikeQueryParams prms)
        //{
        //    var Users = context.Users.OrderBy(x => x.UserName).AsQueryable();
        //    var Likes = context.Likes.AsQueryable();

        //    // Include all photos for each user
        //    Users = Users.Include(x => x.Photos);


        //    if (prms.Predicate == "Liked")
        //    {
        //        Likes = Likes.Where(like => like.SourceUserId == prms.UserId);
        //        Users = Likes.Select(x => x.TargetUser);
        //    }

        //    if (prms.Predicate == "LikedBy")
        //    {
        //        Likes = Likes.Where(like => like.TargetUserId == prms.UserId);
        //        Users = Likes.Select(x => x.SourceUser);
        //    }

        //     var page= await PagedList<AppUser>.CreateAsync(Users, prms.PageNumber, prms.PageSize);
        //    return page;
        //}


        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await context.Users.Include(x => x.LikedUsers).FirstOrDefaultAsync(x=>x.Id==userId);
        }
    }
}
