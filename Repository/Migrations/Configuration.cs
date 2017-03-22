namespace Repository.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Repository.Models.AnnContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Repository.Models.AnnContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();

            SeedRoles(context);
            SeedUsers(context);
            SeedAnnouncement(context);
            SeedCategory(context);
            SeedAnnouncement_Category(context);
        }

        private void SeedRoles(AnnContext context)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>
                (new RoleStore<IdentityRole>());

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Worker"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Worker";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(AnnContext context)
        {
            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);

            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var user = new User { UserName = "Admin" };
                var adminresult = manager.Create(user, "12345678");

                if (adminresult.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "Mark"))
            {
                var user = new User { UserName = "mark@AspNetMvc.pl" };
                var adminresult = manager.Create(user, "1234Abc");

                if (adminresult.Succeeded)
                    manager.AddToRole(user.Id, "Worker");
            }

            if (!context.Users.Any(u => u.UserName == "President"))
            {
                var user = new User { UserName = "president@AspNetMvc.pl" };
                var adminresult = manager.Create(user, "1234Abc");

                if (adminresult.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }
        }

        private void SeedAnnouncement(AnnContext context)
        {
            var userId = context.Set<User>().Where(u => u.UserName == "Admin").FirstOrDefault().Id;

            for (int i = 1; i < 10; i++)
            {
                var ann = new Announcement()
                {
                    Id = i,
                    UserId = userId,
                    Content = "Tresc ogloszenia " + i.ToString(),
                    Title = "Tytul ogloszenia " + i.ToString(),
                    DateAdded = DateTime.Now.AddDays(-i)
                };
                context.Set<Announcement>().AddOrUpdate(ann);
            }
            context.SaveChanges();
        }

        private void SeedCategory(AnnContext context)
        {
            for (int i = 1; i < 10; i++)
            {
                var cat = new Category()
                {
                    Id = i,
                    Name = "Nazwa kategorii " + i.ToString(),
                    Content = "Tresc ogloszenia " + i.ToString(),
                    MetaTitle = "Tytul kategorii " + i.ToString(),
                    MetaDescription = "Opis kategorii " + i.ToString(),
                    MetaWords = "Slowa kluczowe do kategorii " + i.ToString(),
                    ParentId = i
                };
                context.Set<Category>().AddOrUpdate(cat);
            }
            context.SaveChanges();
        }

        private void SeedAnnouncement_Category(AnnContext context)
        {
            for (int i = 1; i < 10; i++)
            {
                var acat = new Announcement_Category()
                {
                    Id = i,
                    AnnouncementId = i / 2 + 1,
                    CategoryId = i / 2 + 2,
                };
                context.Set<Announcement_Category>().AddOrUpdate(acat);
            }
            context.SaveChanges();
        }
    }
}