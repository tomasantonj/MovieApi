using Movie.Core.DTOs;
using Movie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movie.Contracts
{
    public interface IMovieService
    {
        Task<PagedResponse<VideoMovieDto>> GetMovies(int? genreId, int? year, int? directorId, int page, int pageSize);
        Task<VideoMovieDto?> GetMovie(int id);
        Task<PagedResponse<ReviewDto>> GetReviewsForMovie(int movieId, int page, int pageSize);
        Task<MovieDetailDto?> GetMovieDetails(int id);
        Task<bool> UpdateMovie(int id, VideoMovieUpdateDto dto);
        Task<VideoMovieDto?> CreateMovie(VideoMovieCreateDto dto);
        Task<bool> DeleteMovie(int id);
        Task<bool> PatchMovie(int id, VideoMoviePatchDto patchDto);
        Task<bool> AddActorToMovie(int movieId, int actorId);
    }
}
