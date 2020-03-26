using System.Collections.Generic;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories;

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
    }
}