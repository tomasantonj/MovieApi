using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.Core.Models;
using Movie.Core.DTOs;
using MovieApi.Movie.Data.Repositories;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieReviewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieReviewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/MovieReviews
        // Returns a list of all movie reviews with related movie details
        [HttpGet]
        public async Task<ActionResult<PagedResponse<ReviewDto>>> GetMovieReview([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            page = Math.Max(page, 1);

            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            var totalItems = reviews.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paged = reviews
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReviewDto
                {
                    ReviewerName = r.ReviewerName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    MovieTitle = r.VideoMovie != null ? r.VideoMovie.Title : null,
                    MovieYear = r.VideoMovie != null ? r.VideoMovie.Year : null,
                    MovieGenre = r.VideoMovie != null && r.VideoMovie.Genre != null ? r.VideoMovie.Genre.Name : null,
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

        // GET: api/MovieReviews/5
        // Returns a specific movie review by ID with related movie details
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetMovieReview(int id)
        {
            var r = await _unitOfWork.Reviews.GetAsync(id);
            if (r == null)
                return NotFound();
            var dto = new ReviewDto
            {
                ReviewerName = r.ReviewerName,
                Rating = r.Rating,
                Comment = r.Comment,
                MovieTitle = r.VideoMovie != null ? r.VideoMovie.Title : null,
                MovieYear = r.VideoMovie != null ? r.VideoMovie.Year : null,
                MovieGenre = r.VideoMovie != null && r.VideoMovie.Genre != null ? r.VideoMovie.Genre.Name : null,
                MovieDuration = r.VideoMovie != null ? r.VideoMovie.Duration : null
            };
            return dto;
        }

        // PUT: api/MovieReviews/5
        // Endpoint that updates an existing movie review
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieReview(int id, ReviewUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();
            var movieReview = await _unitOfWork.Reviews.GetAsync(id);
            if (movieReview == null)
                return NotFound();
            // Optionally validate that the referenced Movie exists
            var movieExists = await _unitOfWork.VideoMovies.AnyAsync(updateDto.MovieId);
            if (!movieExists)
                return BadRequest($"Movie with Id {updateDto.MovieId} does not exist.");
            // Update allowed fields
            movieReview.ReviewerName = updateDto.ReviewerName;
            movieReview.Rating = updateDto.Rating;
            movieReview.Comment = updateDto.Comment;
            movieReview.VideoMovieId = updateDto.MovieId;
            _unitOfWork.Reviews.Update(movieReview);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        // POST: api/MovieReviews
        [HttpPost]
        public async Task<ActionResult<MovieReview>> PostMovieReview(MovieReview movieReview)
        {
            _unitOfWork.Reviews.Add(movieReview);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction("GetMovieReview", new { id = movieReview.Id }, movieReview);
        }

        // DELETE: api/MovieReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieReview(int id)
        {
            var movieReview = await _unitOfWork.Reviews.GetAsync(id);
            if (movieReview == null)
                return NotFound();
            _unitOfWork.Reviews.Remove(movieReview);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}