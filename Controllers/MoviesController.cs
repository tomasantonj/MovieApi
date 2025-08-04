using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Core.Models;
using Movie.Core.DTOs;
using MovieApi.Movie.Data.Repositories;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MoviesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Movies
        // Supports optional filtering by genreId, year, and directorId using FromQuery
        // GenreId, year, and directorId are all optional so you can use them separately or together
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoMovieDto>>> GetMovie(
            [FromQuery] int? genreId,
            [FromQuery] int? year,
            [FromQuery] int? directorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            page = Math.Max(page, 1);

            var movies = await _unitOfWork.VideoMovies.GetAllAsync();
            var filtered = movies.AsQueryable();
            if (genreId.HasValue)
                filtered = filtered.Where(m => m.GenreId == genreId.Value);
            if (year.HasValue)
                filtered = filtered.Where(m => m.Year == year.Value);
            if (directorId.HasValue)
                filtered = filtered.Where(m => m.DirectorId == directorId.Value);
            var paged = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new VideoMovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    GenreId = m.GenreId,
                    GenreName = m.Genre != null ? m.Genre.Name : string.Empty,
                    DirectorId = m.DirectorId,
                    DirectorName = m.Director != null ? m.Director.Name : string.Empty,
                    Duration = m.Duration
                })
                .ToList();
            return paged;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoMovieDto>> GetMovie(int id)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return NotFound();
            var dto = new VideoMovieDto
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
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            page = Math.Max(page, 1);

            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            var filtered = reviews.Where(r => r.VideoMovieId == movieId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReviewDto
                {
                    ReviewerName = r.ReviewerName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    MovieTitle = r.VideoMovie != null ? r.VideoMovie.Title : null,
                    MovieYear = r.VideoMovie != null ? r.VideoMovie.Year : null,
                    MovieGenre = r.VideoMovie != null ? r.VideoMovie.Genre.Name : null,
                    MovieDuration = r.VideoMovie != null ? r.VideoMovie.Duration : null
                })
                .ToList();
            return filtered;
        }

        // GET: api/Movies/{id}/details
        // Returns a MovieDetailDto using LINQ and Select
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return NotFound();
            var dto = new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                GenreId = movie.GenreId,
                GenreName = movie.Genre != null ? movie.Genre.Name : string.Empty,
                DirectorId = movie.DirectorId,
                DirectorName = movie.Director != null ? movie.Director.Name : string.Empty,
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
                    MovieGenre = movie.Genre != null ? movie.Genre.Name : string.Empty,
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, [FromBody] VideoMovieUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return NotFound();
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.DirectorId = dto.DirectorId;
            movie.Duration = dto.Duration;
            _unitOfWork.VideoMovies.Update(movie);
            await _unitOfWork.CompleteAsync();
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
            var movie = new VideoMovie
            {
                Title = dto.Title,
                Year = dto.Year,
                GenreId = dto.GenreId,
                DirectorId = dto.DirectorId,
                Duration = dto.Duration
            };
            _unitOfWork.VideoMovies.Add(movie);
            await _unitOfWork.CompleteAsync();
            var result = new VideoMovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                GenreId = movie.GenreId,
                GenreName = string.Empty, // You may want to fetch Genre
                DirectorId = movie.DirectorId,
                DirectorName = string.Empty, // You may want to fetch Director
                Duration = movie.Duration
            };
            return CreatedAtAction("GetMovie", new { id = movie.Id }, result);
        }

        // POST: api/Movies/{movieId}/actors/{actorId}
        // This allows us to add an actor to a movie by their respective IDs
        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(movieId);
            if (movie == null)
                return NotFound($"Movie with id {movieId} not found.");
            // You would need to fetch the actor and add to the movie's MovieActors collection
            // This requires an ActorRepository implementation
            return NoContent();
        }

        // DELETE: api/Movies/5
        // returns 204 No Content if the movie is successfully deleted.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return NotFound();
            _unitOfWork.VideoMovies.Remove(movie);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}