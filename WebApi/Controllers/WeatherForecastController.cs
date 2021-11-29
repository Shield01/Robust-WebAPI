using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using LogService.Abstractions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private ILogImplementations _logger;

        public WeatherForecastController(ILogImplementations logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<string> Get(string message)
        //{
        //    _logger.DebugMessage("This is a wwarning message from the log");

        //    _logger.InfoMessage("This is an information from the log");

        //    _logger.WarningMessage("This is a warning message from the log");

        //    _logger.ErrorMessage("This is an error message from the log");

        //    return new string[] { "Just", "Checking" };
        //}


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
