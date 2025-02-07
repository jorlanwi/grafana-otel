using HeroApi.Metrics;
using HeroApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
    private readonly HeroMetrics _meters;

    public HeroController(ILogger<HeroController> logger, HeroMetrics meters)
    {
        _logger = logger;
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
        var hero = new HeroViewModel
        {
            Id = _heros.Max(e => e.Id),
            Name = heroVM.Name,
            SuperPower = heroVM.SuperPower
        };

        _heros.Add(hero);
        _meters.AddHero();
        return Created(hero.Id.ToString(), heroVM);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, PostHeroViewModel heroVM)
    {
        var hero = _heros.FirstOrDefault(e => e.Id == id);
        if (hero is null)
        {
            return NotFound();
        }

        hero.Name = heroVM.Name;
        hero.SuperPower = heroVM.SuperPower;

        _meters.UpdateHero();
        return Ok(hero);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        int index = _heros.FindIndex(e => e.Id == id);
        if (index >= 0)
        {
            _heros.RemoveAt(index);
        }

        _meters.DeleteHero();
        return Ok();
    }
}
