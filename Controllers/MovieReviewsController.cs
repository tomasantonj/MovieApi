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
    public class MovieReviewsController : ControllerBase
    {
        private readonly MovieApiContext _context;

        public MovieReviewsController(MovieApiContext context)
        {
            _context = context;
        }

        // GET: api/MovieReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReview()
        {
            var reviews = await _context.MovieReview
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

        // GET: api/MovieReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetMovieReview(int id)
        {
            var r = await _context.MovieReview
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (r == null)
            {
                return NotFound();
            }
            // TODO: REVIEW IF THIS SHOULD BE INCLUDED IN THE REVIEW DTO
            // PROBABLY NOT AS ITS A NOT RELATED TO THE REVIEW ITSELF
            // BUT COULD BE USEFUL FOR DISPLAYING MOVIE DETAILS ON THE REVIEW VIEW
            var dto = new ReviewDto
            {
                ReviewerName = r.ReviewerName,
                Rating = r.Rating,
                Comment = r.Comment,
                MovieTitle = r.Movie != null ? r.Movie.Title : null,
                MovieYear = r.Movie != null ? r.Movie.Year : null,
                MovieGenre = r.Movie != null ? r.Movie.Genre : null,
                MovieDuration = r.Movie != null ? r.Movie.Duration : null
            };
            return dto;
        }

        // PUT: api/MovieReviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieReview(int id, MovieReview movieReview)
        {
            if (id != movieReview.Id)
            {
                return BadRequest();
            }

            _context.Entry(movieReview).State = EntityState.Modified;

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
            _context.MovieReview.Add(movieReview);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieReview", new { id = movieReview.Id }, movieReview);
        }

        // DELETE: api/MovieReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieReview(int id)
        {
            var movieReview = await _context.MovieReview.FindAsync(id);
            if (movieReview == null)
            {
                return NotFound();
            }

            _context.MovieReview.Remove(movieReview);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieReviewExists(int id)
        {
            return _context.MovieReview.Any(e => e.Id == id);
        }
    }
}
