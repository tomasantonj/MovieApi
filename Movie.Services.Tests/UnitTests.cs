using Xunit;
using Moq;
using Movie.Services;
using Movie.Core.Models;
using Movie.Core.DTOs;
using Movie.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Contracts;
using MovieApi.Movie.Data.Repositories;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Movie.Services.Tests
{
    public class ActorServiceTests
    {
        [Fact]
        public async Task GetActorsAsync_ReturnsActors()
        {
            var options = new DbContextOptionsBuilder<MovieApiContext>()
                .UseInMemoryDatabase(databaseName: "GetActorsAsyncTestDb")
                .Options;
            using var context = new MovieApiContext(options);
            context.Actors.Add(new Actor { Name = "Test Actor" });
            context.SaveChanges();
            var service = new ActorService(context);
            var result = await service.GetActorsAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateActorAsync_AddsActor()
        {
            var options = new DbContextOptionsBuilder<MovieApiContext>()
                .UseInMemoryDatabase(databaseName: "CreateActorAsyncTestDb")
                .Options;
            using var context = new MovieApiContext(options);
            var service = new ActorService(context);
            var dto = new ActorCreateDto { Name = "New Actor" };
            var actor = await service.CreateActorAsync(dto);
            Assert.Equal("New Actor", actor.Name);
            Assert.NotEqual(0, actor.Id);
        }
    }

    public class BusinessRulesServiceTests
    {
        [Fact]
        public void IsBudgetValid_ReturnsTrueForNonNegative()
        {
            var service = new BusinessRulesService(null);
            Assert.True(service.IsBudgetValid(100));
            Assert.True(service.IsBudgetValid(0));
        }

        [Fact]
        public void IsBudgetValid_ReturnsFalseForNegative()
        {
            var service = new BusinessRulesService(null);
            Assert.False(service.IsBudgetValid(-1));
        }
    }

    public class MovieServiceTests
    {
        [Fact]
        public async Task GetMovie_ReturnsNull_WhenMovieNotFound()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.VideoMovies.GetAsync(It.IsAny<int>())).ReturnsAsync((VideoMovie)null);
            var service = new MovieService(mockUnitOfWork.Object);
            var result = await service.GetMovie(1);
            Assert.Null(result);
        }
    }
}
