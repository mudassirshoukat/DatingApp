using System.ComponentModel.DataAnnotations;

namespace API.DTO.AuthDtos

{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(12,MinimumLength =8)]
        public string Password { get; set; }
    }
}
