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
        private readonly IUserRepository _userRepository;
        private static Note currSelectedNote;

        public static List<string> ExtractEmails(string emails)
        {
            List<string> result = new List<string>();

            while (true)
            {
                int position_of_at = emails.IndexOf("@");

                if (position_of_at == -1)
                {
                    break;
                }

                int position_of_comma = emails.IndexOf(",", position_of_at);

                if (position_of_comma == -1)
                {
                    result.Add(emails);
                    break;
                }

                string email = emails.Substring(0, position_of_comma);

                result.Add(email);

                emails = emails.Substring(position_of_comma + 1);

            }

            return result;
        }

        public NoteController(INoteRepository noteRepository, ICategoryRepository categoryRepository, IShareRepository shareRepository, IUserRepository userRepository)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
            _shareRepository = shareRepository;
            _userRepository = userRepository;
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

                List<string> mailsToShare = ExtractEmails(note.ShareMails);
                if(mailsToShare.Count > 0)
                {
                    foreach(string mail in mailsToShare)
                    {
                        Users shareUser = _userRepository.GetUserOnlyEmail(mail);
                        
                        if ( shareUser != null)
                        {
                            //Share share = new Share();
                            //share.isWritable = note.isWritable;
                            //share.UserId = (int)note.UserId;
                            //share.NoteId = note.NoteId;
                            //share.ToShareUserId = shareUser.Id;
                            //_shareRepository.Add(share);
                            int isWritable = 0;
                            if (note.isWritable)
                            {
                                isWritable = 1;
                            }

                            _shareRepository.Add($"insert into dbo.Share (UserId,NoteId,isWritable,ToShareUserId) values ({note.UserId},{note.NoteId},{isWritable},{shareUser.Id})");
                        }

                    }
                }

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
