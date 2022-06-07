using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourOfHeroesAPI.Model;
using TourOfHeroesAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TourOfHeroesAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly HeroService _heroService;
        public HeroController(HeroService heroService)
        {
            _heroService = heroService;
        }
        // GET: api/<HeroController>
        [HttpGet]
        public async Task<IEnumerable<Hero>> Get()
        {
            return await _heroService.GetHeroes();
        }

        //GET<HeroController>/5
        [HttpGet("{id}")]
        public async Task<Hero> Get(int id)
        {
            return await _heroService.GetHero(id);
        }

        [HttpGet("name/{name}")]
        public async Task<IEnumerable<Hero>> Get(string name)
        {
            return await _heroService.GetHerosByName(name);
        }

        [EnableCors]
        [HttpPost]
        public async Task<Hero> Post([FromBody] Hero hero)
        {
            return await _heroService.Insert(hero);
        }

        [EnableCors]
        [HttpPut]
        public async Task<Hero> Put([FromBody] Hero hero)
        {
            return await _heroService.Update(hero);
        }

        [EnableCors]
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var result = await _heroService.Delete(id);
            return result;
        }
    }
}
