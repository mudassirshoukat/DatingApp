namespace API.DTO.PhotoDtos
{
    public class PhotoApprovalResponseDto
    {


        public int Id { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
       
        public bool IsApproved { get; set; }
    }
}
