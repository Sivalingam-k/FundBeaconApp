using FundBeacon.Application.Interfaces;
using FundBeacon.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FundBeacon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScholarshipsController : ControllerBase
    {
        private readonly IScholarshipService _scholarshipService;
        private readonly ILogger<ScholarshipsController> _logger;

        public ScholarshipsController(IScholarshipService scholarshipService, ILogger<ScholarshipsController> logger)
        {
            _scholarshipService = scholarshipService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostScholarship([FromBody] ScholarshipDto dto)
        {
            _logger.LogInformation("POST request to create scholarship received. Title: {Title}", dto.Title);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for scholarship creation.");
                return BadRequest(ModelState);
            }

            try
            {
                var scholarship = await _scholarshipService.CreateScholarshipAsync(dto);
                _logger.LogInformation("Scholarship created successfully. ID: {Id}", scholarship.ScholarshipId);
                return CreatedAtAction(nameof(GetScholarshipById), new { id = scholarship.ScholarshipId }, scholarship);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Scholarship creation failed: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating scholarship.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScholarships()
        {
            _logger.LogInformation("GET request for all scholarships.");

            try
            {
                var scholarships = await _scholarshipService.GetAllScholarshipsAsync();
                _logger.LogInformation("Retrieved {Count} scholarships.", scholarships.Count);
                return Ok(scholarships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving scholarships.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScholarshipById(int id)
        {
            _logger.LogInformation("GET request for scholarship by ID: {Id}", id);

            try
            {
                var scholarship = await _scholarshipService.GetScholarshipByIdAsync(id);
                if (scholarship == null)
                {
                    _logger.LogWarning("Scholarship with ID {Id} not found.", id);
                    return NotFound("Scholarship not found.");
                }

                _logger.LogInformation("Scholarship with ID {Id} retrieved successfully.", id);
                return Ok(scholarship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving scholarship with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScholarship(int id, [FromBody] ScholarshipDto dto)
        {
            _logger.LogInformation("PUT request to update scholarship with ID: {Id}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating scholarship ID: {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _scholarshipService.UpdateScholarshipAsync(id, dto);
                _logger.LogInformation("Scholarship with ID {Id} updated successfully.", id);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Scholarship with ID {Id} not found for update.", id);
                return NotFound("Scholarship not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating scholarship with ID {Id}.", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScholarship(int id)
        {
            _logger.LogInformation("DELETE request for scholarship with ID: {Id}", id);

            try
            {
                var result = await _scholarshipService.DeleteScholarshipAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Scholarship with ID {Id} not found for deletion.", id);
                    return NotFound("Scholarship not found.");
                }

                _logger.LogInformation("Scholarship with ID {Id} deleted (soft delete) successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting scholarship with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
