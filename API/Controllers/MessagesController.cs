using API.DTO.MessageDtos;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Helpers.QueryParams;
using API.Interfaces.RepoInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository userRepo;
        private readonly IMessageRepository messageRepo;
        private readonly IMapper mapper;

        public MessagesController(IUserRepository userRepo,IMessageRepository messageRepo,IMapper mapper)
        {
            this.userRepo = userRepo;
            this.messageRepo = messageRepo;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto messageDto)
        {
            var CurrentUserName = User.GetUserName();
            if (CurrentUserName.ToLower() == messageDto.RecipientUserName.ToLower()) return BadRequest("You Connot Send message To Youself");

            var Sender = await userRepo.GetUserByUserNameAsync(CurrentUserName);
            var Recipient = await userRepo.GetUserByUserNameAsync(messageDto.RecipientUserName);
            if (Recipient == null) return NotFound();
            var message = new Message
            {
                Sender=Sender,
                SenderUserName = CurrentUserName,
                Recipient = Recipient,
                RecipientUserName=Recipient.UserName,
                Content=messageDto.Content,
             };

            messageRepo.AddMessage(message);
            if(await messageRepo.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));
            return BadRequest("Failed To Send message");

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageQueryParams prms)
        {
            prms.UserName = User.GetUserName();
            var messagesDto = await messageRepo.GetMessagesForUser(prms);
            Response.AddPaginationHeader(
                new PaginationHeader(messagesDto.CurrentPage,messagesDto.PageSize,messagesDto.TotalCount,messagesDto.TotalPages));

            return Ok(messagesDto);
        }


        [HttpGet("thread/{UserName}")]
        public async   Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesThread( string UserName)
        {
            var CurrentUserName = User.GetUserName();
            return Ok(await messageRepo.GetMessageThread(CurrentUserName, UserName));
        }





        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteMessage(int Id)
        {
            var UserName = User.GetUserName();
            var message =await  messageRepo.GetMessage(Id);
            if (message == null) return BadRequest();
            if (message.RecipientUserName != UserName && message.SenderUserName != UserName) return Unauthorized();

            if (message.SenderUserName == UserName) message.SenderDeleted = true;
            if (message.RecipientUserName == UserName) message.RecipientDeleted = true;

            if (message.RecipientDeleted && message.SenderDeleted) { messageRepo.DeleteMessage(message); }

            if(await messageRepo.SaveAllAsync()) return Ok();
            return BadRequest("Failed to delete message");

        }

    }
}
