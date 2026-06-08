using ApiRefactor.DTO;
using ApiRefactor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ApiRefactor.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaveController : ControllerBase

    {
        private readonly IWaveService _waveService;
        private readonly ILogger<WaveController> _logger;

        public WaveController(IWaveService waveService, ILogger<WaveController> logger)
        {
            _waveService = waveService;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _waveService.GetAllAsync(pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _waveService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] WaveCreateRequestDto request,
            CancellationToken cancellationToken)
        {
            await _waveService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = request.Id },
                request);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] WaveUpdateRequestDto request,
            CancellationToken cancellationToken)
        {
            await _waveService.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }
    }

}

