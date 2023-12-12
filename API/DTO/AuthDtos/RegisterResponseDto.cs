namespace API.DTO.AuthDtos
{
    public class RegisterResponseDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
       
        public string KnownAs { get; set; }
        public string Gender { get; set; }
    }
}
