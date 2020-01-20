using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Example.Library;
using XPikeIoC22;

namespace XPikeIoC22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly string[] Summaries;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEnumerable<IQuoteProvider> quoteProviders)
        {
            _logger = logger;
            Summaries = quoteProviders.SelectMany(x => x.GetQuotes()).ToArray();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            return Enumerable.Range(1, Summaries.Length).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[index - 1]
            })
            .ToArray();
        }
    }
}