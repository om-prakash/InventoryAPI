using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using $safeprojectname$.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Enums;
using Microsoft.AspNetCore.Authorization;

namespace $safeprojectname$.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILog _logger;

        public WeatherForecastController(ILog logger)
        {
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.Log(LogTypeEnum.Information, "WeatherForecast Test API");
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
