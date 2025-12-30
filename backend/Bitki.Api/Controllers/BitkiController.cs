using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.DTOs;
using Bitki.Core.Interfaces.Repositories;
using Bitki.Core.Interfaces.Services;
using Bitki.Core.Models;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitkiController : ControllerBase
    {
        private readonly IBitkiRepository _repository;
        private readonly ICacheService _cache;
        private const string CacheKeyPrefix = "bitki:detail:";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public BitkiController(IBitkiRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> Get()
        {
            var plants = await _repository.GetAllAsync();
            return Ok(plants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetById(int id)
        {
            var plant = await _repository.GetByIdAsync(id);
            if (plant == null)
                return NotFound();
            return Ok(plant);
        }

        /// <summary>
        /// Get full details of a plant including all fields and related data (compounds, images, literature)
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<ActionResult<BitkiDetailDto>> GetDetails(int id)
        {
            var details = await _repository.GetDetailByIdAsync(id);
            if (details == null)
                return NotFound();
            return Ok(details);
        }

        /// <summary>
        /// Get basic details of a plant (cached, no related data - for lazy loading)
        /// </summary>
        [HttpGet("{id}/details/basic")]
        public async Task<ActionResult<BitkiDetailDto>> GetBasicDetails(int id)
        {
            var cacheKey = $"{CacheKeyPrefix}{id}";

            // Try to get from cache first
            var cached = _cache.Get<BitkiDetailDto>(cacheKey);
            if (cached != null)
                return Ok(cached);

            // Fetch from database
            var details = await _repository.GetBasicDetailByIdAsync(id);
            if (details == null)
                return NotFound();

            // Cache the result
            _cache.Set(cacheKey, details, CacheDuration);

            return Ok(details);
        }

        /// <summary>
        /// Get compounds for a plant (lazy loading)
        /// </summary>
        [HttpGet("{id}/compounds")]
        public async Task<ActionResult<IEnumerable<PlantCompoundDto>>> GetCompounds(int id)
        {
            var compounds = await _repository.GetCompoundsByIdAsync(id);
            return Ok(compounds);
        }

        /// <summary>
        /// Get images for a plant (lazy loading)
        /// </summary>
        [HttpGet("{id}/images")]
        public async Task<ActionResult<IEnumerable<PlantImageDto>>> GetImages(int id)
        {
            var images = await _repository.GetImagesByIdAsync(id);
            return Ok(images);
        }

        /// <summary>
        /// Get literature for a plant (lazy loading)
        /// </summary>
        [HttpGet("{id}/literature")]
        public async Task<ActionResult<IEnumerable<PlantLiteratureDto>>> GetLiterature(int id)
        {
            var literature = await _repository.GetLiteratureByIdAsync(id);
            return Ok(literature);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create([FromBody] Plant plant)
        {
            var id = await _repository.AddAsync(plant);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Plant plant)
        {
            plant.Id = id;
            await _repository.UpdateAsync(plant);

            // Invalidate cache
            _cache.Remove($"{CacheKeyPrefix}{id}");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);

            // Invalidate cache
            _cache.Remove($"{CacheKeyPrefix}{id}");

            return NoContent();
        }

        [HttpPost("query")]
        public async Task<ActionResult<FilterResponse<Plant>>> Query([FromBody] FilterRequest request)
        {
            try
            {
                var result = await _repository.QueryAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Bitki Query Error: {ex.Message}");
                Console.WriteLine($"[DEBUG] StackTrace: {ex.StackTrace}");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
