using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebApi.IRepository;
using NewsWebApi.Services;
using SharedLib.AdminDTO;
using SharedLib.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        public NewsController(NewsService NewsService)
        {
            this.NewsService = NewsService;
        }

        public NewsService NewsService { get; }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<NewDetailsDTO>>> Get(int id)
        {
            return await NewsService.GetAsync(id);
        }

        // POST api/<NewsController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResponseDTO<int>>> Add([FromBody] NewAddDTO newAddDTO)
        {
            if (ModelState.IsValid)
            {
                return Ok(await NewsService.AddAsync(newAddDTO));
            }
            else {
                return Ok(
                       new ResponseDTO<int>
                       {
                           Message = ModelState.ToString(),
                           Success = false
                       }
                    );
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("UploadImage")]
        public async Task<ActionResult<ResponseDTO<string>>> UploadImage()
        {
            var file = this.Request.Form.Files[0];
            return Ok(await NewsService.UploadImage(file));
        }
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult<ResponseDTO<int>>> Edit([FromBody] NewAddDTO newAddDTO)
        {
            if (ModelState.IsValid)
            {
                return Ok(await NewsService.EditAsync(newAddDTO));
            }
            else
            {
                return Ok(
                       new ResponseDTO<int>
                       {
                           Message = ModelState.ToString(),
                           Success = false
                       }
                    );
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<ActionResult<ResponseDTO<object>>> Delete(int newsId)
        {
            return Ok(await NewsService.DeleteAsync(newsId));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("AdminQuery")]
        public async Task<ActionResult<ResponseListNewsAdminDTO>> GetInAdminQuery([FromBody] NewsQueryDTO newsQueryDTO)
        {
            var response = await NewsService.GetAllAdmin(newsQueryDTO);
            return Ok(response);
        }

        [HttpGet("GetInSection/{Id:int}")]
        public async Task<ActionResult<ResponseDTO<IEnumerable<NewDTO>>>> GetAllInSection([FromRoute] int Id)
        {
            var response = await NewsService.GetAllInSection(Id);
            return Ok(response);
        }
        [HttpPost("UserQuery")]
        public async Task<ActionResult<ResponseListNewsDTO>> GetInUserQuery([FromBody] NewsQueryDTO newsQueryDTO)
        {
            var response = await NewsService.GetAllUser(newsQueryDTO);
            return Ok(response);
        }
        //// PUT api/<NewsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<NewsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
