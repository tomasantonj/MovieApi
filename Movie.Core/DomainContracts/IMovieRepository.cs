using Movie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Movie.Core.DomainContracts
{
    public interface IVideoMovieRepository
    {
        Task<IEnumerable<VideoMovie>> GetAllAsync();
        Task<VideoMovie> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(VideoMovie movie);
        void Update(VideoMovie movie);
        void Remove(VideoMovie movie);
    }

    public interface IReviewRepository
    {
        Task<IEnumerable<MovieReview>> GetAllAsync();
        Task<MovieReview> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(MovieReview review);
        void Update(MovieReview review);
        void Remove(MovieReview review);
    }

    public interface IActorRepository
    {
        Task<IEnumerable<Actor>> GetAllAsync();
        Task<Actor> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(Actor actor);
        void Update(Actor actor);
        void Remove(Actor actor);
    }
}
