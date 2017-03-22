using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Models;
using Repository.IRepo;
using Repository.Models.View;

namespace ANN.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepo _repo;

        public CategoriesController(ICategoryRepo repo)
        {
            _repo = repo;
        }

        // GET: Categories
        public ActionResult Index()
        {
            var categories = _repo.GetCategories().AsNoTracking();
            return View(categories);
        }

        public ActionResult ShowAnnouncements(int id)
        {
            var announcements = _repo.GetAnnouncementsFromCategory(id);
            AnnouncementsFromCategoryViewModels model = new AnnouncementsFromCategoryViewModels();
            model.Announcements = announcements.ToList();
            model.CategoryName = _repo.NameForCategory(id);
            return View(model);
        }

        [Route("JSON")]
        public ActionResult CategoriesInJson()
        {
            var categories = _repo.GetCategories().AsNoTracking();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            return View();
        }
    }
}
