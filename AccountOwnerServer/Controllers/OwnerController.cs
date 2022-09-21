using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            try
            {
                var owners = await _repository.Owner.GetAllOwnersAsync();
                _logger.LogInfo($"Returned all owners from database.");
                var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
                return Ok(owners);
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
    }
}
