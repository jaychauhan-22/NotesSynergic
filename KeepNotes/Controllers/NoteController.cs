using KeepNotes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Security.Principal;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeepNotes.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using CollegeApp.Modals;

namespace KeepNotes.Controllers
{
    public class NoteController : Controller
    {
        private readonly INoteRepository _noteRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IShareRepository _shareRepository;
        private static Note currSelectedNote;

        public NoteController(INoteRepository noteRepository, ICategoryRepository categoryRepository)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Note> notes = _noteRepository.GetAllNotes(HomeController.currUser.Id);
            ViewBag.notes = notes;
            IEnumerable<Category> categories = _categoryRepository.GetAllCategories(HomeController.currUser.Id);
            ViewBag.categories = categories;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAllCategories(HomeController.currUser.Id);
            ViewBag.categories = categories;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {            
                note.UserId = HomeController.currUser.Id;
                Note newNote = _noteRepository.Add(note);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Note note = _noteRepository.GetNote(id);
            ViewBag.note = note;
            currSelectedNote = note;
            IEnumerable<Category> categories = _categoryRepository.GetAllCategories(HomeController.currUser.Id);
            ViewBag.categories = categories;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name",note.CategoryId);
            return View(note);
        }
        [HttpPost]
        public IActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                    note.UserId = currSelectedNote.UserId;
                    note.NoteId = currSelectedNote.NoteId;
                    Note newNote = _noteRepository.Update(note);
                    //currUser = newuser;
                    return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Note note = _noteRepository.Delete(id);
            return RedirectToAction("Index");
        }


    }
}
