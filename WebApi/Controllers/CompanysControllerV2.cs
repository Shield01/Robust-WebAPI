using Infrastructure.Query_Features;
using Infrastructure.Repository_Manager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/{v:apiversion}/companies")]
    [ApiController]
    public class CompanysControllerV2 : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        public CompanysControllerV2(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        // GET: api/<CompanysControllerV2>
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies([FromQuery]CompanyParameter companyParameter)
        {
            var value = await _repositoryManager.Company.FindAllCompanies(companyParameter, false);

            if(value == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(value);
            }
        }

        // GET api/<CompanysControllerV2>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CompanysControllerV2>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CompanysControllerV2>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CompanysControllerV2>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
