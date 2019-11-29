using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataLayerAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.DataModels;

namespace CoreAPI.Controllers
{
    [Authorize]
    [Route("Data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ICosmosManager _cosmosManager;

        public DataController(ICosmosManager cosmosManager)
        {
            _cosmosManager = cosmosManager;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _cosmosManager.GetUsers();
            return Ok(users) ;
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _cosmosManager.GetPersonById(id);
            return Ok(user);
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
        public async Task<IActionResult> Put(string id, [FromBody] Person person)
        {
            var response = await _cosmosManager.UpdatePersonsData(id, person);
            if (response == HttpStatusCode.OK)
            {
                return Ok();
            }
            return BadRequest("Unable to update the person");
        }
       
    }
}
