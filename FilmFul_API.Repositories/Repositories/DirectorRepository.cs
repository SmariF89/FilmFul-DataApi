using System.Collections.Generic;
using System.Linq;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Extensions;

namespace FilmFul_API.Repositories
{
    public class DirectorRepository
    {
        private readonly FilmFulDbContext filmFulDbContext = new FilmFulDbContext();

        public (IEnumerable<DirectorDto>, int) GetAllDirectors(int pageSize, int pageIndex)
        {
            int rangeOkay = Utilities.checkRange(pageSize, pageIndex, filmFulDbContext.Director.Count());
            
            if(rangeOkay == Utilities.ok)
            {
                return (
                            DataTypeConversionUtils.DirectorToDirectorDto
                            (
                                ((from d in filmFulDbContext.Director 
                                select d)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                )
                            ), rangeOkay
                        );
            }
            else { return (null, rangeOkay); }
        }

        public DirectorDto GetDirectorById(int id)
        {
            var directorById = filmFulDbContext.Director
                                   .Where(d => d.Id == id)
                                   .SingleOrDefault();

            return directorById == null ? null : DataTypeConversionUtils.DirectorToDirectorDto(directorById);
        }
        
        public (IEnumerable<MovieDto>, int) GetDirectorMoviesByDirectorId(int id, List<string> genres)
        {
            var directorMovies = (from director in filmFulDbContext.Director
                                    join direction in filmFulDbContext.Direction on director.Id equals direction.DirectorId
                                        join movie in filmFulDbContext.Movie on direction.MovieId equals movie.Id
                                            join genre in filmFulDbContext.Genre on movie.Id equals genre.MovieId
                                            where director.Id == id
                                            select new { movie, genre }
                                 ).ToList();

            if (directorMovies == null || !directorMovies.Any()) { return (null, Utilities.notFound); }             // Director has not directed any film.
            
            var directorMoviesWithGenres = directorMovies
                                               .Select(m => m.movie)
                                               .Distinct();

            return
            (
                (
                    DataTypeConversionUtils.MovieToMovieDto(genres != null ?
                        directorMoviesWithGenres.Where(m => !genres.Except(m.Genre.Select(g => g.Genre1)).Any()) :      // Movies where the genres list is a subset of each movie's genre list.
                        directorMoviesWithGenres,                                                                       // No genre filtering
                    true)
                ),                                                                                                  // Denotes whether to return the poster or not. Always true in this instance. (May be changed later)
                Utilities.ok                                                                                        // Code whether the data fetching was a success. Should be 200 (ok).
            );
        }
        
        public IEnumerable<ActorDto> GetDirectorActorsByDirectorId(int id)
        {
            var directorActors = (from actor in filmFulDbContext.Actor
                                    join action in filmFulDbContext.Action on actor.Id equals action.ActorId
                                        join movie in filmFulDbContext.Movie on action.MovieId equals movie.Id
                                            join direction in filmFulDbContext.Direction on movie.Id equals direction.MovieId
                                                join director in filmFulDbContext.Director on direction.DirectorId equals director.Id
                                                where director.Id == id
                                                select actor)
                                                    .Distinct()
                                                    .OrderBy(ac => ac.Name);

            return (directorActors == null || !directorActors.Any()) ? null : DataTypeConversionUtils.ActorToActorDto(directorActors);
        }

        public IEnumerable<DirectorDto> GetDirectorDirectorsByDirectorId(int id)
        {
            // First, get all movies director has directed.
            var directorMovies = 
            (
                from director in filmFulDbContext.Director
                    join direction in filmFulDbContext.Direction on director.Id equals direction.DirectorId
                        join movie in filmFulDbContext.Movie on direction.MovieId equals movie.Id
                        where director.Id == id
                        select movie
            );

            // If director has not directed any movie, he must not have ever co-directed any film with anyone at all.
            if (directorMovies == null || !directorMovies.Any()) { return null; }

            // Second, get all directors that co-directed these movies - Excluding the director in question and avoiding duplicates.
            var directorDirectors = 
            (
                from director in filmFulDbContext.Director
                    join direction in filmFulDbContext.Direction on director.Id equals direction.DirectorId
                        join movie in directorMovies on direction.MovieId equals movie.Id
                        where director.Id != id
                        select director
            ).Distinct();

            // If director has never worked with other directors return null, else return directors.
            return (directorDirectors == null || !directorDirectors.Any()) ? null : DataTypeConversionUtils.DirectorToDirectorDto(directorDirectors);
        }
    }
}