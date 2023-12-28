using API.DTO.MemberDtos;
using API.DTO.PhotoDtos;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Helpers.QueryParams;
using API.Interfaces;
using API.Interfaces.RepoInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUnitOfWork unitOfWork,  IMapper mapper, IPhotoService photoService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this._photoService = photoService;
        }





        [HttpGet]

        public async Task<ActionResult<PagedList<MemberResponseDto>>> GetUsers([FromQuery] UserQueryParams prms)
        {
            var gender = await unitOfWork.UserRepository.GetGenderByUserName(User.GetUserName());
            prms.CurrentUserName = User.GetUserName();
            if (string.IsNullOrEmpty(prms.Gender))
            {
                prms.Gender = gender == "male" ? "female" : "male";
            }


            PagedList<AppUser> users = await unitOfWork.UserRepository.GetAllUserAsync(prms);

            var mappedUsers = mapper.Map<IEnumerable<MemberResponseDto>>(users);

            var mappedPagedList = new PagedList<MemberResponseDto>(
                mappedUsers,
                users.TotalCount,
                users.CurrentPage,
                users.PageSize
            );

            Response.AddPaginationHeader(
                new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(mappedPagedList);
        }



       
        [HttpGet()]
        [Route("{UserName}")]
        public async Task<ActionResult<MemberResponseDto>> GetUserByUserName(string UserName)
        {
            var appUser = await unitOfWork.UserRepository.GetUserByUserNameAsync(UserName);

            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<MemberResponseDto>(appUser));
        }




        [HttpPut()]
        public async Task<IActionResult> PutUser(MemberUpdateRequestDto member)
        {
            string userName = User.GetUserName();
            AppUser user = await unitOfWork.UserRepository.GetUserByUserNameAsync(userName);
            mapper.Map(member, user);
            if (await unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update resource");

        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<AppUser>> PostUser([FromBody] AppUser appUser)
        //{
        //    _context.Users.Add(appUser);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAppUser", new { id = appUser.id }, appUser);
        //}

        // DELETE: api/Users/5
        [HttpDelete()]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var appUser = await unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (appUser == null)
            {

                return NotFound();
            }

            unitOfWork.UserRepository.DeleteUserAsync(appUser);
            await unitOfWork.Complete();

            return NoContent();
        }




        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoResponseDto>> AddPhoto(IFormFile file)
        {
            AppUser user = await unitOfWork.UserRepository.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return NotFound();
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId

            };
            if (user.Photos.Count == 0)
                photo.IsMain = true;
            user.Photos.Add(photo);
            if (await unitOfWork.Complete())
                return CreatedAtAction(nameof(GetUserByUserName),
                    new { UserName = user.UserName }, mapper.Map<PhotoResponseDto>(photo));
            return BadRequest("Problem Adding Photo");

        }



        [HttpDelete("delete-photo/{Id:int}")]
        public async Task<ActionResult> DeletePhoto(int Id)
        {
            var user = await unitOfWork.UserRepository.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return BadRequest(new { Message = "User not found" });

            var photoToDelete = user.Photos.FirstOrDefault(x => x.Id == Id);
            if (photoToDelete == null) return NotFound(new { Message = "Photo not found" });



            if (photoToDelete.IsMain) return BadRequest("You Cant Delete Main Photo");

            if (photoToDelete.PublicId != null)
            {
                var res = await _photoService.DeletePhotoAsync(photoToDelete.PublicId);
                if (res.Error != null) return BadRequest("Failed To Delete Photo");
            }


            user.Photos.Remove(photoToDelete);

            if (await unitOfWork.Complete()) return Ok(new { message = "Delete Success" });


            return BadRequest(new { Message = "Failed to delete photo" });
        }



        [HttpPut("set-main-photo/{PhotoId}")]
        public async Task<ActionResult> SetMainPhoto(int PhotoId)
        {
            var user = await unitOfWork.UserRepository.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == PhotoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("Photo Already Setted As Main");
            var currentmain = user.Photos.FirstOrDefault<Photo>(x => x.IsMain);
            if (currentmain != null) currentmain.IsMain = false;
            photo.IsMain = true;
            if (await unitOfWork.Complete()) return NoContent();
            return BadRequest("Problem setting the Main Photo");
        }
    }
}
