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
            
            if(rangeOkay == 0)
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

        public IEnumerable<MovieDto> GetDirectorMoviesByDirectorId(int id)
        {
            var directorMovies = (from director in filmFulDbContext.Director
                                  join direction in filmFulDbContext.Direction on director.Id equals direction.DirectorId
                                      join movie in filmFulDbContext.Movie on direction.MovieId equals movie.Id
                                      where director.Id == id
                                      select movie);

            return (directorMovies == null || !directorMovies.Any()) ? null : DataTypeConversionUtils.MovieToMovieDto(directorMovies, true);
        }
    }
}