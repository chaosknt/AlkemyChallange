﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlkemyChallange.Data;
using AlkemyChallange.Models;
using AlkemyChallange.ViewModels;

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
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "LastName");

            return View();
        }
             
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectViewModel model)
        {
            Subject subject = new Subject();
            CreateSubject(model, subject);                        
            _context.Add(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            
        }

        private void CreateSubject(SubjectViewModel model, Subject subject)
        {
            subject.SubjectId = Guid.NewGuid();
            subject.Name = model.Name;
            subject.DayOfTheWeekId = model.DayOfTheWeekId;
            subject.Hour = model.Hour;
            subject.TeacherId = model.TeacherId;
            subject.MaxStudents = model.MaxStudents;
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
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "LastName", subject.TeacherId);
            return View(subject);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Guid SubjectId, string Name, Guid DayOfTheWeekId, DateTime Hour, Guid TeacherId, int MaxStudents)
        {
            Subject subject = _context.Subjects.Find(SubjectId);

            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            editSubject(subject, Name, DayOfTheWeekId, Hour, TeacherId, MaxStudents);
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private void editSubject(Subject subject, string Name, Guid DayOfTheWeekId, DateTime Hour, Guid TeacherId, int MaxStudents)
        {
            subject.Name = Name;
            subject.DayOfTheWeekId = DayOfTheWeekId;
            subject.Hour = Hour;
            subject.TeacherId = TeacherId;
            subject.MaxStudents = MaxStudents;
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