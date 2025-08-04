using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Movie.Core.DTOs;
using Movie.Contracts;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/Movies
        // Supports optional filtering by genreId, year, and directorId using FromQuery
        // GenreId, year, and directorId are all optional so you can use them separately or together
        [HttpGet]
        public async Task<ActionResult<PagedResponse<VideoMovieDto>>> GetMovie(
            [FromQuery] int? genreId,
            [FromQuery] int? year,
            [FromQuery] int? directorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _movieService.GetMovies(genreId, year, directorId, page, pageSize);
            return Ok(result);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoMovieDto>> GetMovie(int id)
        {
            var dto = await _movieService.GetMovie(id);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        // GET: api/Movies/{movieId}/reviews
        // Returns a list of reviews for a specific movie
        [HttpGet("{movieId}/reviews")]
        public async Task<ActionResult<PagedResponse<ReviewDto>>> GetReviewsForMovie(int movieId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _movieService.GetReviewsForMovie(movieId, page, pageSize);
            return Ok(result);
        }

        // GET: api/Movies/{id}/details
        // Returns a MovieDetailDto using LINQ and Select
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var dto = await _movieService.GetMovieDetails(id);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        // PUT: api/Movies/5
        // Updates Movie through the MovieUpdateDTO and returns 204 No Content if successful.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, [FromBody] VideoMovieUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var success = await _movieService.UpdateMovie(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        // POST: api/Movies
        // This allows to send a json with movie details to create a new movie
        // and store it in the database. It then returns the created movie with a 201 status code.
        [HttpPost]
        public async Task<ActionResult<VideoMovieDto>> PostMovie([FromBody] VideoMovieCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _movieService.CreateMovie(dto);
            if (result == null)
                return BadRequest();
            return CreatedAtAction("GetMovie", new { id = result.Id }, result);
        }

        // POST: api/Movies/{movieId}/actors/{actorId}
        // This allows us to add an actor to a movie by their respective IDs
        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var success = await _movieService.AddActorToMovie(movieId, actorId);
            if (!success)
                return NotFound($"Movie with id {movieId} not found.");
            return NoContent();
        }

        // DELETE: api/Movies/5
        // returns 204 No Content if the movie is successfully deleted.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var success = await _movieService.DeleteMovie(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

        // PATCH: api/Movies/5
        // Patch a movie's fields. Accepts partial updates for movie and movie details.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchMovie(int id, [FromBody] VideoMoviePatchDto patchDto)
        {
            if (patchDto == null)
                return BadRequest();
            var success = await _movieService.PatchMovie(id, patchDto);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}