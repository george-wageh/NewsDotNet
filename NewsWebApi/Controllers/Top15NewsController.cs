using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebApi.Services;
using SharedLib.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class Top15NewsController : ControllerBase
    {
        // GET: api/<Top15NewsController>
        public Top15NewsController(Top15NewsService top15NewsService)
        {
            Top15NewsService = top15NewsService;
        }

        public Top15NewsService Top15NewsService { get; }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<IEnumerable<NewDTO>>>> GetAll()
        {
            return Ok(await Top15NewsService.GetAllAsync());
        }
        [HttpPost("[Action]")]
        public async Task<ActionResult<ResponseDTO<object>>> SetUp([FromBody] int newsId)
        {
            return Ok(await Top15NewsService.SetUpAsync(newsId));
        }
        [HttpPost("[Action]")]
        public async Task<ActionResult<ResponseDTO<object>>> SetDown([FromBody] int newsId)
        {
            return Ok(await Top15NewsService.SetDownAsync(newsId));
        }
        [HttpPost("[Action]")]
        public async Task<ActionResult<ResponseDTO<object>>> Push([FromBody] int newsId)
        {
            return Ok(await Top15NewsService.AddAsync(newsId));
        }
        [HttpDelete]
        public async Task<ActionResult<ResponseDTO<object>>> Delete(int newsId)
        {
            return Ok(await Top15NewsService.DeleteAsync(newsId));
        }
        //// GET api/<Top15NewsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<Top15NewsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<Top15NewsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<Top15NewsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
