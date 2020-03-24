using System.Collections.Generic;

namespace FilmFul_API.Models.Entities
{
    public partial class Movie
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
    }
}
