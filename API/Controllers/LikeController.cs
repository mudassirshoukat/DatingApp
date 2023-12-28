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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LikeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }



        [HttpPost("{UserName}")]
        public async Task<ActionResult> AddLike( string UserName)
        {
            int SourceUserId = User.GetUserId();
            var SourceUser=await unitOfWork.UserRepository.GetUserByIdAsync(SourceUserId);
            var LikedUser =await unitOfWork.UserRepository.GetUserByUserNameAsync(UserName);

            if (LikedUser == null) return NotFound();
            if (SourceUser.UserName == UserName) return BadRequest("User can't like ownSelf");
            var userLike = await unitOfWork.LikesRepository.GetUserLike(SourceUserId, LikedUser.Id);
            if (userLike != null) return BadRequest("You Already Liked This Member");

            userLike = new UserLike
            {
                SourceUserId = SourceUserId,
                TargetUserId = LikedUser.Id
            };
            SourceUser.LikedUsers.Add(userLike);    

            if (await unitOfWork.Complete()) return Ok();
            return BadRequest("Failed To Like This Member");

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<LikeResponseDto>>> getUserLikes([FromQuery] LikeQueryParams prms/* [FromQuery] string Predicate*/)
        {
          

            prms.UserId = User.GetUserId();
            var likeUsers = await unitOfWork.LikesRepository.GetUserLikes(prms);
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
