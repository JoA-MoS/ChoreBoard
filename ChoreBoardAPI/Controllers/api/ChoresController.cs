using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChoreBoardAPI.Data;
using ChoreBoardAPI.Models;
using Microsoft.AspNetCore.Authorization;
using ChoreBoardAPI.Models.dto;
using System.Security.Claims;

namespace ChoreBoardAPI.Controllers.api
{
    [Produces("application/json")]
    [Route("api/boards/{boardId:int}/chores/")]
    public class ChoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/boards/{boardId:int}/chores
        [HttpGet]
        public IEnumerable<Chore> GetChores([FromRoute] int boardId, [FromQuery] bool includeCompleted=false)
        {
            return _context.Chores.Where(c => c.BoardId == boardId && (c.Completed==false || includeCompleted==true));
        }


     
        // GET: api/boards/{boardId:int}/chores/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetChore([FromRoute] int boardId, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.ChoreId == id);

            if (chore == null)
            {
                return NotFound();
            }

            return Ok(chore);
        }

        // PUT: api/boards/{boardId:int}/chores/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutChore([FromRoute] int boardId, [FromRoute] int id, [FromBody] Chore chore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chore.ChoreId)
            {
                return BadRequest();
            }

            _context.Entry(chore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChoreExists(id))
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

        // https://stackoverflow.com/questions/15875443/getting-date-using-day-of-the-week
        // POST: api/boards/{boardId:int}/chores
        [HttpPost]
        public async Task<IActionResult> PostChore([FromRoute] int boardId, [FromBody] NewChoreDTO newChore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Chore chore = new Chore
            {
                Name = newChore.Name,
                Description = newChore.Description,
                BoardId = boardId
            };

            _context.Chores.Add(chore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChore", new { id = chore.ChoreId }, chore);
        }

        // DELETE: api/boards/{boardId:int}/chores/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteChore([FromRoute] int boardId, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.ChoreId == id);
            if (chore == null)
            {
                return NotFound();
            }

            _context.Chores.Remove(chore);
            await _context.SaveChangesAsync();

            return Ok(chore);
        }



        // Complete Chore: api/boards/{boardId:int}/chores/5/complete
        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> CompleteChore([FromRoute] int boardId, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.ChoreId == id);
            if (chore == null)
            {
                return NotFound();
            }
            chore.Completed = true;
           
            await _context.SaveChangesAsync();

            return Ok(chore);
        }

        // Complete Chore: api/boards/{boardId:int}/chores/5/complete
        [HttpPost("complete")]
        public async Task<IActionResult> CompleteAllChores([FromRoute] int boardId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chores = _context.Chores.Where(c => c.BoardId == boardId);
            if (chores == null)
            {
                return NotFound();
            }
            foreach (Chore chore in chores)
            {
                chore.Completed = true;
            }

            await _context.SaveChangesAsync();

            return Ok(chores);
        }



        // Complete Chore: api/boards/{boardId:int}/chores/5/complete
        [HttpDelete("{id}/complete")]
        public async Task<IActionResult> UnCompleteChore([FromRoute] int boardId, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.ChoreId == id);
            if (chore == null)
            {
                return NotFound();
            }
            chore.Completed = false;

            await _context.SaveChangesAsync();

            return Ok(chore);
        }

        private bool ChoreExists(int id)
        {
            return _context.Chores.Any(e => e.ChoreId == id);
        }
    }
}
