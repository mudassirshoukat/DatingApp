using System.ComponentModel.DataAnnotations;

namespace API.DTO.AuthDtos

{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
