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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MessagesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto messageDto)
        {
            var CurrentUserName = User.GetUserName();
            if (CurrentUserName.ToLower() == messageDto.RecipientUserName.ToLower()) return BadRequest("You Connot Send message To Youself");

            var Sender = await unitOfWork.UserRepository.GetUserByUserNameAsync(CurrentUserName);
            var Recipient = await unitOfWork.UserRepository.GetUserByUserNameAsync(messageDto.RecipientUserName);
            if (Recipient == null) return NotFound();
            var message = new Message
            {
                Sender=Sender,
                SenderUserName = CurrentUserName,
                Recipient = Recipient,
                RecipientUserName=Recipient.UserName,
                Content=messageDto.Content,
             };

            unitOfWork.MessageRepository.AddMessage(message);
            if(await unitOfWork.Complete()) return Ok(mapper.Map<MessageDto>(message));
            return BadRequest("Failed To Send message");

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageQueryParams prms)
        {
            prms.UserName = User.GetUserName();
            var messagesDto = await unitOfWork.MessageRepository.GetMessagesForUser(prms);
            Response.AddPaginationHeader(
                new PaginationHeader(messagesDto.CurrentPage,messagesDto.PageSize,messagesDto.TotalCount,messagesDto.TotalPages));

            return Ok(messagesDto);
        }





        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteMessage(int Id)
        {
            var UserName = User.GetUserName();
            var message =await  unitOfWork.MessageRepository.GetMessage(Id);
            if (message == null) return BadRequest();
            if (message.RecipientUserName != UserName && message.SenderUserName != UserName) return Unauthorized();

            if (message.SenderUserName == UserName) message.SenderDeleted = true;
            if (message.RecipientUserName == UserName) message.RecipientDeleted = true;

            if (message.RecipientDeleted && message.SenderDeleted) { unitOfWork.MessageRepository.DeleteMessage(message); }

            if(await unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to delete message");

        }

    }
}
//abcabc