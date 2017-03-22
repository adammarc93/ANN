using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Models;
using System.Diagnostics;
using Repository.Repo;
using Repository.IRepo;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ANN.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementRepo _repo;

        public AnnouncementsController(IAnnouncementRepo repo)
        {
            _repo = repo;
        }

        // GET: Announcements
        public ActionResult Index(int? page, string sortOrder)
        {
            int cureentPage = page ?? 1;
            //Number of announcements on page
            int onPage = 10;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.DateAddedSort = sortOrder == "DateAdded" ? "DateAddedAsc" : "DateAdded";
            ViewBag.ContentSort = sortOrder == "ContentAsc" ? "Content" : "ContentAsc";
            ViewBag.TitleSort = sortOrder == "TitleAsc" ? "Title" : "TitleAsc";

            var announcements = _repo.GetAnnouncements();
            switch (sortOrder)
            {
                case "DateAdded":
                    announcements = announcements.OrderByDescending(s => s.DateAdded);
                    break;
                case "DateAddedAsc":
                    announcements = announcements.OrderBy(s => s.DateAdded);
                    break;
                case "Title":
                    announcements = announcements.OrderByDescending(s => s.Title);
                    break;
                case "TitleAsc":
                    announcements = announcements.OrderBy(s => s.Title);
                    break;
                case "Content":
                    announcements = announcements.OrderByDescending(s => s.Content);
                    break;
                case "ContentAsc":
                    announcements = announcements.OrderBy(s => s.Content);
                    break;
                case "IdAsc":
                    announcements = announcements.OrderBy(s => s.Id);
                    break;
                default:
                    announcements = announcements.OrderByDescending(s => s.Id);
                    break;
            }
            return View(announcements.ToPagedList<Announcement>(cureentPage, onPage));
        }

        // GET: Announcements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = _repo.GetAnnouncementById((int)id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // GET: Announcements/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Content,Title")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.UserId = User.Identity.GetUserId();
                announcement.DateAdded = DateTime.Now;

                try
                {
                    _repo.Add(announcement);
                    _repo.SaveChanges();
                    return RedirectToAction("MyAnnouncements");
                }
                catch
                {
                    return View(announcement);
                }
            }

            return View(announcement);
        }

        // GET: Announcements/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Announcement announcement = _repo.GetAnnouncementById((int)id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            else if (announcement.UserId != User.Identity.GetUserId() && !(User.IsInRole("Admin")) && !(User.IsInRole("Worker")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,Title,DateAdded,UserId")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //To check if error alert is working
                    //announcement.UserId = "fdfdfdf";
                    _repo.Update(announcement);
                    _repo.SaveChanges();
                }
                catch
                {
                    ViewBag.Blad = true;
                    return View(announcement);
                }
            }
            ViewBag.Blad = false;
            return View(announcement);
        }

        // GET: Announcements/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, bool? blad)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Announcement announcement = _repo.GetAnnouncementById((int)id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            else if (announcement.UserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (blad != null)
                ViewBag.Blad = true;

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteAnnouncement(id);
            try
            {
                _repo.SaveChanges();
            }
            catch
            {
                RedirectToAction("Delete", new { id = id, blad = true });
            }

            return RedirectToAction("Index");
        }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }

        //Get:/Announcements/
        public ActionResult Partial(int? page)
        {
            int cureentPage = page ?? 1;
            //Number of announcements on page
            int onPage = 3;

            var announcements = _repo.GetAnnouncements();
            announcements = announcements.OrderByDescending(d => d.DateAdded);

            return PartialView("Index", announcements.ToPagedList<Announcement>(cureentPage, onPage));
        }

        //List of user's announcements
        [OutputCache(Duration =1000)]
        public ActionResult MyAnnouncements(int? page)
        {
            int currentPage = page ?? 1;
            int onPage = 3;
            string userId = User.Identity.GetUserId();
            var announcements = _repo.GetAnnouncements();
            announcements = announcements.OrderByDescending(d => d.DateAdded).Where(o => o.UserId == userId);
            return View(announcements.ToPagedList<Announcement>(currentPage, onPage));
        }
    }
}
