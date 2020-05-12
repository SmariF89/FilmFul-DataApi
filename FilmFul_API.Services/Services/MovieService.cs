using System.Collections.Generic;
using FilmFul_API.Repositories.Extensions;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Repositories;

namespace FilmFul_API.Services.Services
{
    public class MovieService
    {
        private readonly MovieRepository movieRepository = new MovieRepository();

        public (IEnumerable<MovieDto>, int) GetAllMovies(int pageSize, int pageIndex, bool poster, List<string> genres)
        {
            if (!Utilities.genresOkay(ref genres)) { return (null, Utilities.badRequest); }
            return movieRepository.GetAllMovies(pageSize, pageIndex, poster, genres);
        }

        public MovieDto GetMovieById(int id)
        {
            return movieRepository.GetMovieById(id);
        }

        public IEnumerable<ActorDto> GetMovieActorsByMovieId(int id)
        {
            return movieRepository.GetMovieActorsByMovieId(id);
        }

        public IEnumerable<DirectorDto> GetMovieDirectorsByMovieId(int id)
        {
            return movieRepository.GetMovieDirectorsByMovieId(id);
        }
    }
}