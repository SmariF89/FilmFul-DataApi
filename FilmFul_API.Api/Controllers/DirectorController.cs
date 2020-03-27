using FilmFul_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmFul_API.Api.Controllers
{
    [Route("api/directors")]
    [ApiController]
    public class DirectorController : Controller
    {
        private readonly DirectorService directorService = new DirectorService();

        // GET api/directors
        [HttpGet]
        [Route("")]
        public IActionResult GetAllDirectors([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var directorsResult = directorService.GetAllDirectors(pageSize, pageIndex);
            if (directorsResult.Item2 == 0) { return Ok(directorsResult.Item1); }
            else { return StatusCode(directorsResult.Item2); }
        }

        // GET api/directors/5
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDirectorById(int id)
        {
            var directorById = directorService.GetDirectorById(id);
            if (directorById == null) { return StatusCode(404); }        
            return Ok(directorById);
        }

        // GET api/directors/5/movies
        [HttpGet]
        [Route("{id}/movies")]
        public IActionResult GetDirectorFilmsByDirectorId(int id)
        {
            return StatusCode(404);
        }

        // GET api/directors/5/actors
        [HttpGet]
        [Route("{id}/actors")]
        public IActionResult GetDirectorActorsByDirectorId(int id)
        {
            var directorActors = directorService.GetDirectorActorsByDirectorId(id);
            if (directorActors == null) { return StatusCode(404); }
            return Ok(directorActors);
        }
    }
}