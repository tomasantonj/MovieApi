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
    public class MovieReviewsController : ControllerBase
    {
        private readonly Movie.Data.MovieApiContext _context;

        public MovieReviewsController(Movie.Data.MovieApiContext context)
        {
            _context = context;
        }

        // ...existing code...
        // (rest of the controller code remains unchanged)
        // ...existing code...
    }
}
