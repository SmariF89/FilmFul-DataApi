using System.Collections.Generic;
using System.Linq;
using FilmFul_API.Repositories.Extensions;
using FilmFul_API.Models.Dtos;

namespace FilmFul_API.Repositories
{
    public class ActorRepository
    {
        private readonly FilmFulDbContext filmFulDbContext = new FilmFulDbContext();

        public (IEnumerable<ActorDto>, int) GetAllActors(int pageSize, int pageIndex)
        {
            int rangeOkay = Utilities.checkRange(pageSize, pageIndex, filmFulDbContext.Actor.Count());
            
            if(rangeOkay == 0)
            {
                return (
                            DataTypeConversionUtils.ActorToActorDto
                            (
                                ((from a in filmFulDbContext.Actor 
                                select a)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                )
                            ), rangeOkay
                        );
            }
            else
            {
                return (null, rangeOkay);
            }
        }

        public ActorDto GetActorById(int id)
        {
            return DataTypeConversionUtils.ActorToActorDto
                (
                    filmFulDbContext.Actor
                        .Where(a => a.Id == id)
                        .SingleOrDefault()
                );
        }

        public MovieDto GetActorMoviesByActorId(int id)
        {
            // This query returns all movies which actor with Id == id stars in.
            var query = (from actor in filmFulDbContext.Actor
                         where actor.Id == id
                         join action in filmFulDbContext.Action on actor.Id equals action.ActorId
                         join movie in filmFulDbContext.Movie on action.MovieId equals movie.Id
                         select new { ActorFilms = movie.Title });

            foreach (var film in query) { System.Console.WriteLine("MOVIES: " + film.ActorFilms); }

            return null;
        }
    }
}