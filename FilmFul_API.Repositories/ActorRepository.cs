using System.Collections.Generic;
using FilmFul_API.Models.Entities;
using System.Linq;
using FilmFul_API.Repositories.Extensions;

namespace FilmFul_API.Repositories
{
    public class ActorRepository
    {
        private readonly FilmFulDbContext filmFulDbContext = new FilmFulDbContext();

        public (IEnumerable<Actor>, int) GetAllActors(int pageSize, int pageIndex)
        {
            int rangeOkay = Utilities.checkRange(pageSize, pageIndex, filmFulDbContext.Actor.Count());

            if(rangeOkay == 0)
            {
                return ((from a in filmFulDbContext.Actor 
                        select a).Skip(pageIndex * pageSize)
                            .Take(pageSize), rangeOkay);
            }
            else
            {
                return (null, rangeOkay);
            }
        }

        public Actor GetActorById(int id)
        {
            return filmFulDbContext.Actor.Where(a => a.Id == id).SingleOrDefault();
        }
    }
}