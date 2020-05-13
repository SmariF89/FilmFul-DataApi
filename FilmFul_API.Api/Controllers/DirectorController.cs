using System.Collections.Generic;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Extensions;
using FilmFul_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmFul_API.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/directors")]
    [ApiController]
    public class DirectorController : Controller
    {
        private readonly DirectorService directorService = new DirectorService();

        // GET api/directors
        /// <summary>
        /// Gets all directors with pagination.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/directors
        ///     http://URL:&lt;port&gt;/api/directors?pageSize=1&amp;pageIndex=0
        ///     http://URL:&lt;port&gt;/api/directors?pageSize=100&amp;pageIndex=2
        /// </remarks>
        /// <param name="pageSize" example="25">Determines the number of directors returned in one payload. Min value is 1. Max value is 100.</param>
        /// <param name="pageIndex">Determines which index, or page, is to be returned. Zero-based.</param>
        /// <returns>Directors paged in accordance to the supplied query parameters.</returns>
        /// <response code="200" example="">Upon success. Returns the requested directors.</response>
        /// <response code="400" example="">If the supplied index is out of bounds.</response>
        /// <response code="404" example="">If the resource is empty for some reason. If there are no directors in the database. An unlikely result but it is there.</response>
        /// <response code="413" example="">If the supplied page size is larger than 100.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<DirectorDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllDirectors([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var directorsResult = directorService.GetAllDirectors(pageSize, pageIndex);
            if (directorsResult.Item2 == Utilities.ok) { return Ok(directorsResult.Item1); }
            else { return StatusCode(directorsResult.Item2); }
        }

        // GET api/directors/5
        /// <summary>
        /// Gets a director by a consumer-supplied id.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/directors/13
        ///     http://URL:&lt;port&gt;/api/directors/36
        /// </remarks>
        /// <param name="id" example="36">The id of the director which is to be fetched.</param>
        /// <returns>A single director data-transfer object with an id matching the supplied one.</returns>
        /// <response code="200" example="">Upon success. Returns the requested director.</response>
        /// <response code="400" example="">If the supplied id is not an integer.</response>
        /// <response code="404" example="">If a director with the supplied id is not present in the database.</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DirectorDto), StatusCodes.Status200OK)]
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
        public IActionResult GetDirectorMoviesByDirectorId(int id, [FromQuery] List<string> genres = null)
        {
            var directorMovies = directorService.GetDirectorMoviesByDirectorId(id, genres);
            if (directorMovies.Item1 == null) { return StatusCode(directorMovies.Item2); }
            return Ok(directorMovies.Item1);
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