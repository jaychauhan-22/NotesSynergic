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
using Microsoft.AspNetCore.Mvc.Filters;

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

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.User = HomeController.currUser;
            ViewBag.isPublic = HomeController.ispublic;
            ViewBag.isLogout = HomeController.isLogout;
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
        public IActionResult SharedNotes()
        {
            IEnumerable<Share> share = _shareRepository.GetAllSharedNotes(HomeController.currUser.Id);
            List<Note> notes = new List<Note>();
            List<Category> notes_category = new List<Category>();
            foreach (Share s in share)
            {
                Note note = _noteRepository.GetNote((int)s.NoteId);
                Category category= _categoryRepository.GetCategory((int)note.CategoryId,(int)s.UserId);
                notes.Add(note);
                notes_category.Add(category);
            }
            ViewBag.share = share;
            ViewBag.note = notes;
            ViewBag.categories = notes_category;
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
                List<string> mailsToShare = new List<string>();
                int flagcheck = 0;
                if (note.ShareMails != null)
                {
                    mailsToShare = ExtractEmails(note.ShareMails);
                    flagcheck = 1;
                }
                if(mailsToShare.Count > 0 && flagcheck!=0)
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
            int userid = Int32.Parse( note.UserId.ToString());
            IEnumerable<Share> share = _shareRepository.GetAllShares(id, userid);
            List<string> sharedusername = new List<string>();
            List<int> shareduserid = new List<int>();
            foreach (var shareid in share)
            {
                Users user = _userRepository.GetUserFromId(Int32.Parse(shareid.ToShareUserId.ToString()));
                sharedusername.Add(user.Username);
                shareduserid.Add(user.Id);
            }
            ViewBag.shareusers = sharedusername;
            ViewBag.shareuserid = shareduserid;
            ViewBag.share = share;
            return View(note);
        }
        [HttpPost]
        public IActionResult Edit(Note note,string[] shareduserid,string sharenote)
        {
            if (ModelState.IsValid)
            {
                note.UserId = currSelectedNote.UserId;
                
                note.NoteId = currSelectedNote.NoteId;
                for (int i=0;i<shareduserid.Length;i++)
                {
                    _shareRepository.Delete($"delete from dbo.share where UserId = {note.UserId} and NoteId = {(int)note.NoteId} and ToShareUserId = {shareduserid[i]}");
                }

                Note newNote = _noteRepository.Update(note);
                List<string> mailsToShare = new List<string>();
                if (note.ShareMails!=null)
                {
                    mailsToShare = ExtractEmails(note.ShareMails);
                }
                IEnumerable<Share> sharednotes = _shareRepository.GetAllShares(note.NoteId,(int)note.UserId);
                Users dummyuser = _userRepository.GetUserOnlyEmail(note.ShareMails);

                int emailexists = 0;
                foreach (Share share in sharednotes)
                {
                    if(dummyuser != null && share.ToShareUserId == dummyuser.Id)
                    {
                        emailexists = 1;
                    }
                }
                if (mailsToShare.Count > 0 && emailexists == 0)
                {
                    foreach (string mail in mailsToShare)
                    {
                        Users shareUser = _userRepository.GetUserOnlyEmail(mail);

                        if (shareUser != null)
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
                //currUser = newuser;
                return RedirectToAction("Index");
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult SharedEdit(int id)
        {
            Note note = _noteRepository.GetNote(id);

            ViewBag.note = note;
            currSelectedNote = note;
            IEnumerable<Category> categories = _categoryRepository.GetAllCategories(HomeController.currUser.Id);
            ViewBag.categories = categories;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name", note.CategoryId);
            int userid = Int32.Parse(note.UserId.ToString());
            IEnumerable<Share> share = _shareRepository.GetAllShares(id, userid);
            List<string> sharedusername = new List<string>();
            List<int> shareduserid = new List<int>();
            foreach (var shareid in share)
            {
                Users user = _userRepository.GetUserFromId(Int32.Parse(shareid.ToShareUserId.ToString()));
                sharedusername.Add(user.Username);
                shareduserid.Add(user.Id);
            }
            ViewBag.shareusers = sharedusername;
            ViewBag.shareuserid = shareduserid;
            ViewBag.share = share;
            return View(note);
        }
        [HttpPost]
        public IActionResult SharedEdit(Note note, string[] shareduserid, string sharenote)
        {
            if (ModelState.IsValid)
            {
                note.UserId = currSelectedNote.UserId;
                note.NoteId = currSelectedNote.NoteId;
                Note oldnote = _noteRepository.GetNote(note.NoteId);
                oldnote.Title = note.Title;
                oldnote.Text = note.Text;
                Note newnote = _noteRepository.Update(oldnote);
                return RedirectToAction("SharedNotes");
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

