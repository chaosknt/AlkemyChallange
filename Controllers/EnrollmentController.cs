﻿using AlkemyChallange.Data;
using AlkemyChallange.Migrations;
using AlkemyChallange.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnrolledStudents = AlkemyChallange.Models.EnrolledStudents;

namespace AlkemyChallange.Controllers
{
    public class EnrollmentController : Controller
    {
        private Context _context;
        public EnrollmentController(Context context)
        {
            _context = context;
        }
        
        public IActionResult Enroll(Guid id)//subject id
        {           
            if (id == null)
            {
                TempData["ErrorSubject"] = AlertMessages.SubjectError + " La materia especificada no existe.";              
                return RedirectToAction("index", "Home");
            }
            var canEnroll = CanEnroll(id);

            if (!canEnroll)
            {
                TempData["ErrorSubject"] = AlertMessages.SubjectError + "Ya estas anotado en esa materia o no hay más cupo.";
                return RedirectToAction("index", "Home");
            }

           EnrollStudent(id);
           UpdateSubject(id);
           TempData["Alert"] = AlertMessages.SubjectSuccess;
           return RedirectToAction("index", "Home");
        }
        private void EnrollStudent(Guid id)//subject id
        {
            var studentLoggedIn = _context.UserAccs.FirstOrDefault(s => s.DNI == User.Identity.Name);
            EnrolledStudents student = new EnrolledStudents();           
            student.StudentId = studentLoggedIn.Id;
            student.SubjectId = id;
            student.EnrolledStudentsId = new Guid();

            _context.EnrolledStudents.Add(student);
            _context.SaveChanges();

        }

        private void UpdateSubject(Guid id)//subject id{
        {
            Subject subject = _context.Subjects.Find(id);
            subject.MaxStudents--;

            _context.Subjects.Update(subject);
            _context.SaveChanges();
        }

        private Boolean CanEnroll(Guid id)//subject id
        {
            var student = _context.UserAccs.FirstOrDefault(s => s.DNI == User.Identity.Name);
            var subjects = _context.EnrolledStudents.Where(s => s.SubjectId == id).ToList();         
            
            if(subjects.Count == 0)
            {
                return true;
            }

            var subject = _context.Subjects.Find(id);

            if(subject.MaxStudents <= 0)
            {
                return false;
            }

            foreach (var item in subjects)
            {
                if(item.StudentId == student.Id)
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}