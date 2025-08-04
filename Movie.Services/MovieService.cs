using Movie.Contracts;
using Movie.Core.DTOs;
using Movie.Core.Models;
using MovieApi.Movie.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Movie.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<VideoMovieDto>> GetMovies(int? genreId, int? year, int? directorId, int page, int pageSize)
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

        public async Task<VideoMovieDto?> GetMovie(int id)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return null;
            return new VideoMovieDto
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
        }

        public async Task<PagedResponse<ReviewDto>> GetReviewsForMovie(int movieId, int page, int pageSize)
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

        public async Task<MovieDetailDto?> GetMovieDetails(int id)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return null;
            return new MovieDetailDto
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
        }

        public async Task<bool> UpdateMovie(int id, VideoMovieUpdateDto dto)
        {
            if (id != dto.Id)
                return false;
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return false;
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.DirectorId = dto.DirectorId;
            movie.Duration = dto.Duration;
            _unitOfWork.VideoMovies.Update(movie);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<VideoMovieDto?> CreateMovie(VideoMovieCreateDto dto)
        {
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
            return new VideoMovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                GenreId = movie.GenreId,
                GenreName = string.Empty,
                DirectorId = movie.DirectorId,
                DirectorName = string.Empty,
                Duration = movie.Duration
            };
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return false;
            _unitOfWork.VideoMovies.Remove(movie);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> PatchMovie(int id, VideoMoviePatchDto patchDto)
        {
            if (patchDto == null)
                return false;
            var movie = await _unitOfWork.VideoMovies.GetAsync(id);
            if (movie == null)
                return false;
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
            return true;
        }

        public async Task<bool> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _unitOfWork.VideoMovies.GetAsync(movieId);
            if (movie == null)
                return false;

            return true;
        }
    }
}
