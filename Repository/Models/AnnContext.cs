using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using Repository.IRepo;

namespace Repository.Models
{
    //IdentityModels to AnnContext

    //Before changes
    //public class ApplicationDbContext : IdentityDbContext<User>
    //{
    //    public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}


    //After changes
    public class AnnContext : IdentityDbContext, IAnnContext
    {
        public AnnContext() : base("DefaultConnection")
        {
        }

        public static AnnContext Create()
        {
            return new AnnContext();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Announcement_Category> Announcement_Category { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Necessary for Identity class
            base.OnModelCreating(modelBuilder);

            //turn off convention, which creating prular for table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //CascadeDelete will be turn off by Fluent API
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Fluent API is used to determinate reletionship between the tabes
            //and turn on CascadeDelete for that reletionship
            modelBuilder.Entity<Announcement>().HasRequired(x => x.User).WithMany(x => x.Announcements)
                .HasForeignKey(x => x.UserId).WillCascadeOnDelete(true);
        }

    }
}