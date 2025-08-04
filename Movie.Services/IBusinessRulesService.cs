using Movie.Core.Models;
using Movie.Core.DTOs;
using Movie.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Movie.Services
{
    public interface IBusinessRulesService
    {
        Task<bool> CanAddReviewToMovie(int movieId);
        Task<bool> CanAssignActorToMovie(int movieId, int actorId);
        bool IsBudgetValid(decimal budget);
        Task<bool> CanAddActorToDocumentary(int movieId);
        Task<bool> IsDocumentaryBudgetValid(int movieId, decimal budget);
    }
}
