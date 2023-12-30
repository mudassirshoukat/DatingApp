using API.DTO.PhotoDtos;
using API.Entities;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.RepoInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PhotosController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public PhotosController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoService = photoService;
        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoResponseDto>> AddPhoto(IFormFile file)
        {
            AppUser user = await unitOfWork.UserRepository.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return NotFound();
            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsApproved = false,
                IsMain = false,
            };


            user.Photos.Add(photo);
            if (await unitOfWork.Complete())
                return CreatedAtAction("GetUserByUserName", "Users",
                    new { UserName = user.UserName }, mapper.Map<PhotoResponseDto>(photo));
            return BadRequest("Problem Adding Photo");

        }



        [HttpDelete("delete-photo/{Id:int}")]
        public async Task<ActionResult> DeletePhoto(int Id)
        {
            var user = await unitOfWork.UserRepository.GetCurrentUserByUserNameAsync(User.GetUserName());
            if (user == null) return BadRequest(new { Message = "User not found" });

            var photoToDelete = user.Photos.FirstOrDefault(x => x.Id == Id);
            if (photoToDelete == null) return NotFound(new { Message = "Photo not found" });



            if (photoToDelete.IsMain) return BadRequest("You Cant Delete Main Photo");

            if (photoToDelete.PublicId != null)
            {
                var res = await photoService.DeletePhotoAsync(photoToDelete.PublicId);
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
            if (!photo.IsApproved) return BadRequest("Photo Is Not Approved Yet By Moderators");
            if (photo.IsMain) return BadRequest("Photo Already Setted As Main");
            var currentmain = user.Photos.FirstOrDefault<Photo>(x => x.IsMain);
            if (currentmain != null) currentmain.IsMain = false;
            photo.IsMain = true;
            if (await unitOfWork.Complete()) return NoContent();
            return BadRequest("Problem setting the Main Photo");
        }





        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("Photos-to-Moderate")]
        public async Task<ActionResult<IEnumerable<PhotoResponseDto>>> GetPhotosForModeration()
        {
            var photosDtos = await unitOfWork.PhotoRepository.GetUnApprovedPhotosAsync();
       
            return Ok(photosDtos);

        }



        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("photo-approval")]
        public async Task<ActionResult> PhotoApproval(PhotoApprovalRequestDto dto)
        {
            string successMessage = "Success";

            var photo = await unitOfWork.PhotoRepository.GetPhotoByIdAsync(dto.Id);
            if (photo == null) return NotFound("Photo NotFound" );
            if (photo.IsApproved) return BadRequest("Photo Already Approved");

            if (dto.Approve)
            {
                photo.IsApproved = true;
                var mainPhoto = await unitOfWork.PhotoRepository.GetMainPhotoByAppUserIdAsync(photo.AppuserId);
                if (mainPhoto == null)
                {
                    photo.IsMain = true;
                }

                successMessage = "Photo Approved SuccessFully";
            }
            else
            {
                if (photo.PublicId != null)
                {
                    var res = await photoService.DeletePhotoAsync(photo.PublicId);
                    if (res.Error != null) return BadRequest("Failed To Delete Rejected Photo" );
                }

                unitOfWork.PhotoRepository.DeletePhoto(photo);
                successMessage = "Rejected Photo Deleted";

            }
            if (await unitOfWork.Complete()) return Ok(new {Message=successMessage });
            return BadRequest("Something Went Wrong");


        }

    }

}
