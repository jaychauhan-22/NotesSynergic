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

namespace KeepNotes.Controllers
{
    public class NoteController : Controller
    {
        private readonly INoteRepository _noteRepository;
        private static Note currSelectedNote;

        public NoteController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Note> notes = _noteRepository.GetAllNotes(HomeController.currUser.Id);
            ViewBag.notes = notes;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {            
                    note.UserId = HomeController.currUser.Id;
                    Note newNote = _noteRepository.Add(note);
                    //currUser = newuser;
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
