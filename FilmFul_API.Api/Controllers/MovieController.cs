using System.Collections.Generic;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Extensions;
using FilmFul_API.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmFul_API.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly MovieService movieService = new MovieService();

        // GET api/
        // GET api/movies
        /// <summary>
        /// Gets all movies with possibilities of certain customizations of how the data is returned.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/&#xD;
        ///     http://URL:&lt;port&gt;/api/movies&#xD;
        ///     http://URL:&lt;port&gt;/api/movies?pageSize=15&amp;pageIndex=3&#xD;
        ///     http://URL:&lt;port&gt;/api/movies?genres=action&amp;pageIndex=2&amp;pageSize=50&#xD;
        ///     http://URL:&lt;port&gt;/api/movies?genres=action&amp;genres=history&amp;pageIndex=2&amp;pageSize=50
        /// </remarks>
        /// <param name="pageSize" example="25">Determines the number of movies returned in one payload. Min value is 1. Max value is 100.</param>
        /// <param name="pageIndex">Determines which index, or page, is to be returned. Zero-based.</param>
        /// <param name="poster">Determines whether or not each movie's poster will be present within the returned data. The poster may be unwanted if a large payload of movies is being fetched.</param>
        /// <param name="genres">
        /// Determines of which genre(s) the returned movies should be. Can be a one or more genres. By default, the returned data is genre-agnostic. The movies are filtered by genre after the requested page of data has been gathered.&#xD;
        /// Possible genres are: Action, Adventure, Animation, Biography, Comedy, Crime, Drama, Family, Fantasy, Film-Noir, History, Horror, Music, Musical, Mystery, Romance, Sci-Fi, Sport, Thriller, War, Western.&#xD;
        /// The genres are case-insensitive but care about the dashes where they apply.
        /// </param>
        /// <returns>All movies or some subset of them according to the values supplied as query parameters.</returns>
        /// <response code="200" example="">Upon success. Returns the requested movies.</response>
        /// <response code="400" example="">If the one or more genre is invalid. Also if the supplied index is out of bounds.</response>
        /// <response code="404" example="">If the resource is empty for some reason. If there are no movies in the database. An unlikely result but it is there.</response>
        /// <response code="413" example="">If the supplied page size is larger than 100.</response>
        [HttpGet]
        [Route("")]
        [Route("movies")]
        [ProducesResponseType(typeof(IEnumerable<MovieDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllMovies([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, [FromQuery] bool poster = false, [FromQuery] List<string> genres = null)
        {
            var moviesResult = movieService.GetAllMovies(pageSize, pageIndex, poster, genres);
            if (moviesResult.Item2 == Utilities.ok) { return Ok(moviesResult.Item1); }
            else { return StatusCode(moviesResult.Item2); }
        }

        // GET api/5
        // GET api/movies/5
        /// <summary>
        /// Gets a movie by a consumer-supplied id. The returned movie data-transfer object will always contain the poster binary data.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/75
        ///     http://URL:&lt;port&gt;/api/movies/33
        ///     http://URL:&lt;port&gt;/api/movies/112
        /// </remarks>
        /// <param name="id" example="112">The id of the movie which is to be fetched.</param>
        /// <returns>A single movie data-transfer object with an id matching the supplied one.</returns>
        /// <response code="200" example="">Upon success. Returns the requested movie.</response>
        /// <response code="400" example="">If the supplied id is not an integer.</response>
        /// <response code="404" example="">If a movie with the supplied id is not present in the database.</response>
        [HttpGet]
        [Route("{id}")]
        [Route("movies/{id}")]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
        public IActionResult GetMovieById(int id)
        {
            var movieById = movieService.GetMovieById(id);
            if (movieById == null) { return StatusCode(404); }        
            return Ok(movieById);
        }

        // GET api/5/actors
        // GET api/movies/5/actors
        /// <summary>
        /// Gets a movie's actors by a consumer-supplied movie id.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/75/actors
        ///     http://URL:&lt;port&gt;/api/movies/33/actors
        ///     http://URL:&lt;port&gt;/api/movies/112/actors
        /// </remarks>
        /// <param name="id" example="112">The id of the movie which's actors are to be fetched.</param>
        /// <returns>A list of actor data-transfer objects denoting actors that starred in the movie which's id matches the supplied one.</returns>
        /// <response code="200" example="">Upon success. Returns the requested movie actors.</response>
        /// <response code="400" example="">If the supplied id is not an integer.</response>
        /// <response code="404" example="">If a movie with the supplied id is not present in the database. Also if, for some reason, no actors are registered to have starred in the movie in question.</response>
        [HttpGet]
        [Route("{id}/actors")]
        [Route("movies/{id}/actors")]
        [ProducesResponseType(typeof(IEnumerable<ActorDto>), StatusCodes.Status200OK)]
        public IActionResult GetMovieActorsByMovieId(int id)
        {
            var movieActors = movieService.GetMovieActorsByMovieId(id);
            if (movieActors == null) { return StatusCode(404); }
            return Ok(movieActors);
        }

        // GET api/5/directors
        // GET api/movies/5/directors
        /// <summary>
        /// Gets a movie's directors by a consumer-supplied movie id. Usually only one, sometimes more.
        /// </summary>
        /// <remarks>
        /// Sample requests:&#xD;
        ///     http://URL:&lt;port&gt;/api/75/directors
        ///     http://URL:&lt;port&gt;/api/movies/33/directors
        ///     http://URL:&lt;port&gt;/api/movies/112/directors
        /// </remarks>
        /// <param name="id" example="52">The id of the movie which's directors are to be fetched.</param>
        /// <returns>A list of director data-transfer objects denoting directors that directed the movie which's id matches the supplied one.</returns>
        /// <response code="200" example="">Upon success. Returns the requested movie directors.</response>
        /// <response code="400" example="">If the supplied id is not an integer.</response>
        /// <response code="404" example="">If a movie with the supplied id is not present in the database. Also if, for some reason, no directors are registered to have directed the movie in question.</response>
        [HttpGet]
        [Route("{id}/directors")]
        [Route("movies/{id}/directors")]
        [ProducesResponseType(typeof(IEnumerable<DirectorDto>), StatusCodes.Status200OK)]
        public IActionResult GetMovieDirectorsByMovieId(int id)
        {
            var movieDirectors = movieService.GetMovieDirectorsByMovieId(id);
            if (movieDirectors == null) { return StatusCode(404); }
            return Ok(movieDirectors);
        }
    }
}