using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Repository.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Announcements = new HashSet<Announcement>();
        }

        //Add Name and LastName fields
        public string Name { get; set; }
        public string LastName { get; set; }
        
        //This fiels will be not created in the database
        #region additional notmapped field

        [NotMapped]
        [Display(Name = "Pan/Pani:")]
        public string FullLastName
        {
            get { return Name + " " + LastName; }
        }

        #endregion

        public virtual ICollection<Announcement> Announcements { get; private set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }
}