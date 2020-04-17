using System;
using System.Collections.Generic;

namespace FilmFul_API.Models.Entities
{
    public partial class Movie : IEqualityComparer<Movie>
    {
        public Movie()
        {
            Action = new HashSet<Action>();
            Direction = new HashSet<Direction>();
            Genre = new HashSet<Genre>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Poster { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public float RatingImdb { get; set; }
        public int? RatingMetascore { get; set; }
        public string Certificate { get; set; }
        public float? Gross { get; set; }
        public int VoteCount { get; set; }

        public virtual ICollection<Action> Action { get; set; }
        public virtual ICollection<Direction> Direction { get; set; }
        public virtual ICollection<Genre> Genre { get; set; }

        public bool Equals(Movie x, Movie y)
        {
            //Check if the compared Movie objects (x and y) are null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) { return false; }

            //Check if compared Movie object (x) refers to the same instance as the other (y).
            if (Object.ReferenceEquals(x, y)) { return true; }

            //Check if the Movies' properties are the same.
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Movie obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
