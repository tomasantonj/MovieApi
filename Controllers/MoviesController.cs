using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Core.Domain.Models;
using Movie.Core.DTOs;

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
        // Supports optional filtering by genreId, year, and directorId using FromQuery
        // GenreId, year, and directorId are all optional so you can use them separately or together
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie([FromQuery] int? genreId, [FromQuery] int? year, [FromQuery] int? directorId)
        {
            var query = _context.Movies.Include(m => m.Genre).Include(m => m.Director).AsQueryable();

            if (genreId.HasValue)
                query = query.Where(m => m.GenreId == genreId.Value);
            if (year.HasValue)
                query = query.Where(m => m.Year == year.Value);
            if (directorId.HasValue)
                query = query.Where(m => m.DirectorId == directorId.Value);

            var movies = await query.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                GenreId = m.GenreId,
                GenreName = m.Genre != null ? m.Genre.Name : string.Empty,
                DirectorId = m.DirectorId,
                DirectorName = m.Director != null ? m.Director.Name : string.Empty,
                Duration = m.Duration
            }).ToListAsync();
            return movies;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).Include(m => m.Director).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                GenreId = movie.GenreId,
                GenreName = movie.Genre != null ? movie.Genre.Name : string.Empty,
                DirectorId = movie.DirectorId,
                DirectorName = movie.Director != null ? movie.Director.Name : string.Empty,
                Duration = movie.Duration
            };
            return dto;
        }

        // GET: api/Movies/{movieId}/reviews
        // Returns a list of reviews for a specific movie
        [HttpGet("{movieId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var reviews = await _context.MovieReviews
                .Where(r => r.MovieId == movieId)
                .Include(r => r.Movie)
                .Select(r => new ReviewDto
                {
                    ReviewerName = r.ReviewerName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    MovieTitle = r.Movie != null ? r.Movie.Title : null,
                    MovieYear = r.Movie != null ? r.Movie.Year : null,
                    MovieGenre = r.Movie != null ? r.Movie.Genre.Name : null,
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
            var movieDetail = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .Where(m => m.Id == id)
                .Select(m => new MovieDetailDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    GenreId = m.GenreId,
                    GenreName = m.Genre != null ? m.Genre.Name : string.Empty,
                    DirectorId = m.DirectorId,
                    DirectorName = m.Director != null ? m.Director.Name : string.Empty,
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
                        MovieGenre = m.Genre != null ? m.Genre.Name : string.Empty,
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

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.DirectorId = dto.DirectorId;
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
        public async Task<ActionResult<MovieDto>> PostMovie([FromBody] MovieCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genre = await _context.Genres.FindAsync(dto.GenreId);
            if (genre == null)
                return BadRequest($"Genre with id {dto.GenreId} does not exist.");

            var director = await _context.Directors.FindAsync(dto.DirectorId);
            if (director == null)
                return BadRequest($"Director with id {dto.DirectorId} does not exist.");

            // Using fully qualified name for Movie to avoid ambiguity with Movie namespace
            var movie = new Movie.Core.Domain.Models.Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                GenreId = dto.GenreId,
                DirectorId = dto.DirectorId,
                Duration = dto.Duration
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var result = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                GenreId = movie.GenreId,
                GenreName = genre.Name,
                DirectorId = movie.DirectorId,
                DirectorName = director.Name,
                Duration = movie.Duration
            };

            return CreatedAtAction("GetMovie", new { id = movie.Id }, result);
        }

        // POST: api/Movies/{movieId}/actors/{actorId}
        // This allows us to add an actor to a movie by their respective IDs
        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
                return NotFound($"Movie with id {movieId} not found.");

            var actor = await _context.Actors.FindAsync(actorId);
            if (actor == null)
                return NotFound($"Actor with id {actorId} not found.");

            bool alreadyExists = await _context.MovieActors.AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);
            if (alreadyExists)
                return Conflict($"Actor with id {actorId} is already associated with movie {movieId}.");

            var movieActor = new MovieActor { MovieId = movieId, ActorId = actorId };
            _context.MovieActors.Add(movieActor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Movies/5
        // returns 204 No Content if the movie is successfully deleted.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}