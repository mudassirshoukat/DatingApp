using API.DTO.LikeDtos;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int SourceUserId,int TargetUserId);
        Task<PagedList<AppUser>> GetUserLikes( LikeQueryParams prms);
        Task<AppUser> GetUserWithLikes(int userId);

    }
}
