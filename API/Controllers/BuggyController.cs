using API.Data;
using API.DTO.UserDtos;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            this._context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret Text";
        }


        
        [HttpGet("not-found")]
        public ActionResult<UserResponseDto?> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing==null)
            {
                return NotFound();
            }
            return new  UserResponseDto{ UserName =thing.UserName};
        }




        
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            // we potentialy genrating null exeption here
            var thingToReturn = thing.ToString();
            return thingToReturn;  
        }



        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
           ;
            return BadRequest("This Was not a Good Request");
        }


    }
}
