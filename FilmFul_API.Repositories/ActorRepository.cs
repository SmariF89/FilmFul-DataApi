using System.Collections.Generic;
using FilmFul_API.Models.Entities;
using System.Linq;

namespace FilmFul_API.Repositories
{
    public class ActorRepository
    {
        private readonly FilmFulDbContext filmFulDbContext = new FilmFulDbContext();
    
        public IEnumerable<Actor> GetAllActors()
        {
            return from a in filmFulDbContext.Actor select a;
        }

        public Actor GetActorById(int id)
        {
            return filmFulDbContext.Actor.Where(a => a.Id == id).SingleOrDefault();
        }
    }
}