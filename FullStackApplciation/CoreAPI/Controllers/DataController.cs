using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CoreAPI.Helpers;
using DataLayerAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.DataModels;
using ModelsLibrary.DTOS;

namespace CoreAPI.Controllers
{
    [ServiceFilter(typeof(LogPersonsActivity))]
    [Authorize]
    [Route("Data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ICosmosManager _cosmosManager;
        private readonly IMapper _mapper;

        public DataController(ICosmosManager cosmosManager, IMapper mapper)
        {
            _cosmosManager = cosmosManager;
            _mapper = mapper;
        }


        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _cosmosManager.GetUsers();

            // var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            var usersToReturn = Helpers.HelperMapper.MapUserToUserForListDto(users);

            return Ok(usersToReturn) ;
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _cosmosManager.GetPersonById(id);
            if (user == null)
            {
                return BadRequest("User with such Id doesn't exist");
            }

            // var userToReturn = _mapper.Map<UserForDetailDto>(user);
            var userToReturn = Helpers.HelperMapper.MapUserToUserForDetailDto(user);

            return Ok(userToReturn);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> CreatePerson ([FromBody] Person person)
        {
            var response = await _cosmosManager.AddPersonData(person);
            if (response == HttpStatusCode.Created)
            {
                return Ok();
            }
            return BadRequest("Unable to create the person");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserForUpdateDtos user)
        {
            // check if the user is authorized
            if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            var currentPerson = await _cosmosManager.GetPersonById(id);
            var person = Helpers.HelperMapper.MapUserToUserForUpdateDto(currentPerson, user);
            var response = await _cosmosManager.UpdatePersonsData(id, person);
            if (response == HttpStatusCode.OK)
            {
                return NoContent();
            }
            return BadRequest("Unable to update the person");
        }


       
    }
}
