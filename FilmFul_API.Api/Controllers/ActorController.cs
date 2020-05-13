using System.Collections.Generic;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Extensions;
using FilmFul_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmFul_API.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/actors")]
    [ApiController]
    public class ActorController : Controller
    {
        private readonly ActorService actorService = new ActorService();

        // GET api/actors
        /// <summary>
        /// Gets all actors with pagination.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/actors
        ///     http://URL:&lt;port&gt;/api/actors?pageSize=1&amp;pageIndex=0
        ///     http://URL:&lt;port&gt;/api/actors?pageSize=100&amp;pageIndex=2
        /// </remarks>
        /// <param name="pageSize" example="25">Determines the number of actors returned in one payload. Min value is 1. Max value is 100.</param>
        /// <param name="pageIndex">Determines which index, or page, is to be returned. Zero-based.</param>
        /// <returns>Actors paged in accordance to the supplied query parameters.</returns>
        /// <response code="200" example="">Upon success. Returns the requested actors.</response>
        /// <response code="400" example="">If the supplied index is out of bounds.</response>
        /// <response code="404" example="">If the resource is empty for some reason. If there are no actors in the database. An unlikely result but it is there.</response>
        /// <response code="413" example="">If the supplied page size is larger than 100.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<ActorDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllActors([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var actorsResult = actorService.GetAllActors(pageSize, pageIndex);
            if (actorsResult.Item2 == Utilities.ok) { return Ok(actorsResult.Item1); }
            else { return StatusCode(actorsResult.Item2); }
        }

        // GET api/actors/5
        /// <summary>
        /// Gets an actor by a consumer-supplied id.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/actors/12
        ///     http://URL:&lt;port&gt;/api/actors/55
        /// </remarks>
        /// <param name="id" example="55">The id of the actor which is to be fetched.</param>
        /// <returns>A single actor data-transfer object with an id matching the supplied one.</returns>
        /// <response code="200" example="">Upon success. Returns the requested actor.</response>
        /// <response code="400" example="">If the supplied id is not an integer.</response>
        /// <response code="404" example="">If an actor with the supplied id is not present in the database.</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ActorDto), StatusCodes.Status200OK)]
        public IActionResult GetActorById(int id)
        {
            var actorById = actorService.GetActorById(id);
            if (actorById == null) { return StatusCode(404); }        
            return Ok(actorById);
        }

        // GET api/actors/5/movies
        [HttpGet]
        [Route("{id}/movies")]
        public IActionResult GetActorMoviesByActorId(int id, [FromQuery] List<string> genres = null)
        {
            var actorMovies = actorService.GetActorMoviesByActorId(id, genres);
            if (actorMovies.Item1 == null) { return StatusCode(actorMovies.Item2); }
            return Ok(actorMovies.Item1);
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