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
using Microsoft.AspNetCore.Mvc.Filters;

namespace KeepNotes.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private static Category currSelectedCategory;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
            IEnumerable<Category> categories = _categoryRepository.GetAllCategories(HomeController.currUser.Id);
            ViewBag.categories = categories;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.GetFieldValidationState("Name") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {
                if (category.Name != null)
                {
                    category.UserId = HomeController.currUser.Id;
                    Category newCategory = _categoryRepository.Add(category);
                    //currUser = newuser;
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = _categoryRepository.GetCategory(id, HomeController.currUser.Id);
            ViewBag.category = category;
            currSelectedCategory = category;
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.GetFieldValidationState("Name") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {
                if (category.Name != null)
                {
                    category.UserId = currSelectedCategory.UserId;
                    category.CategoryId = currSelectedCategory.CategoryId;

                    Category newCategory = _categoryRepository.Update(category);
                    //currUser = newuser;
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category category = _categoryRepository.Delete(id);
            return RedirectToAction("Index");
        }


    }
}
