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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            return await _context.Movie.ToListAsync();
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
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _context.Movie
                .Include(m => m.MovieDetails)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Duration = movie.Duration,
                MovieDetails = movie.MovieDetails == null ? null : new MovieDetailsDto
                {
                    Synopsis = movie.MovieDetails.Synopsis,
                    Language = movie.MovieDetails.Language,
                    Budget = movie.MovieDetails.Budget
                },
                Reviews = movie.Reviews.Select(r => new ReviewDto
                {
                    ReviewerName = r.ReviewerName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    MovieTitle = movie.Title,
                    MovieYear = movie.Year,
                    MovieGenre = movie.Genre,
                    MovieDuration = movie.Duration
                }).ToList(),
                Actors = movie.MovieActors.Select(ma => new ActorDto
                {
                    Id = ma.Actor.Id,
                    Name = ma.Actor.Name
                }).ToList()
            };

            return dto;
        }

        // PUT: api/Movies/5
        // Updates Movie through the MovieUpdateDTO and returns 204 No Content if successful.
        // TODO: Maybe respond with a 200 OK and show the updated movie?
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

        // DELETE: api/Movies/5
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
