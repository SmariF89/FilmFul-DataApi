using System.Collections.Generic;

namespace FilmFul_API.Models.Entities
{
    public partial class Director
    {
        public Director()
        {
            Direction = new HashSet<Direction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Direction> Direction { get; set; }
    }
}
