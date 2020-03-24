namespace FilmFul_API.Models.Entities
{
    public partial class Genre
    {
        public int MovieId { get; set; }
        public string Genre1 { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
