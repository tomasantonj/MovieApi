using Movie.Core.Domain.Models;
using Movie.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movie.Contracts
{
    public interface IActorService
    {
        Task<IEnumerable<Actor>> GetActorsAsync();
        Task<Actor?> GetActorByIdAsync(int id);
        Task<Actor> CreateActorAsync(ActorCreateDto dto);
        Task<bool> UpdateActorAsync(int id, Actor actor);
        Task<bool> DeleteActorAsync(int id);
    }
}
