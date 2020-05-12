using System.Collections.Generic;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories;
using FilmFul_API.Repositories.Extensions;

namespace FilmFul_API.Services
{
    public class ActorService
    {
        private readonly ActorRepository actorRepository = new ActorRepository();

        public (IEnumerable<ActorDto>, int) GetAllActors(int pageSize, int pageIndex)
        {
            return actorRepository.GetAllActors(pageSize, pageIndex);
        }

        public ActorDto GetActorById(int id)
        {
            return actorRepository.GetActorById(id);
        }

        public (IEnumerable<MovieDto>, int) GetActorMoviesByActorId(int id, List<string> genres)
        {
            if (!Utilities.genresOkay(ref genres)) { return (null, Utilities.badRequest); }
            return actorRepository.GetActorMoviesByActorId(id, genres);
        }

        public IEnumerable<DirectorDto> GetActorDirectorsByActorId(int id)
        {
            return actorRepository.GetActorDirectorsByActorId(id);
        }

        public IEnumerable<ActorDto> GetActorActorsByActorId(int id)
        {
            return actorRepository.GetActorActorsByActorId(id);
        }
    }
}