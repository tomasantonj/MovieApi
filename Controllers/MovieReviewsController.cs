using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Core.Models;
using Movie.Core.DTOs;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieReviewsController : ControllerBase
    {
        private readonly MovieApiContext _context;

        public MovieReviewsController(MovieApiContext context)
        {
            _context = context;
        }

        // GET: api/MovieReviews
        // Returns a list of all movie reviews with related movie details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReview()
        {
            var reviews = await _context.MovieReviews
                .Include(r => r.VideoMovie)
                .ThenInclude(m => m.Genre)
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
                .ToListAsync();
            return reviews;
        }

        // GET: api/MovieReviews/5
        // Returns a specific movie review by ID with related movie details
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetMovieReview(int id)
        {
            var r = await _context.MovieReviews
                .Include(r => r.VideoMovie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (r == null)
            {
                return NotFound();
            }
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
            {
                return BadRequest();
            }

            var movieReview = await _context.MovieReviews.FindAsync(id);
            if (movieReview == null)
            {
                return NotFound();
            }

            // Optionally validate that the referenced Movie exists
            var movieExists = await _context.VideoMovies.AnyAsync(m => m.Id == updateDto.MovieId);
            if (!movieExists)
            {
                return BadRequest($"Movie with Id {updateDto.MovieId} does not exist.");
            }

            // Update allowed fields
            movieReview.ReviewerName = updateDto.ReviewerName;
            movieReview.Rating = updateDto.Rating;
            movieReview.Comment = updateDto.Comment;
            movieReview.VideoMovieId = updateDto.MovieId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieReviewExists(id))
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

        // POST: api/MovieReviews
        [HttpPost]
        public async Task<ActionResult<MovieReview>> PostMovieReview(MovieReview movieReview)
        {
            _context.MovieReviews.Add(movieReview);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieReview", new { id = movieReview.Id }, movieReview);
        }

        // DELETE: api/MovieReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieReview(int id)
        {
            var movieReview = await _context.MovieReviews.FindAsync(id);
            if (movieReview == null)
            {
                return NotFound();
            }

            _context.MovieReviews.Remove(movieReview);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieReviewExists(int id)
        {
            return _context.MovieReviews.Any(e => e.Id == id);
        }
    }
}