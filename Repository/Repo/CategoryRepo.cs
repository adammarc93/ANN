using Repository.IRepo;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Repository.Repo
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly IAnnContext _db;
        public CategoryRepo(IAnnContext db)
        {
            _db = db;
        }

        public IQueryable<Category> GetCategories()
        {
            _db.Database.Log = message => Trace.WriteLine(message);
            var categories = _db.Categories.AsNoTracking();
            return categories;
        }

        public IQueryable<Announcement> GetAnnouncementsFromCategory(int id)
        {
            _db.Database.Log = message => Trace.WriteLine(message);
            var announcements = from o in _db.Announcements
                                join k in _db.Announcement_Category on o.Id equals k.Id
                                where k.CategoryId == id
                                select o;

            return announcements;
        }

        public string NameForCategory(int id)
        {
            var name = _db.Categories.Find(id).Name;
            return name;
        }
    }
}