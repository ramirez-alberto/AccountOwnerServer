using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contracts;
using AutoMapper;
using Entities.DataTransferObjects;

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
        public IActionResult GetAllOwners()
        {
            try
            {
                var owners = _repository.Owner.GetAllOwners();
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
    }
}
