using Microsoft.AspNetCore.Mvc;
namespace ApiRefactor.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaveController : ControllerBase

    {
        [HttpGet()]
        [Route("GetAllWaves")]
        public async Task<ActionResult<string>>GetAll()
        {
            return Ok("yes");
        }

    }
}
