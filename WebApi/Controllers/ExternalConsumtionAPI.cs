using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApi.External_Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalConsumtionAPI : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalConsumtionAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/<ExternalConsumtionAPI>
        [HttpGet]
        public async Task<IActionResult>GetMetaWeather()
        {
            MetaWeather metaWeather;

            string errorString;

            var client = _httpClientFactory.CreateClient("meta");

            try
            {
                metaWeather = await client.GetFromJsonAsync<MetaWeather>("location/44418/");

                errorString = null;

                return Ok(metaWeather);
            }
            catch (Exception ex)
            {
                errorString = $"There was an error getting the fprecast{ex.Message}";

                return BadRequest(errorString);
            }
        }

        // GET api/<ExternalConsumtionAPI>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExternalConsumtionAPI>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExternalConsumtionAPI>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExternalConsumtionAPI>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
