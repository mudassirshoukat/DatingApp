using System.ComponentModel.DataAnnotations;

namespace API.DTO.MessageDtos
{
    public class CreateMessageDto
    {
        [Required]
        public string RecipientUserName { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
