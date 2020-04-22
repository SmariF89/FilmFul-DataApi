using System.Collections.Generic;
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
        public IActionResult GetAllMovies([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, [FromQuery] bool poster = false, [FromQuery] List<string> genres = null)
        {
            var moviesResult = movieService.GetAllMovies(pageSize, pageIndex, poster, genres);
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

        // GET api/5/actors
        // GET api/movies/5/actors
        [HttpGet]
        [Route("{id}/actors")]
        [Route("movies/{id}/actors")]
        public IActionResult GetMovieActorsByMovieId(int id)
        {
            var movieActors = movieService.GetMovieActorsByMovieId(id);
            if (movieActors == null) { return StatusCode(404); }
            return Ok(movieActors);
        }

        // GET api/5/directors
        // GET api/movies/5/directors
        [HttpGet]
        [Route("{id}/directors")]
        [Route("movies/{id}/directors")]
        public IActionResult GetMovieDirectorsByMovieId(int id)
        {
            var movieDirectors = movieService.GetMovieDirectorsByMovieId(id);
            if (movieDirectors == null) { return StatusCode(404); }
            return Ok(movieDirectors);
        }
    }
}