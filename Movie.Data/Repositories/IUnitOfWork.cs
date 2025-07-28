using MovieApi.Movie.Core.DomainContracts;
using System.Threading.Tasks;
using Movie.Data;

namespace MovieApi.Movie.Data.Repositories
{
    public interface IUnitOfWork
    {
        IVideoMovieRepository VideoMovies { get; }
        IReviewRepository Reviews { get; }
        IActorRepository Actors { get; }
        Task CompleteAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieApiContext _context;
        private readonly IVideoMovieRepository _videoMovieRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IActorRepository _actorRepository;
        public IVideoMovieRepository VideoMovies => _videoMovieRepository;
        public IReviewRepository Reviews => _reviewRepository;
        public IActorRepository Actors => _actorRepository;

        public UnitOfWork(MovieApiContext context, IVideoMovieRepository videoMovieRepository, IReviewRepository reviewRepository, IActorRepository actorRepository)
        {
            _context = context;
            _videoMovieRepository = videoMovieRepository;
            _reviewRepository = reviewRepository;
            _actorRepository = actorRepository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
