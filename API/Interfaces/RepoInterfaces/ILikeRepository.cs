using API.DTO.LikeDtos;
using API.Entities;
using API.Helpers;
using API.Helpers.QueryParams;

namespace API.Interfaces.RepoInterfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int SourceUserId, int TargetUserId);
        Task<PagedList<AppUser>> GetUserLikes(LikeQueryParams prms);
        Task<AppUser> GetUserWithLikes(int userId);

    }
}
