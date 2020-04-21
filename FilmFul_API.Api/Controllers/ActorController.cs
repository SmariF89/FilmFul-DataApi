using FilmFul_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmFul_API.Api.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorController : Controller
    {
        private readonly ActorService actorService = new ActorService();

        // GET api/actors
        [HttpGet]
        [Route("")]
        public IActionResult GetAllActors([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var actorsResult = actorService.GetAllActors(pageSize, pageIndex);
            if (actorsResult.Item2 == 0) { return Ok(actorsResult.Item1); }
            else { return StatusCode(actorsResult.Item2); }
        }

        // GET api/actors/5
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetActorById(int id)
        {
            var actorById = actorService.GetActorById(id);
            if (actorById == null) { return StatusCode(404); }        
            return Ok(actorById);
        }

        // GET api/actors/5/movies
        // TODO: Add an optional query parameter for movie genre.
        [HttpGet]
        [Route("{id}/movies")]
        public IActionResult GetActorMoviesByActorId(int id)
        {
            var actorMovies = actorService.GetActorMoviesByActorId(id);
            if (actorMovies == null) { return StatusCode(404); }
            return Ok(actorMovies);
        }

        // GET api/actors/5/directors
        // Returns all directors affiliated with a certain actor.
        [HttpGet]
        [Route("{id}/directors")]
        public IActionResult GetActorDirectorsByActorId(int id)
        {
            var actorDirectors = actorService.GetActorDirectorsByActorId(id);
            if (actorDirectors == null) { return StatusCode(404); }
            return Ok(actorDirectors);
        }

        // GET api/actors/5/actors
        // Returns all actors that have starred in some film(s) with a certain actor.
        [HttpGet]
        [Route("{id}/actors")]
        public IActionResult GetActorActorsByActorId(int id)
        {
            var actorActors = actorService.GetActorActorsByActorId(id);
            if (actorActors == null) { return StatusCode(404); }
            return Ok(actorActors);
        }
    }
}