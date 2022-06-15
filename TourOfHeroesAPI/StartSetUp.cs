//using ServiceStack.Data;
using ORMF23.Data.Contracts;
using ORMF23.Statics;
//using ServiceStack.OrmLite;
using System.Data;
using TourOfHeroesAPI.Model;

namespace TourOfHeroesAPI
{
    public class StartSetUp
    {
        private static readonly Hero[] startupData = new[]
        {
            new Hero{ Id = 12, Name = "Dr. Nice" },
            new Hero{ Id = 13, Name = "Bombasto" },
            new Hero{ Id = 14, Name = "Celeritas" },
            new Hero{ Id = 15, Name = "Magneta" },
            new Hero{ Id = 16, Name = "RubberMan" },
            new Hero{ Id = 17, Name = "Dynama" },
            new Hero{ Id = 18, Name = "Dr. IQ" },
            new Hero{ Id = 19, Name = "Magma" },
            new Hero{ Id = 20, Name = "Tornado" }
        };
        private IDbConnection db;

        public StartSetUp(IDbConnectionFactory dbFactory)
        {
            db = dbFactory.OpenDbConnection();
            Run();
        }

        private async void Run()
        {
            await db.DropAndCreateTableAsync<Hero>();
            //db.Save(startupData);
            foreach (var item in startupData)
            {
                await db.InsertAsync(item);
            }
        }
    }
}
