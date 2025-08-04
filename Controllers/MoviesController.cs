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
        public async Task<ActionResult<PagedResponse<VideoMovieDto>>> GetMovie(
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
            var totalItems = filtered.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
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
            var meta = new PagedMeta
            {
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };
            return new PagedResponse<VideoMovieDto> { Data = paged, Meta = meta };
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
        public async Task<ActionResult<PagedResponse<ReviewDto>>> GetReviewsForMovie(int movieId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            page = Math.Max(page, 1);

            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            var filtered = reviews.Where(r => r.VideoMovieId == movieId);
            var totalItems = filtered.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paged = filtered
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
            var meta = new PagedMeta
            {
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };
            return new PagedResponse<ReviewDto> { Data = paged, Meta = meta };
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

        // PATCH: api/Movies/5
        // Patch a movie's fields. Accepts partial updates for movie and movie details.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchMovie(int id, [FromBody] VideoMoviePatchDto patchDto)
        {
            if (patchDto == null)
                return BadRequest();
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return NotFound();

            // Update Movie fields
            if (patchDto.Title != null)
                movie.Title = patchDto.Title;
            if (patchDto.Year.HasValue)
                movie.Year = patchDto.Year.Value;
            if (patchDto.GenreId.HasValue)
                movie.GenreId = patchDto.GenreId.Value;
            if (patchDto.DirectorId.HasValue)
                movie.DirectorId = patchDto.DirectorId.Value;
            if (patchDto.Duration.HasValue)
                movie.Duration = patchDto.Duration.Value;

            // Update MovieDetails fields
            if (movie.MovieDetails == null && (patchDto.Synopsis != null || patchDto.Language != null || patchDto.Budget.HasValue))
            {
                movie.MovieDetails = new MovieDetails();
            }
            if (movie.MovieDetails != null)
            {
                if (patchDto.Synopsis != null)
                    movie.MovieDetails.Synopsis = patchDto.Synopsis;
                if (patchDto.Language != null)
                    movie.MovieDetails.Language = patchDto.Language;
                if (patchDto.Budget.HasValue)
                    movie.MovieDetails.Budget = patchDto.Budget.Value;
            }

            _unitOfWork.VideoMovies.Update(movie);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}