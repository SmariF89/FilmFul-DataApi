using System.Collections.Generic;
using FilmFul_API.Models.Entities;
using FilmFul_API.Repositories;

namespace FilmFul_API.Services
{
    public class ActorService
    {
        private readonly ActorRepository actorRepository = new ActorRepository();

        public (IEnumerable<Actor>, int) GetAllActors(int pageSize, int pageIndex)
        {
            return actorRepository.GetAllActors(pageSize, pageIndex);
        }

        public Actor GetActorById(int id)
        {
            return actorRepository.GetActorById(id);
        }
    }
}