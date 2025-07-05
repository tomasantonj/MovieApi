using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;

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
        public async Task<ActionResult<IEnumerable<MovieReview>>> GetMovieReview()
        {
            return await _context.MovieReview.ToListAsync();
        }

        // GET: api/MovieReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieReview>> GetMovieReview(int id)
        {
            var movieReview = await _context.MovieReview.FindAsync(id);

            if (movieReview == null)
            {
                return NotFound();
            }

            return movieReview;
        }

        // PUT: api/MovieReviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
