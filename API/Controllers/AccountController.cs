using API.Data;
using API.DTO.AuthDtos;
using API.DTO.PhotoDtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IMapper _mapper;

        public AccountController( DataContext context,ITokenService tokenService,IMapper mapper)
        {
            this.context = context;
            this.tokenService = tokenService;
            this._mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]

        public async Task<ActionResult<RegisterResponseDto>> Register(RegisterRequestDto dto)
        {
            if (await UserExists(dto.UserName)) return BadRequest("User is Taken");
            var user = _mapper.Map<AppUser>(dto);

            using var Hmac = new HMACSHA512();

            

                user.PasswordHash = Hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
               user.PasswordSalt = Hmac.Key;
           

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok(new RegisterResponseDto {
                UserName=user.UserName,
                Token=tokenService.CreateToken(user),
                KnownAs=user.KnownAs
            });
            


        }


        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            AppUser user = await context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x=>x.UserName.ToLower()== dto.UserName.ToLower());

            if (user == null) return Unauthorized("User Not Exist");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            byte[] passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
           
            if (!passwordhash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Wrong Password");
            

            return new LoginResponseDto
            {
                UserName=user.UserName,
                Token=tokenService.CreateToken(user),
                PhotoUrl=user.Photos?.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs=user.KnownAs

            };

        }
           

        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x=>x.UserName.ToLower()==username.ToLower());

        }


    }
}
