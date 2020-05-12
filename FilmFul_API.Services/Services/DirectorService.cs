using System.Collections.Generic;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories;
using FilmFul_API.Repositories.Extensions;

namespace FilmFul_API.Services
{
    public class DirectorService
    {
        private readonly DirectorRepository directorRepository = new DirectorRepository();

        public (IEnumerable<DirectorDto>, int) GetAllDirectors(int pageSize, int pageIndex)
        {
            return directorRepository.GetAllDirectors(pageSize, pageIndex);
        }

        public DirectorDto GetDirectorById(int id)
        {
            return directorRepository.GetDirectorById(id);
        }

        public (IEnumerable<MovieDto>, int) GetDirectorMoviesByDirectorId(int id, List<string> genres)
        {
            if (!Utilities.genresOkay(ref genres)) { return (null, Utilities.badRequest); }
            return directorRepository.GetDirectorMoviesByDirectorId(id, genres);
        }

        public IEnumerable<ActorDto> GetDirectorActorsByDirectorId(int id)
        {
            return directorRepository.GetDirectorActorsByDirectorId(id);
        }

        public IEnumerable<DirectorDto> GetDirectorDirectorsByDirectorId(int id)
        {
            return directorRepository.GetDirectorDirectorsByDirectorId(id);
        }
    }
}