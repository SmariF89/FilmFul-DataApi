using System.Collections.Generic;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Repositories;

namespace FilmFul_API.Services.Services
{
    public class MovieService
    {
        private readonly MovieRepository movieRepository = new MovieRepository();

        public (IEnumerable<MovieDto>, int) GetAllMovies(int pageSize, int pageIndex, bool poster)
        {
            return movieRepository.GetAllMovies(pageSize, pageIndex, poster);
        }

        public MovieDto GetMovieById(int id)
        {
            return movieRepository.GetMovieById(id);
        }
    }
}