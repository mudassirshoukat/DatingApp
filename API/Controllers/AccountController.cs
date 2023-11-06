using API.Data;
using API.DTO.AuthDtos;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController( DataContext context,ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost]
        [Route("Register")]

        public async Task<ActionResult<AppUser>> Register(RegisterRequestDto dto)
        {
            if (await UserExists(dto.UserName)) return BadRequest("User is Taken");

            using var Hmac = new HMACSHA512();

            AppUser user = new AppUser
            {
                UserName = dto.UserName,
                PasswordHash =Hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt=Hmac.Key
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok(user);


        }


        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            AppUser user = await context.Users.FirstOrDefaultAsync(x=>x.UserName.ToLower()== dto.UserName.ToLower());

            if (user == null) return Unauthorized("User Not Exist");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            byte[] passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
           
            if (!passwordhash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Wrong Password");
            

            return new LoginResponseDto
            {
                UserName=user.UserName,
                Token=tokenService.CreateToken(user)

            };

        }


        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x=>x.UserName.ToLower()==username.ToLower());

        }


    }
}
