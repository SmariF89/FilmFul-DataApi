using FilmFul_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmFul_API.Api.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorController : Controller
    {
        private readonly ActorService actorService = new ActorService();

        // GET api/actor
        [HttpGet("")]
        public IActionResult GetAllActors()
        {
            var allActors = actorService.GetAllActors();
            if (allActors == null) { return StatusCode(404); }
            return Ok(allActors);
        }

        // GET api/actor/5
        [HttpGet("{id}")]
        public IActionResult GetActorById(int id)
        {
            var actorById = actorService.GetActorById(id);
            if (actorById == null) { return StatusCode(404); }        
            return Ok(actorById);
        }
    }
}