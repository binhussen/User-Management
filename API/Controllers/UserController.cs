using AutoMapper;
using Contracts.Interfaces;
using Contracts.Services;
using Entities;
using Entities.DTOs;
using Entities.Models;
using Entities.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public UserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] RequestParameters requestParameters)
        {
            var userFromDb = await _repository.User.GetUsersAsync(requestParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(userFromDb.MetaData));

            var userDto = _mapper.Map<IEnumerable<UserDto>>(userFromDb);
            return Ok(userDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser( int id)
        {
            var user = await _repository.User.GetUserAsync(id, trackChanges: false);
            if (user == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserForCreationDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogError("UserForCreationDto object sent from client is null.");
                return BadRequest("UserItemForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserItemForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var user = _mapper.Map<User>(userDto);
            _repository.User.CreateUser(user);

            await _repository.SaveAsync();

            return NoContent();

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogError("UserForUpdateDto object sent from client is null.");
                return BadRequest("UserForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserItemForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var user = await _repository.User.GetUserAsync(id, trackChanges: false);
            if (userDto == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(userDto, user);
            await _repository.SaveAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _repository.User.GetUserAsync(id, trackChanges: false);
            if (user == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.User.DeleteUser(user);
            await _repository.SaveAsync();

            return NoContent();
        }

    }
}

