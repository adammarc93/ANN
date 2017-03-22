using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IAnnContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Announcement> Announcements { get; set; }
        DbSet<User> User { get; set; }
        DbSet<Announcement_Category> Announcement_Category { get; set; }

        int SaveChanges();
        Database Database { get; }

        //To be able to use the Entry properties in AnnouncementRepo.cs
        DbEntityEntry Entry(object entity);
    }
}
