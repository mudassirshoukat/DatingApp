﻿using API.DTO.MessageDtos;
using API.Entities;
using API.Helpers;
using API.Helpers.QueryParams;
using API.Interfaces.RepoInterfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    [Authorize]
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public MessageRepository(DataContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int Id)
        {
            return await context.Messages.FindAsync(Id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageQueryParams prms)
        {
            var query=context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();
            query = prms.Container switch
            {
                "InBox" => query.Where(x => x.RecipientUserName == prms.UserName&& !x.RecipientDeleted),
                "OutBox"=> query.Where(x=>x.SenderUserName==prms.UserName&& !x.SenderDeleted),
                _=> query.Where(x=>x.RecipientUserName==prms.UserName&&x.DateRead==null&& !x.RecipientDeleted)
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, prms.PageNumber, prms.PageSize);
        }



        public async Task<IEnumerable<MessageDto>> GetMessageThread(string CurrentUserName, string RecipientUserName)
        {
            var messages = await context.Messages
                .Include(x => x.Sender).ThenInclude(x => x.Photos)
                .Include(x => x.Recipient).ThenInclude(x => x.Photos)
                .Where(x=>
                x.SenderUserName==RecipientUserName&&x.RecipientUserName==CurrentUserName&& !x.RecipientDeleted
                || x.SenderUserName == CurrentUserName && x.RecipientUserName == RecipientUserName &&!x.SenderDeleted)
                .OrderBy(z => z.MessageSent)
                .ToListAsync();


            var UnreadMessages = messages.Where(x => x.DateRead == null && x.RecipientUserName == CurrentUserName).ToList();

            if (UnreadMessages.Any())
            {
                foreach (var msg in UnreadMessages)
                {
                    msg.DateRead = DateTime.UtcNow;
                }
            }

            await context.SaveChangesAsync();
            return mapper.Map<IEnumerable<MessageDto>>(messages);


        }



        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}