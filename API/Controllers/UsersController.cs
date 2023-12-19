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
        private readonly IUserRepository _UserRepo;
        private readonly IMapper mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository repo, IMapper mapper, IPhotoService photoService)
        {
            this._UserRepo = repo;
            this.mapper = mapper;
            this._photoService = photoService;
        }



        // GET: api/Users
        //[HttpGet]
        ////[Authorize]
        //public async Task<ActionResult<IEnumerable<MemberResponseDto>>> GetUsers([FromQuery] UserParams prms)
        //{
        //    var users = await _UserRepo.GetAllUserAsync(prms);


        //    var dto = mapper.Map<IEnumerable<MemberResponseDto>>(users);
        //    return Ok(dto);
        //}

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<PagedList<MemberResponseDto>>> GetUsers([FromQuery] UserQueryParams prms)
        {
            var CurrentUser = await _UserRepo.GetUserByUserNameAsync(User.GetUserName());
            prms.CurrentUserName = CurrentUser.UserName;
            if (string.IsNullOrEmpty(prms.Gender))
            {
                prms.Gender=CurrentUser.Gender=="male"?"female":"male";
            }


            PagedList<AppUser> users = await _UserRepo.GetAllUserAsync(prms);

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

        // GET: api/Users/5
        //[HttpGet()]
        //[Route("{id:int}")]
        //public async Task<ActionResult<MemberResponseDto>> GetUserById(int id)
        //{
        //    var appUser = await _UserRepo.GetUserByIdAsync(id);

        //    if (appUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(mapper.Map<MemberResponseDto>(appUser));
        //}


        [HttpGet()]
        [Route("{UserName}")]
        public async Task<ActionResult<MemberResponseDto>> GetUserByUserName(string UserName)
        {
            var appUser = await _UserRepo.GetUserByUserNameAsync(UserName);

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
            AppUser user = await _UserRepo.GetUserByUserNameAsync(userName);
            mapper.Map(member, user);
            if (await _UserRepo.SaveAllAsync()) return NoContent();
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
            var appUser = await _UserRepo.GetUserByIdAsync(id);
            if (appUser == null)
            {

                return NotFound();
            }

            _UserRepo.DeleteUserAsync(appUser);
            await _UserRepo.SaveAllAsync();

            return NoContent();
        }



        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoResponseDto>> AddPhoto(IFormFile file)
        {
            AppUser user = await _UserRepo.GetUserByUserNameAsync(User.GetUserName());
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
            if (await _UserRepo.SaveAllAsync())
                return CreatedAtAction(nameof(GetUserByUserName),
                    new { UserName = user.UserName }, mapper.Map<PhotoResponseDto>(photo));
            return BadRequest("Problem Adding Photo");

        }



        [HttpDelete("delete-photo/{Id:int}")]
        public async Task<ActionResult> DeletePhoto(int Id)
        {
            var user = await _UserRepo.GetUserByUserNameAsync(User.GetUserName());
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

            if (await _UserRepo.SaveAllAsync()) return Ok(new { message= "Delete Success" });
           

            return BadRequest(new { Message = "Failed to delete photo" });
        }



        [HttpPut("set-main-photo/{PhotoId}")]
        public async Task<ActionResult> SetMainPhoto(int PhotoId)
        {
            var user = await _UserRepo.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return NotFound();
            
            var photo=user.Photos.FirstOrDefault(x=>x.Id == PhotoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("Photo Already Setted As Main");
            var currentmain = user.Photos.FirstOrDefault<Photo>(x => x.IsMain);
            if(currentmain != null) currentmain.IsMain=false;
            photo.IsMain = true;
            if (await _UserRepo.SaveAllAsync()) return NoContent();
            return BadRequest("Problem setting the Main Photo");
        }
    }
}
