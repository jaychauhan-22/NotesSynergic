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
        private static Users currUser;
        private static bool isLogout=false;
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
            if (!isLogout)
            {
                if (ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                {
                    if (user.Email != null && user.Password != null)
                    {
                        Users validateuser = _userRepository.GetUserEmailPassword(user.Email, user.Password);
                        if (validateuser != null)
                        {
                            ViewBag.User = validateuser;
                            currUser = validateuser;
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
                            currUser = validateuser;
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
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult UpdateProfile()
        {
            if (!isLogout)
            {
                Users newuser = _userRepository.GetUserFromId(currUser.Id);
                ViewBag.User = currUser;
                return View(newuser);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateProfile(Users user)
        {
            if (!isLogout)
            {
                if (ModelState.GetFieldValidationState("Username") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid
                    && ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid
                    && ModelState.GetFieldValidationState("Contact") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                {
                    Users newuser = _userRepository.GetUserFromId(currUser.Id);
                    newuser.Username = user.Username;
                    newuser.Email = user.Email;
                    newuser.Contact = user.Contact;
                    Users updateduser = _userRepository.Update(newuser);

                    return RedirectToAction("Home");
                }
                return View();
            }
            return RedirectToAction("Index");
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
                currUser = newuser;
                return RedirectToAction("Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Home()
        {
            if (!isLogout)
            {
                Users newuser = _userRepository.GetUserFromId(currUser.Id);
                ViewBag.User = newuser;
                return View();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            isLogout = true;
            return RedirectToAction("Index");
        }
    }
}
