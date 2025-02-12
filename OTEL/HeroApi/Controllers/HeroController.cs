using HeroApi.Metrics;
using HeroApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HeroApi.Controllers;
[ApiController]
[Route("[controller]")]
public class HeroController : ControllerBase
{
    private static readonly List<HeroViewModel> _heros = new List<HeroViewModel>
    {
        new  HeroViewModel{ Id = 1, Name = "Ronald", SuperPower = "Coding"}
    };

    private readonly ILogger<HeroController> _logger;
    private readonly HeroTelemetry _heroTelemetry;
    private readonly HeroMetrics _meters;

    public HeroController(ILogger<HeroController> logger, HeroTelemetry heroTelemetry, HeroMetrics meters)
    {
        _logger = logger;
        _heroTelemetry = heroTelemetry;
        _meters = meters;
    }

    [HttpGet()]
    public IActionResult Get()
    {
        _meters.ReadHeros();
        return Ok(_heros);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var hero = _heros.FirstOrDefault(e => e.Id == id);
        if (hero is null)
        {
            return NotFound();
        }
        _meters.ReadHero();
        return Ok(hero);
    }

    [HttpPost()]
    public IActionResult Post(PostHeroViewModel heroVM)
    {
        using (var activity = _heroTelemetry.ActivitySource.StartActivity("Create new hero"))
        {
            var id = _heros.Count() > 0 ? _heros.Max(e => e.Id) + 1 : 1;
            var hero = new HeroViewModel
            {
                Id = id,
                Name = heroVM.Name,
                SuperPower = heroVM.SuperPower
            };

            _heros.Add(hero);
            _meters.AddHero();

            activity.AddTag("hero.id", hero.Id);
            activity.AddTag("hero.name", hero.Name);
            activity.AddTag("hero.superPower", hero.SuperPower);

            return Created(hero.Id.ToString(), hero);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, PostHeroViewModel heroVM)
    {
        using (var activity = _heroTelemetry.ActivitySource.StartActivity("Changing hero"))
        {
            var hero = _heros.FirstOrDefault(e => e.Id == id);
            if (hero is null)
            {
                return NotFound();
            }

            hero.Name = heroVM.Name;
            hero.SuperPower = heroVM.SuperPower;

            _meters.UpdateHero();
            activity.AddTag("hero.id", hero.Id);
            activity.AddTag("hero.name", hero.Name);
            activity.AddTag("hero.superPower", hero.SuperPower);

            return Ok(hero);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        using (var activity = _heroTelemetry.ActivitySource.StartActivity("Killing hero"))
        {
            int index = _heros.FindIndex(e => e.Id == id);
            if (index >= 0)
            {
                _heros.RemoveAt(index);
                _meters.DeleteHero();
                activity.AddTag("hero.id", id);
            }
            else {
                activity.AddTag("error.message", "hero not found");
                activity.SetStatus(ActivityStatusCode.Error);
            }

            return Ok();
        }
    }
}
