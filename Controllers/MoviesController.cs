using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using MovieApi.DTOs;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieApiContext _context;

        public MoviesController(MovieApiContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        // Supports optional filtering by genre and year using FromQuery
        // Genre and year are both optional so you can use them separetely or together
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie([FromQuery] string? genre, [FromQuery] int? year)
        {
            var query = _context.Movie.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(m => m.Genre == genre);
            if (year.HasValue)
                query = query.Where(m => m.Year == year.Value);

            return await query.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // GET: api/Movies/{movieId}/reviews
        // Returns a list of reviews for a specific movie
        [HttpGet("{movieId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var reviews = await _context.MovieReview
                .Where(r => r.MovieId == movieId)
                .Include(r => r.Movie)
                .Select(r => new ReviewDto
                {
                    ReviewerName = r.ReviewerName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    MovieTitle = r.Movie != null ? r.Movie.Title : null,
                    MovieYear = r.Movie != null ? r.Movie.Year : null,
                    MovieGenre = r.Movie != null ? r.Movie.Genre : null,
                    MovieDuration = r.Movie != null ? r.Movie.Duration : null
                })
                .ToListAsync();
            return reviews;
        }

        // GET: api/Movies/{id}/details
        // Returns a MovieDetailDto using LINQ and Select
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movieDetail = await _context.Movie
                .Where(m => m.Id == id)
                .Select(m => new MovieDetailDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    Duration = m.Duration,
                    MovieDetails = m.MovieDetails == null ? null : new MovieDetailsDto
                    {
                        Synopsis = m.MovieDetails.Synopsis,
                        Language = m.MovieDetails.Language,
                        Budget = m.MovieDetails.Budget
                    },
                    Reviews = m.Reviews.Select(r => new ReviewDto
                    {
                        ReviewerName = r.ReviewerName,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        MovieTitle = m.Title,
                        MovieYear = m.Year,
                        MovieGenre = m.Genre,
                        MovieDuration = m.Duration
                    }).ToList(),
                    Actors = m.MovieActors.Select(ma => new ActorDto
                    {
                        Id = ma.Actor.Id,
                        Name = ma.Actor.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (movieDetail == null)
            {
                return NotFound();
            }

            return movieDetail;
        }

        // PUT: api/Movies/5
        // Updates Movie through the MovieUpdateDTO and returns 204 No Content if successful.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, [FromBody] MovieUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Genre = dto.Genre;
            movie.Duration = dto.Duration;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // This allows to send a json with movie details to create a new movie
        // and store it in the database. It then returns the created movie with a 201 status code.
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie([FromBody] MovieCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Genre = dto.Genre,
                Duration = dto.Duration
            };

            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // POST: api/Movies/{movieId}/actors/{actorId}
        // This allows us to add an actor to a movie by their respective IDs
        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _context.Movie.FindAsync(movieId);
            if (movie == null)
                return NotFound($"Movie with id {movieId} not found.");

            var actor = await _context.Actor.FindAsync(actorId);
            if (actor == null)
                return NotFound($"Actor with id {actorId} not found.");

            bool alreadyExists = await _context.MovieActor.AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);
            if (alreadyExists)
                return Conflict($"Actor with id {actorId} is already associated with movie {movieId}.");

            var movieActor = new MovieActor { MovieId = movieId, ActorId = actorId };
            _context.MovieActor.Add(movieActor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Movies/5
        // returns 204 No Content if the movie is successfully deleted.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
