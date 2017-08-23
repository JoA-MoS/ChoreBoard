using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChoreBoardAPI.Data;
using ChoreBoardAPI.Models;

namespace ChoreBoardAPI.Controllers.api
{
    [Produces("application/json")]
    [Route("api/Chores")]
    public class ChoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Chores
        [HttpGet]
        public IEnumerable<Chore> GetChores()
        {
            return _context.Chores;
        }

        // GET: /api/boards/{boardId}/Chores
        [HttpGet("~/api/boards/{boardId:int}/chores")]
        public IEnumerable<Chore> GetBoardChores([FromRoute] int boardId)
        {
            return _context.Chores.Where(c => c.BoardId == boardId);
        }



        // GET: api/Chores/5
        [HttpGet("{id}")]
        [HttpGet("~/api/boards/{boardId:int}/chores/{id}")]
        public async Task<IActionResult> GetChore([FromRoute] int id)
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

        // PUT: api/Chores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChore([FromRoute] int id, [FromBody] Chore chore)
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

        // POST: api/Chores
        [HttpPost]
        public async Task<IActionResult> PostChore([FromBody] Chore chore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Chores.Add(chore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChore", new { id = chore.ChoreId }, chore);
        }

        // DELETE: api/Chores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChore([FromRoute] int id)
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

        private bool ChoreExists(int id)
        {
            return _context.Chores.Any(e => e.ChoreId == id);
        }
    }
}