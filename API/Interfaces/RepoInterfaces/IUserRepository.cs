﻿using API.DTO.MemberDtos;
using API.Entities;
using API.Helpers;
using API.Helpers.QueryParams;
using System.Collections.Generic;

namespace API.Interfaces.RepoInterfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        bool UserExists(int id);
        Task<PagedList<AppUser>> GetAllUserAsync(UserQueryParams prms);

        Task<AppUser> GetUserByIdAsync(int Id);

        Task<AppUser> GetUserByUserNameAsync(string UserName);

        void DeleteUserAsync(AppUser user);

    }
}