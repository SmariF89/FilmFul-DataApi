using System;
using System.Collections.Generic;

namespace FilmFul_API.Models.Entities
{
    public partial class Director : IEqualityComparer<Director>
    {
        public Director()
        {
            Direction = new HashSet<Direction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Direction> Direction { get; set; }

        public bool Equals(Director x, Director y)
        {
            //Check if the compared Director objects (x and y) are null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) { return false; }

            //Check if compared Director object (x) refers to the same instance as the other (y).
            if (Object.ReferenceEquals(x, y)) { return true; }

            //Check if the Directors' Ids are the same.
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Director obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
