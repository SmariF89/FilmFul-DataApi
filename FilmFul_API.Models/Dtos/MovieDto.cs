namespace FilmFul_API.Models.Dtos
{
    public class MovieDto
    {
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
    }
}