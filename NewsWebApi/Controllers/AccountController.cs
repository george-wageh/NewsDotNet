using Microsoft.AspNetCore.Mvc;
using NewsWebApi.IRepository;
using NewsWebApi.Repository;
using NewsWebApi.Services;
using SharedLib.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        public AccountController(AccountService accountService)
        {
            AccountService = accountService;
        }

        public AccountService AccountService { get; }

        [HttpPost("SendPasswordToMail")]
        public async Task<ActionResult<ResponseDTO<string>>> SendPasswordToMail([FromBody]string email)
        {
            return Ok(await AccountService.SendPasswordToMail(email));
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ResponseDTO<string>>> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            return Ok(await AccountService.Login(userLoginDTO));
        }

    }
}
