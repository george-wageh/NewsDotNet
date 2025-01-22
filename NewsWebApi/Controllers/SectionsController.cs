using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebApi.Services;
using SharedLib.DTO;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        public SectionsController(SectionsService sectionsService)
        {
            SectionsService = sectionsService;
        }

        public SectionsService SectionsService { get; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectionDTO>>> Get()
        {
            var sections =  await SectionsService.GetAllAsync();
            return Ok(sections);

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] SectionDTO value)
        {
            var response = await SectionsService.AddAsync(value);
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult<object>> Put([FromBody] SectionDTO value)
        {
            var response = await SectionsService.EditAsync(value);
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("[Action]")]
        public async Task<ActionResult<ResponseDTO<object>>> SetUp([FromBody] int sectionId)
        {
            return Ok(await SectionsService.SetUpAsync(sectionId));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("[Action]")]
        public async Task<ActionResult<ResponseDTO<object>>> SetDown([FromBody] int sectionId)
        {
            return Ok(await SectionsService.SetDownAsync(sectionId));
        }
    }
}
