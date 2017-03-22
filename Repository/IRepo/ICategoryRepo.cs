using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface ICategoryRepo
    {
        IQueryable<Category> GetCategories();
        IQueryable<Announcement> GetAnnouncementsFromCategory(int id);
        string NameForCategory(int id);
    }
}
