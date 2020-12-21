﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlkemyChallange.Data;
using AlkemyChallange.Models;

namespace AlkemyChallange.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly Context _context;

        public SubjectsController(Context context)
        {
            _context = context;
        }
               
        public async Task<IActionResult> Index()
        {
            var context = _context.Subjects.Include(s => s.DayOfTheWeek).Include(s => s.Teacher);
            return View(await context.ToListAsync());
        }
                
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.DayOfTheWeek)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }
              
        public IActionResult Create()
        {
            ViewData["DayOfTheWeekId"] = new SelectList(_context.DayOfTheWeek, "DayOfTheWeekId", "Name");

            var teachers = _context.Teachers.Where(t => t.isActive == true).ToList();

            ViewData["TeacherId"] = new SelectList(teachers, "TeacherId", "LastName");
            return View();
        }
             
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectId,Name,DayOfTheWeekId,Hour,TeacherId,MaxStudents")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.SubjectId = Guid.NewGuid();
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayOfTheWeekId"] = new SelectList(_context.DayOfTheWeek, "DayOfTheWeekId", "Name", subject.DayOfTheWeekId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "DNI", subject.TeacherId);
            return View(subject);
        }
                
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["DayOfTheWeekId"] = new SelectList(_context.DayOfTheWeek, "DayOfTheWeekId", "Name", subject.DayOfTheWeekId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "DNI", subject.TeacherId);
            return View(subject);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SubjectId,Name,DayOfTheWeekId,Hour,TeacherId,MaxStudents")] Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.SubjectId))
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
            ViewData["DayOfTheWeekId"] = new SelectList(_context.DayOfTheWeek, "DayOfTheWeekId", "Name", subject.DayOfTheWeekId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "DNI", subject.TeacherId);
            return View(subject);
        }
               
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.DayOfTheWeek)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(Guid id)
        {
            return _context.Subjects.Any(e => e.SubjectId == id);
        }
    }
}
