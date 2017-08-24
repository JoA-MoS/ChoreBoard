using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChoreBoardAPI.Data;
using ChoreBoardAPI.Models;

namespace ChoreBoardAPI.Controllers
{
    public class ChoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Chores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Chores.Include(c => c.Board).Include(c => c.CreatedBy).Include(c => c.ModifiedBy).Include(c => c.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Chores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.Board)
                .Include(c => c.CreatedBy)
                .Include(c => c.ModifiedBy)
                .Include(c => c.Owner)
                .SingleOrDefaultAsync(m => m.ChoreId == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // GET: Chores/Create
        public IActionResult Create()
        {
            ViewData["BoardId"] = new SelectList(_context.Boards, "BoardId", "BoardId");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Chores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChoreId,Name,Description,Deadline,Rollover,BoardId,OwnerId,Created,CreatedById,Modified,ModifiedById")] Chore chore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoardId"] = new SelectList(_context.Boards, "BoardId", "BoardId", chore.BoardId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", chore.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", chore.ModifiedById);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", chore.OwnerId);
            return View(chore);
        }

        // GET: Chores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.ChoreId == id);
            if (chore == null)
            {
                return NotFound();
            }
            ViewData["BoardId"] = new SelectList(_context.Boards, "BoardId", "BoardId", chore.BoardId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", chore.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", chore.ModifiedById);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", chore.OwnerId);
            return View(chore);
        }

        // POST: Chores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChoreId,Name,Description,Deadline,Rollover,BoardId,OwnerId,Created,CreatedById,Modified,ModifiedById")] Chore chore)
        {
            if (id != chore.ChoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoreExists(chore.ChoreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoardId"] = new SelectList(_context.Boards, "BoardId", "BoardId", chore.BoardId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", chore.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", chore.ModifiedById);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", chore.OwnerId);
            return View(chore);
        }

        // GET: Chores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.Board)
                .Include(c => c.CreatedBy)
                .Include(c => c.ModifiedBy)
                .Include(c => c.Owner)
                .SingleOrDefaultAsync(m => m.ChoreId == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // POST: Chores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.ChoreId == id);
            _context.Chores.Remove(chore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoreExists(int id)
        {
            return _context.Chores.Any(e => e.ChoreId == id);
        }
    }
}
