using ServiceStack.DataAnnotations;

namespace TourOfHeroesAPI.Model
{
    [Alias("Heroes")]
    [ORMF23.Data.Alias("Heroes")]
    public class Hero
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
