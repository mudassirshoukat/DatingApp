using System.ComponentModel.DataAnnotations;

namespace API.DTO.AuthDtos

{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
