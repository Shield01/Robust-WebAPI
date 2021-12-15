using Infrastructure.Link_Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        // GET: api/<RootController>
        [HttpGet(Name = "GetRoot")]
        //public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        //{
        //    if (mediaType.Contains("application/vnd.codemaze.apiroot"))
        //    {
        //        var list = new List<Link>
        //        {
        //            new Link
        //            {
        //                Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new { }),
        //                Rel = "self",
        //                Method = "GET"
        //            },
        //            new Link
        //            {
        //                Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetAllCompanies), new { }),
        //                Rel = "companies",
        //                Method = "GET"
        //            },
        //            new Link
        //            {
        //                Href = _linkGenerator.GetUriByName(HttpContext, nameof(CreateCompany), new { }),
        //                Rel = "create_company",
        //                Method = "POST"
        //            }
        //        };

        //        return Ok();
        //    }

        //    return NoContent();
        //}

        // GET api/<RootController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RootController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RootController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RootController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
