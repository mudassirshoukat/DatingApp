using API.DTO.PhotoDtos;
using API.Entities;

namespace API.Interfaces.RepoInterfaces
{
    public interface IPhotoRepository
    {
        Task<Photo> GetPhotoByIdAsync(int id);
        Task<Photo?> GetMainPhotoByAppUserIdAsync(int userId);
        Task<IEnumerable<PhotoApprovalResponseDto>> GetUnApprovedPhotosAsync();
        void DeletePhoto(Photo photo);
    }
}
