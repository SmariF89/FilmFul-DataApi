using FilmFul_API.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmFul_API.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly MovieService movieService = new MovieService();

        // GET api/
        // GET api/movies
        [HttpGet]
        [Route("")]
        [Route("movies")]
        public IActionResult GetAllMovies([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, [FromQuery] bool poster = false)
        {
            var moviesResult = movieService.GetAllMovies(pageSize, pageIndex, poster);
            if (moviesResult.Item2 == 0) { return Ok(moviesResult.Item1); }
            else { return StatusCode(moviesResult.Item2); }
        }

        // GET api/5
        // GET api/movies/5
        [HttpGet]
        [Route("{id}")]
        [Route("movies/{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movieById = movieService.GetMovieById(id);
            if (movieById == null) { return StatusCode(404); }        
            return Ok(movieById);
        }
    }
}