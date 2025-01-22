using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsWebApi.Enums;
using NewsWebApi.Services;
using SharedLib.AdminDTO;
using SharedLib.DTO;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="owner")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        public AdminService AdminService { get; }

        // GET: api/<AdminsController>
        public AdminsController(AdminService adminService)
        {
            AdminService = adminService;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<IEnumerable<AdminModelDTO>>>> GetAll()
        {
            var user = this.User;
            var admin = this.User.IsInRole(RoleTypes.admin.ToString());
            var owner = this.User.IsInRole(RoleTypes.owner.ToString());
            var email = this.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Email).Value;
            var response = await AdminService.GetAllAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<int>>> Post(AdminModelDTO adminModelDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await AdminService.AddAsync(adminModelDTO);
                return Ok(response);
            }
            else {
                return Ok(new ResponseDTO<int> {
                    Message = ModelState.Aggregate("", (s, x) => s + $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}\n")
                });
            }

        }
        [HttpPut]
        public async Task<ActionResult<ResponseDTO<object>>> Put(AdminModelDTO adminModelDTO)
        {
            var response = await AdminService.EditAsync(adminModelDTO);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseDTO<object>>> DeleteAsync(string email)
        {
            var response = await AdminService.RemoveAsync(email);
            return Ok(response);
        }

        [HttpPost("AssignToRole")]
        public async Task<ActionResult<ResponseDTO<object>>> AssignToRole([FromBody] string email)
        {
            var response = await AdminService.AddRoleToUserAsync(email , RoleTypes.admin.ToString());
            return Ok(response);
        }

        [HttpPost("RemoveFromRole")]
        public async Task<ActionResult<ResponseDTO<object>>> RemoveFromRole([FromBody] string email)
        {
            var response = await AdminService.RemoveFromRoleAsync(email, RoleTypes.admin.ToString());
            return Ok(response);
        }

    }
}
