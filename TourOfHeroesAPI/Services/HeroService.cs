//using ServiceStack.Data;
//using ServiceStack.OrmLite;
using ORMF23.Data.Contracts;
using ORMF23.Statics;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TourOfHeroesAPI.Model;

namespace TourOfHeroesAPI.Services
{
    public class HeroService
    {
        private readonly IDbConnection db;
        public HeroService(IDbConnectionFactory connectionFactory)
        {
            db = connectionFactory.OpenDbConnection();
        }

        public async Task<IEnumerable<Hero>> GetHeroes()
        {
            return await db.SelectAsync<Hero>();
        }

        public async Task<IEnumerable<Hero>> GetHerosByName(string name)
        {
            return await db.SelectAsync<Hero>(h => h.Name.Contains(name));
        }

        public async Task<Hero> GetHero(int id)
        {
            return await db.SingleAsync<Hero>(h => h.Id == id);
        }

        public async Task<Hero> Insert(Hero hero)
        {
            if (await db.ExistsAsync<Hero>(h => h.Name == hero.Name) == false)
            {
                var id = await db.InsertAsync(hero, selectIdentity: true);
                return await GetHero((int)id);
            }
            return null;
        }

        public async Task<Hero> Update(Hero hero)
        {
            if (await db.ExistsAsync<Hero>(h => h.Id == hero.Id))
            {
                await db.UpdateAsync(hero);
            }
            return await GetHero(hero.Id);
        }

        public async Task<int> Delete(int id)
        {
            return await db.DeleteAsync<Hero>(h => h.Id == id);
        }
    }
}
