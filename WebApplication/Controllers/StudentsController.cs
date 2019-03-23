﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.AppContext;
using WebApplication.Entities;

namespace WebApplication.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? groupId)
        {
            if (groupId != null)
            {
                return View(await _context.Students.Where(g => g.GroupId == groupId).Include(g => g.Group).ToListAsync());
            }
            var appDbContext = _context.Students.Include(s => s.Group);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Group)
                .Include(m => m.Marks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            // Данные по оценкам
            ViewBag.TeacherSubject = await _context.Mark
                .Include(m => m.TeacherSubject)
                .Include(t => t.TeacherSubject.Teacher)
                .Include(s => s.TeacherSubject.Subject)
                .ToListAsync();

            if (student.Marks.Count > 0)
            {
                ViewBag.Rating = ComputeRating(id);
            }

            return View(student);
        }
        // Вычисление рейтинга студента (по его id)
        private dynamic ComputeRating(int? id)
        {
            var student = _context.Students
                .FirstOrDefault(s => s.Id == id);

            var students = _context.Students
                .Where(s => s.GroupId == student.GroupId);


            // id студента / средняя оценка.
            Dictionary<int, double> dictStudIdAvgMark
                = new Dictionary<int, double>();
            foreach (var item in students)
            {
                dictStudIdAvgMark.Add(
                    item.Id,
                    item.Marks.Average(m => m.Value)
                    );
            }
            // Сортированный список id студентов (по рейтингу)
            List<int> listId = 
                (from entry in dictStudIdAvgMark
                orderby entry.Value
                descending
                select entry.Key)
                .ToList();

            int rating = listId.IndexOf((int)id);

            return ++rating;
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,GroupId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name", student.GroupId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name", student.GroupId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,GroupId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name", student.GroupId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
