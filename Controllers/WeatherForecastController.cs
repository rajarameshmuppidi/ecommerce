using EcommercePlatform.Models;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;

namespace EcommercePlatform.Controllers
{

    [ApiController]
    [Route("[controller]")]
    
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet("signalRDemo")]
        public async Task<IActionResult> getDataDemo()
        {
            //var hub = new OrderHub();
            //var hub = app.Services.GetRequiredService<IHubContext<OrderHub>>(

            await _hubContext.Clients.All.SendAsync("UpdateOrder", 1, 2);

            return Ok(new { status = "Ok" });
        }

        [Authorize(Roles = "User")]
        [HttpGet("User")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Authorize(Roles ="Seller")]
        [HttpGet("Seller")]
        public string SellerResult()
        {
            return "Seller Information";
        }
    }
}
