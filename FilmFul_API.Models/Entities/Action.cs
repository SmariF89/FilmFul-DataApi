namespace FilmFul_API.Models.Entities
{
    public partial class Action
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }

        public virtual Actor Actor { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
