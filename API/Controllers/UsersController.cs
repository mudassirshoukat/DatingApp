using API.Data;
using API.DTO.MemberDtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _repo;
        private readonly IMapper mapper;

        public UsersController(IUserRepository repo,IMapper  mapper)
        {
            this._repo = repo;
            this.mapper = mapper;
        }

        
        
        // GET: api/Users
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<MemberResponseDto>>> GetUsers()
        {
            var users = await _repo.GetAllUserAsync();
            var dto=mapper.Map<IEnumerable<MemberResponseDto>>(users);
            return Ok(dto);
        }

        // GET: api/Users/5
        [HttpGet()]
        [Route("{id:int}")]
        public async Task<ActionResult<MemberResponseDto>> GetUserById(int id)
        {
            var appUser = await _repo.GetUserByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<MemberResponseDto>(appUser));
        }


        [HttpGet()]
        [Route("{UserName}")]
        public async Task<ActionResult<MemberResponseDto>> GetUserByUserName(string UserName)
        {
            var appUser = await _repo.GetUserByUserNameAsync(UserName);

            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<MemberResponseDto>(appUser));
        }


        // PUT: api/Users/5
        [HttpPut()]
        
        public async Task<IActionResult> PutUser(MemberUpdateRequestDto member)
        {
            string userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            AppUser user =await _repo.GetUserByUserNameAsync(userName);
            mapper.Map(member, user);
            if (await _repo.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update resource");

        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<AppUser>> PostUser([FromBody] AppUser appUser)
        //{
        //    _context.Users.Add(appUser);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAppUser", new { id = appUser.id }, appUser);
        //}

        // DELETE: api/Users/5
        [HttpDelete()]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var appUser = await _repo.GetUserByIdAsync(id);
            if (appUser == null)
            {
                
                return NotFound();
            }

            _repo.DeleteUserAsync(appUser);
            await _repo.SaveAllAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return  _repo.UserExists(id);
        }
    }
}
