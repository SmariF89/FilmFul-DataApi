using System.Collections.Generic;
using System.Linq;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Extensions;

namespace FilmFul_API.Repositories.Repositories
{
    public class MovieRepository
    {
        private readonly FilmFulDbContext filmFulDbContext = new FilmFulDbContext();

        public (IEnumerable<MovieDto>, int) GetAllMovies(int pageSize, int pageIndex, bool poster)
        {
            int rangeOkay = Utilities.checkRange(pageSize, pageIndex, filmFulDbContext.Movie.Count());
            
            if(rangeOkay == 0)
            {
                return (
                            DataTypeConversionUtils.MovieToMovieDto
                            (
                                (
                                    (from m in filmFulDbContext.Movie
                                     select m)
                                        .Skip(pageIndex * pageSize)
                                        .Take(pageSize)
                                ),
                                poster
                            ), 
                            rangeOkay
                        );
            }
            else { return (null, rangeOkay); }
        }

        public MovieDto GetMovieById(int id)
        {
            return DataTypeConversionUtils.MovieToMovieDto
            (
                filmFulDbContext.Movie
                    .Where(m => m.Id == id)
                    .SingleOrDefault()
            );
        }
    }
}