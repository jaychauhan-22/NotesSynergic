using KeepNotes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Security.Principal;

namespace KeepNotes.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(Users user)
        {
            if (ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {
                if (user.Email != null && user.Password != null)
                {
                    Users validateuser = _userRepository.GetUserEmailPassword(user.Email,user.Password);
                    if (validateuser != null)
                    {
                        ViewBag.User = validateuser;
                        TempData["User"] = validateuser.Id.ToString();
                        return RedirectToAction("Home");
                    }
                    else
                    {
                        ViewData["InvalidUser"] = "Invalid Email Address or Password";

                    }
                }
                else if (user.Email != null)
                {
                    Users validateuser = _userRepository.GetUserOnlyEmail(user.Email);
                    if (validateuser != null)
                    {
                        ViewBag.User = validateuser;
                        TempData["User"] = validateuser.Id.ToString();
                        return RedirectToAction("Home");
                    }
                    else
                    {
                        ViewData["InvalidUser"] = "Invalid Email Address";

                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(Users user)
        {
            if (ModelState.IsValid)
            {
                Users newuser = _userRepository.Add(user);
                return RedirectToAction("Home");
            }
            return View();
        }
        [HttpGet]
        public ViewResult Home()
        {
            String Id = TempData["User"] as String;
            int id = Int32.Parse(Id);
            Users newuser = _userRepository.GetUserFromId(id);
            ViewBag.User = newuser;
            return View();
        }
    }
}
