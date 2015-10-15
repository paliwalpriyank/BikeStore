using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using IS7012.ProjectAssignment.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace IS7012.ProjectAssignment.Models
{
    public class ApplicationDbConfiguration : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        
        protected override void Seed(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!RoleManager.RoleExists("Manager"))
            {
                RoleManager.Create(new IdentityRole("Manager"));
            }

            if (!RoleManager.RoleExists("Employee"))
            {
                RoleManager.Create(new IdentityRole("Employee"));
            }

            CreateUserAndRole("manager@mikes.com", "Manager", "P@ssw0rd", UserManager, RoleManager);
            CreateUserAndRole("firstEmployee@mikes.com", "Employee", "P@ssw0rd", UserManager, RoleManager);
            CreateUserAndRole("secondEmployee@mikes.com", "Employee", "P@ssw0rd", UserManager, RoleManager);
            CreateUserAndRole("thirdEmployee@mikes.com", "Employee", "P@ssw0rd", UserManager, RoleManager);

            //TODO: CREATE ADDITIONAL USERS / ROLES HERE

            base.Seed(context);
        }

        private void CreateUserAndRole(string email, string role, string password,
                        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExists(role))
            {
                roleManager.Create(new IdentityRole(role));
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            if (userManager.Create(user, password) == IdentityResult.Success)
            {
                userManager.AddToRole(user.Id, role);
            }
        }
    }
}