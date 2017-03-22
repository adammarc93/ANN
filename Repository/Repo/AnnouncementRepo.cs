using Repository.IRepo;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Repository.Repo
{
    public class AnnouncementRepo : IAnnouncementRepo
    {
        private readonly IAnnContext _db;

        public AnnouncementRepo(IAnnContext db)
        {
            _db = db;
        }

        public IQueryable<Announcement> GetAnnouncements()
        {
            //To see what request has been sent to database
            _db.Database.Log = message => Trace.WriteLine(message);
            var announcements = _db.Announcements.AsNoTracking();
            return announcements;
        }

        public Announcement GetAnnouncementById(int id)
        {
            Announcement announcement = _db.Announcements.Find(id);
            return announcement;
        }

        //Metod return true when record is save succesfuly to database, false if not
        public void DeleteAnnouncement(int id)
        {
            RemoveLinkAnnouncementCategory(id);
            Announcement announcement = _db.Announcements.Find(id);
            _db.Announcements.Remove(announcement);
        }

        private void RemoveLinkAnnouncementCategory(int idAnnouncement)
        {
            var list = _db.Announcement_Category.Where(o => o.AnnouncementId == idAnnouncement);

            foreach (var el in list)
            {
                _db.Announcement_Category.Remove(el);
            }
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Add(Announcement announcement)
        {
            _db.Announcements.Add(announcement);
        }

        public void Update(Announcement announcement)
        {
            _db.Entry(announcement).State = EntityState.Modified;
        }

        public IQueryable<Announcement> GetPage(int? page = 1, int? pageSize=10)
        {
            var announcements = _db.Announcements
                .OrderByDescending(o => o.DateAdded)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);

            return announcements;
        }
    }
}