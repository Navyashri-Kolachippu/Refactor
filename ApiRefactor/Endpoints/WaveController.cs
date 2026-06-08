using ApiRefactor.DTO;
using ApiRefactor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ApiRefactor.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _waveService.GetAllAsync(pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _waveService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "CanWriteWaves")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [Authorize(Policy = "CanWriteWaves")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

