using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Core.Models;
using Movie.Core.DTOs;
using Movie.Contracts;


namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
        {
            var actors = await _actorService.GetActorsAsync();
            return Ok(actors);
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
                return NotFound();
            return Ok(actor);
        }

        // PUT: api/Actors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, Actor actor)
        {
            var updated = await _actorService.UpdateActorAsync(id, actor);
            if (!updated)
                return BadRequest();
            return NoContent();
        }

        // POST: api/Actors
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor([FromBody] ActorCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var actor = await _actorService.CreateActorAsync(dto);
            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var deleted = await _actorService.DeleteActorAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
