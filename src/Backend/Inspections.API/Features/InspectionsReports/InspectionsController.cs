using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inspections.API.Features.Inspections
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionsController : ControllerBase
    {
        // GET: api/<InspectionsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value3" };
        }

        // GET api/<InspectionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InspectionsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InspectionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InspectionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
