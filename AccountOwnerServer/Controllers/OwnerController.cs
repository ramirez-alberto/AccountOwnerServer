using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Contracts;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OwnerController(IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllOwners()
        //{
        //    try
        //    {
        //        var owners = await _repository.Owner.GetAllOwnersAsync();
        //        _logger.LogInfo($"Returned all owners from database.");
        //        var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
        //        return Ok(owners);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"There was an error in GetAllOwner action: {ex.Message}");
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetOwners([FromQuery] OwnerParameters ownerParameters)
        {
            if (!ownerParameters.isValidDate())
                return BadRequest("Max year of birth cannot be less than min year of birth");
            try
            {
                var owners = await _repository.Owner.GetOwnersAsync(ownerParameters);
                var metadata = new
                {
                    owners.TotalCount,
                    owners.PageSize,
                    owners.CurrentPage,
                    owners.TotalPages,
                    owners.HasNext,
                    owners.HasPrevious
                };

                _logger.LogInfo($"Returned all owners from database.");
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
                return Ok(ownersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error in GetAllOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public async Task<IActionResult> GetOwnerById(Guid Id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(Id);
            if (owner == null)
            {
                _logger.LogError($"Cannot find owner with id: {Id}");
                return NotFound();
            }

            var ownerResult = _mapper.Map<OwnerDto>(owner);

            return Ok(ownerResult);
        }

        [HttpGet("{id}/account")]
        public async Task<IActionResult> GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = await _repository.Owner.GetOwnerWithDetailsAsync(id);
                if (owner == null)
                {
                    _logger.LogError($"Cannot find owner with id: {id}");
                    return NotFound();
                }

                var ownerResult = _mapper.Map<OwnerDto>(owner);

                return Ok(ownerResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error in GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerForCreateDto owner)
        {
            try
            {
                if (owner == null)
                {
                    _logger.LogError("Owner object sent by the client is null.");
                    return BadRequest("Owner model is null.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Owner model sent by client is invalid");
                    return BadRequest("Invalid model Owner.");
                }

                var ownerEntity = _mapper.Map<Owner>(owner);
                _repository.Owner.CreateOwner(ownerEntity);
                await _repository.SaveAsync();

                var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

                return CreatedAtRoute("OwnerById", new { Id = createdOwner.Id }, createdOwner);

            }
            catch (Exception ex)
            {

                _logger.LogError($"There was an error in GetAllOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner([FromBody] OwnerForUpdateDto owner,Guid id)
        {
            try
            {
                if (owner == null)
                {
                    _logger.LogError("Owner object sent by the client is null.");
                    return BadRequest("Owner model is null.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Owner model sent by client is invalid");
                    return BadRequest("Invalid model Owner.");
                }

                var ownerEntity = await _repository.Owner.GetOwnerByIdAsync(id);
                if(ownerEntity == null)
                {
                    _logger.LogError($"Cannot find owner with id: {id}");
                    return NotFound();
                }

                _mapper.Map(owner, ownerEntity);
                _repository.Owner.UpdateOwner(ownerEntity);
                await _repository.SaveAsync();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error in GetAllOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
