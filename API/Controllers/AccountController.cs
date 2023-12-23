using API.Data;
using API.DTO.AuthDtos;
using API.DTO.PhotoDtos;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
   
    public class AccountController : BaseApiController
    {
        
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper _mapper;

        public AccountController( UserManager<AppUser> userManager, ITokenService tokenService,IMapper mapper)
        {
            
            this.userManager = userManager;
            this.tokenService = tokenService;
            this._mapper = mapper;
        }


        [HttpPost]
        [Route("Register")]

        public async Task<ActionResult<RegisterResponseDto>> Register(RegisterRequestDto dto)
        {
            if (await UserExists(dto.UserName)) return BadRequest("User is Taken");
            var user = _mapper.Map<AppUser>(dto);
           

           var result= await userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok(new RegisterResponseDto {
                UserName=user.UserName,
                Token= await tokenService.CreateToken(user),
                KnownAs=user.KnownAs,
                Gender=user.Gender
                
            });
            


        }

      
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            AppUser user = await userManager.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x=>x.UserName.ToLower()== dto.UserName.ToLower());

            if (user == null) return Unauthorized("User Not Exist");
            var result = await userManager.CheckPasswordAsync(user,dto.Password);

            if (!result) return Unauthorized("Wrong Password");
            
          
           
            

            return new LoginResponseDto
            {
                UserName=user.UserName,
                Token= await tokenService.CreateToken(user),
                PhotoUrl=user.Photos?.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs=user.KnownAs,
                Gender=user.Gender

            };

        }
           

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x=>x.UserName.ToLower()==username.ToLower());

        }


    }
}
