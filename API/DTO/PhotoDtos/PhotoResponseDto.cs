using API.Entities;

namespace API.DTO.PhotoDtos
{
    public class PhotoResponseDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
      
    }
}
