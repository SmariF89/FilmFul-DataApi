using System.Collections.Generic;

namespace FilmFul_API.Models.Entities
{
    public partial class Actor
    {
        public Actor()
        {
            Action = new HashSet<Action>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Action> Action { get; set; }
    }
}
