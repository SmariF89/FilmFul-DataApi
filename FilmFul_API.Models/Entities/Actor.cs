using System;
using System.Collections.Generic;

namespace FilmFul_API.Models.Entities
{
    public partial class Actor : IEqualityComparer<Actor>
    {
        public Actor()
        {
            Action = new HashSet<Action>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Action> Action { get; set; }

        public bool Equals(Actor x, Actor y)
        {
            //Check if the compared Actor objects (x and y) are null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) { return false; }

            //Check if compared Actor object (x) refers to the same instance as the other (y).
            if (Object.ReferenceEquals(x, y)) { return true; }

            //Check if the Actors' Ids are the same.
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Actor obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
