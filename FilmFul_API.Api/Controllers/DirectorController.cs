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
        // Returns all directors, paged.
        [HttpGet]
        [Route("")]
        public IActionResult GetAllDirectors([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var directorsResult = directorService.GetAllDirectors(pageSize, pageIndex);
            if (directorsResult.Item2 == 0) { return Ok(directorsResult.Item1); }
            else { return StatusCode(directorsResult.Item2); }
        }

        // GET api/directors/5
        // Returns a director by id.
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDirectorById(int id)
        {
            var directorById = directorService.GetDirectorById(id);
            if (directorById == null) { return StatusCode(404); }        
            return Ok(directorById);
        }

        // GET api/directors/5/movies
        // Returns all films directed by a certain director.
        // TODO: Add an optional query parameter for movie genre.
        [HttpGet]
        [Route("{id}/movies")]
        public IActionResult GetDirectorMoviesByDirectorId(int id)
        {
            var directorMovies = directorService.GetDirectorMoviesByDirectorId(id);
            if (directorMovies == null) { return StatusCode(404); }
            return Ok(directorMovies);
        }

        // GET api/directors/5/actors
        // Returns all actors affiliated with a certain director.
        [HttpGet]
        [Route("{id}/actors")]
        public IActionResult GetDirectorActorsByDirectorId(int id)
        {
            var directorActors = directorService.GetDirectorActorsByDirectorId(id);
            if (directorActors == null) { return StatusCode(404); }
            return Ok(directorActors);
        }

        // GET api/directors/5/directors
        // Returns all directors that have co-directed some film(s) with a certain director.
        [HttpGet]
        [Route("{id}/directors")]
        public IActionResult GetDirectorDirectorsByDirectorId(int id)
        {
            var directorDirectors = directorService.GetDirectorDirectorsByDirectorId(id);
            if (directorDirectors == null) { return StatusCode(404); }
            return Ok(directorDirectors);
        }
    }
}