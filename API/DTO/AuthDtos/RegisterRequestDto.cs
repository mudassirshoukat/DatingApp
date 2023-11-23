using System.ComponentModel.DataAnnotations;

namespace API.DTO.AuthDtos

{
    public class RegisterRequestDto
    {
        [Required] public string Gender { get; set; }
        [Required] public DateOnly? DateOfBirth { get; set; }//make optional for required. silly
        [Required] public string knownAs { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }


        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
