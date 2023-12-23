using API.DTO.LikeDtos;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Helpers.QueryParams;
using API.Interfaces.RepoInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikeController : BaseApiController
    {
        private readonly IUserRepository userRepo;
        private readonly ILikesRepository likeRepo;
        private readonly IMapper mapper;

        public LikeController(IUserRepository UserRepo,ILikesRepository LikeRepo,IMapper mapper)
        {
            userRepo = UserRepo;
            likeRepo = LikeRepo;
            this.mapper = mapper;
        }



        [HttpPost("{UserName}")]
        public async Task<ActionResult> AddLike( string UserName)
        {
            int SourceUserId = User.GetUserId();
            var SourceUser=await userRepo.GetUserByIdAsync(SourceUserId);
            var LikedUser =await userRepo.GetUserByUserNameAsync(UserName);

            if (LikedUser == null) return NotFound();
            if (SourceUser.UserName == UserName) return BadRequest("User can't like ownSelf");
            var userLike = await likeRepo.GetUserLike(SourceUserId, LikedUser.Id);
            if (userLike != null) return BadRequest("You Already Liked This Member");

            userLike = new UserLike
            {
                SourceUserId = SourceUserId,
                TargetUserId = LikedUser.Id
            };
            SourceUser.LikedUsers.Add(userLike);    

            if (await userRepo.SaveAllAsync()) return Ok();
            return BadRequest("Failed To Like This Member");

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<LikeResponseDto>>> getUserLikes([FromQuery] LikeQueryParams prms/* [FromQuery] string Predicate*/)
        {
          

            prms.UserId = User.GetUserId();
            var likeUsers = await likeRepo.GetUserLikes(prms);
            var MappedList = mapper.Map<IEnumerable<LikeResponseDto>>(likeUsers);
            var mappedPagedList = new PagedList<LikeResponseDto>(
                MappedList,
                likeUsers.TotalCount,
                likeUsers.CurrentPage,
                likeUsers.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(
                mappedPagedList.CurrentPage,
                mappedPagedList.PageSize,
                mappedPagedList.TotalCount,
                mappedPagedList.TotalPages));

            return Ok(mappedPagedList);
        }

    }
}
