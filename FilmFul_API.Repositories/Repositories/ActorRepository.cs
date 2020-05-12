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
            
            if(rangeOkay == Utilities.ok)
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
            else { return (null, rangeOkay); }
        }

        public ActorDto GetActorById(int id)
        {
            var actorById = filmFulDbContext.Actor
                                .Where(a => a.Id == id)
                                .SingleOrDefault();

            return actorById == null ? null : DataTypeConversionUtils.ActorToActorDto(actorById);
        }

        public IEnumerable<DirectorDto> GetActorDirectorsByActorId(int id)
        {
            var actorDirectors = (from director in filmFulDbContext.Director
                                    join direction in filmFulDbContext.Direction on director.Id equals direction.DirectorId
                                        join movie in filmFulDbContext.Movie on direction.MovieId equals movie.Id
                                            join action in filmFulDbContext.Action on movie.Id equals action.MovieId
                                                join actor in filmFulDbContext.Actor on action.ActorId equals actor.Id
                                                where actor.Id == id
                                                select director)
                                                    .Distinct()
                                                    .OrderBy(di => di.Name);

            return (actorDirectors == null || !actorDirectors.Any()) ? null : DataTypeConversionUtils.DirectorToDirectorDto(actorDirectors);
        }

        public (IEnumerable<MovieDto>, int) GetActorMoviesByActorId(int id, List<string> genres)
        {
            var actorMovies = (from actor in filmFulDbContext.Actor
                                   join action in filmFulDbContext.Action on actor.Id equals action.ActorId
                                       join movie in filmFulDbContext.Movie on action.MovieId equals movie.Id
                                           join genre in filmFulDbContext.Genre on movie.Id equals genre.MovieId
                                           where actor.Id == id
                                           select new  { movie, genre }
                              ).ToList();

            var actorMoviesWithGenres = actorMovies
                                            .Select(m => m.movie)
                                            .Distinct();

            return (actorMovies == null || !actorMovies.Any()) ?
                       (null, Utilities.notFound) :                                                                         // Actor has not starred in any film.
                       (
                           (DataTypeConversionUtils.MovieToMovieDto(genres != null ?
                               actorMoviesWithGenres.Where(m => !genres.Except(m.Genre.Select(g => g.Genre1)).Any()) :      // Movies where the genres list is a subset of each movie's genre list.
                               actorMoviesWithGenres,                                                                       // No genre filtering
                               true)),                                                                                      // Denotes whether to return the poster or not. Always true in this instance. (May be changed later)
                            Utilities.ok                                                                                    // Code whether the data fetching was a success. Should be 200 (ok).
                       );
        }

        public IEnumerable<ActorDto> GetActorActorsByActorId(int id)
        {
            // First, get all movies actor has starred in.
            var actorMovies = 
            (
                from actor in filmFulDbContext.Actor
                    join action in filmFulDbContext.Action on actor.Id equals action.ActorId
                        join movie in filmFulDbContext.Movie on action.MovieId equals movie.Id
                        where actor.Id == id
                        select movie
            );

            // If actor has not been in any movie, he must not have ever starred in any film with anyone at all.
            if (actorMovies == null || !actorMovies.Any()) { return null; }

            // Second, get all actors that appeared in these movies - Excluding the actor in question and avoiding duplicates.
            var actorActors = 
            (
                from actor in filmFulDbContext.Actor
                    join action in filmFulDbContext.Action on actor.Id equals action.ActorId
                        join movie in actorMovies on action.MovieId equals movie.Id
                        where actor.Id != id
                        select actor
            ).Distinct();

            // If actor has never worked with other actors return null, else return actors.
            return (actorActors == null || !actorActors.Any()) ? null : DataTypeConversionUtils.ActorToActorDto(actorActors);
        }
    }
}