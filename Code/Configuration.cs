    namespace JobPlacementDashboard.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using JobPlacementDashboard.Models;
    using JobPlacementDashboard_2.Enums;
    using JobPlacementDashboard_2.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<JobPlacementDashboard.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
                
        protected override void Seed(JobPlacementDashboard.Models.ApplicationDbContext context)
        {
            ClearDatabase(context);
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating duplicate seed data.

            // fy: adding this stub to create a default 'Not Assigned' location for initializing students
            var locations = new List<JPStudentLocation>
            {
                new JPStudentLocation { Location_id = Guid.NewGuid(), Location_name = "Not Assigned" }
            };
            locations.ForEach(x => context.JPStudentLocations.Add(x));
            context.SaveChanges();

            // Add 5 sample bulletins with no null fields
            var bulletins = new List<JPBulletin>
            {
                new JPBulletin { Bulletin_category = BulletinCategory.Advice,
                                 Bulletin_body = "This is a short bulletin body." },
                new JPBulletin { Bulletin_category = BulletinCategory.Event,
                                 Bulletin_body = "This is a long bulletin body about an event that we recommend all Tech Academy students go to. " +
                                                 "It will be held at the Portland campus and there will be 5 surprise guest speakers from the Tech industry."},
                new JPBulletin { Bulletin_category = BulletinCategory.JobPost,
                                 Bulletin_body = "You want a job?  Come get it!" },
                new JPBulletin { Bulletin_category = BulletinCategory.Advice,
                                 Bulletin_body = "Here is some pretty handy advice about whatever you want it to be about." },
                new JPBulletin { Bulletin_category = BulletinCategory.Event,
                                 Bulletin_body = "You like baby animals?  Come to this event to snuggle all the fluffy kittens, puppies, and more!" }
            };
            bulletins.ForEach(x => context.JPBulletins.Add(x));
            context.SaveChanges();

            CreateAdmin(context);

            //kc: may end up needing to include AddtoRole code (referenced stackoverflow: https://stackoverflow.com/questions/19280527/mvc-5-seed-users-and-roles)
            // Add 1 admin user
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var admin = new ApplicationUser { UserName = "admin0@email.com", Email = "admin0@email.com" };
            manager.Create(admin, "Admin123!");
            manager.AddToRole(admin.Id, "Admin");// this is the line that makes him an admin

            //add 3 sample users w / corresponding students and checklists + 1 default message for each, latest contacts
            var users = new List<ApplicationUser>();
            var students = new List<JPStudent>();
            var checklists = new List<JPChecklist>();
            var messages = new List<JPMessage>();
            var latestContacts = new List<JPLatestContact>();

            for (int i = 0; i < 3; i++)
            {
                var email = "user" + i + "@email.com";
                if (!context.Users.Any(u => u.UserName == email))
                {
                    users.Add(new ApplicationUser
                    {
                        UserName = email,
                        Email = email
                    });
                    // kc: if statements included to dynamically create different variations of student completions
                    // kc: could also do this with pre-staged lists, but would be harder to maintain with larger # of seed students
                    students.Add(new JPStudent
                    {
                        ApplicationUser = users[i],
                        Start_date = new DateTime(2018, 01, 01),
                        LinkedIn = (i % 2 == 0) ? "http://" : "",
                        Portfolio = (i < 2) ? "http://" : "",
                        GitHub = "http://",
                        JPContact = (i < 2) ? true : false,
                        Graduated = (i < 2) ? true : false,
                        Hired = (i < 1) ? true : false
                    });
                    checklists.Add(new JPChecklist
                    {
                        JPStudent = students[i],
                        GitHub = (students[i].GitHub == "") ? false : true,
                        LinkedIn = (students[i].LinkedIn == "") ? false : true,
                        Portfolio = (students[i].Portfolio == "") ? false : true,
                        Cover_letter = (i < 2) ? true : false,
                        Resume = (i < 3) ? true : false,
                        Interview_questions = (i < 1) ? true : false,
                        Mock_interview = (i < 1) ? true : false,
                        JPCourse_completion = (i < 2) ? true : false
                    });
                    messages.Add(new JPMessage
                    {
                        Student_id = students[i].Student_id,
                        Graduate = students[i].Graduated,
                        Hire = students[i].Hired,
                        Checklist = true
                    });
                    latestContacts.Add(new JPLatestContact
                    {
                        Latest_contact_id = Guid.NewGuid(),
                        Latest_contact_date = new DateTime(2019, 01, 01),
                        Student_id = students[i].Student_id
                    });
                }
            }
            users.ForEach(x => manager.Create(x, "Password123!"));
            students.ForEach(x => context.JPStudents.Add(x));
            checklists.ForEach(x => context.JPChecklists.Add(x));
            messages.ForEach(x => context.JPMessages.Add(x));
            latestContacts.ForEach(x => context.JPLatestContacts.Add(x));
            context.SaveChanges();

            // Add 3 JPOutsideContacts
            var outsideContacts = new List<JPOutsideContact>();
            for (int i = 0; i < 3; i++)
            {
                outsideContacts.Add(new JPOutsideContact()
                {
                    Name = "Contact " + i,
                    Position = "Position " + i,
                    Company = "Company " + i,
                    CompanyURL = "http://",
                    LinkedIn = "http://",
                    Location = "Company Address " + i,
                    Contact = "Contact " + i,
                    Stack = "Stack " + i
                });
             }
            outsideContacts.ForEach(x => context.JPOutsideContacts.Add(x));
            context.SaveChanges();

            //Add 3 meetup groups
            var meetupGroups = new List<JPMeetupGroup>
            {
                new JPMeetupGroup { Group_url = "https://www.meetup.com/Women-Who-Code-Boulder-Denver/" },
                new JPMeetupGroup { Group_url = "https://www.meetup.com/Bootcampers-Collective/" },
                new JPMeetupGroup { Group_url = "https://www.meetup.com/CodeForDenver/" }
             };
            meetupGroups.ForEach(x => context.JPMeetupGroups.Add(x));
            context.SaveChanges();

            //Add 3 applications
            var applications = new List<JPApplication>();
            var n = 3; //# of seed applications
            for (int i = 0; i < n; i++)
            {
                // modular used to dynamically assign properties, creating variation, but also allowing for multiple students at one company.
                // this setup is overkill considering only 3 seeds, however it's setting up for expansion of many seeds
                applications.Add(new JPApplication()
                {
                    //dynamically ensures there will be multiple applications for some companies, 
                    //expands the company pool as # of seed applications grows 
                    Company_name = "Company " + i%(int)Math.Floor(.8*n),
                    Job_title = "Title " + i%3, 
                    JPJob_catagory = i%4, 
                    Company_city = "City " + i % (int)Math.Floor(.8 * n), //for simplicity, city options aligns with company options
                    State_code = "ST",
                    Application_date = DateTime.Now,
                    Student_id = students[i%(students.Count-1)].Student_id, //-1 to leave one student with 0 applications
                    Student_location = locations[i%locations.Count],
                    Heard_back = (i < (int)Math.Floor(.7 * n)) ? true:false, //~2/3 of applications will have heard back
                    Interview = (i < (int)Math.Floor(.35 * n)) ? true:false //~half of the heard back applications will have interview
                });
            }
            applications.ForEach(x => context.JPApplications.Add(x));
            context.SaveChanges();

            base.Seed(context);

            //Add 3 hires
            Random rnd = new Random(); //used for salary below
            var hires = new List<JPHire>();
            n = 3; //# of seed hires

            for (int i = 0; i < n; i++)
            {
                // modular used to dynamically assign properties, creating variation, but also allowing for multiple students at one company.
                // this setup is overkill considering only 3 seeds, however it's setting up for expansion of many seeds
                hires.Add(new JPHire()
                {
                    //dynamically ensures there will be multiple hires for some companies, 
                    //expands the company pool as # of seed hires grows
                    Company_name = "Company " + i % (int)Math.Floor(.9 * n),
                    Job_title = "Title " + i % 3,
                    JobCategory = (JobCategory)(i% (Enum.GetNames(typeof(JobCategory)).Length)),
                    Salary = rnd.Next(30000, 90000),
                    Company_city = "City " + i % (int)Math.Floor(.9 * n), //for simplicity, city options aligns with company options
                    State_code = "ST",
                    Careers_page = "http://",
                    Hire_date = DateTime.Now,
                    Student_id = students[i % (students.Count - 1)].Student_id, //-1 to leave one student with 0 applications
                    JPStudentLocation = locations[i % locations.Count]
                });
            }
            hires.ForEach(x => context.JPHires.Add(x));
            context.SaveChanges();

            base.Seed(context);
        }

        public void CreateAdmin(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)); //role manager keeps track of user roles

            if (!roleManager.RoleExists("Admin")) //only create admin role if it doesnt already exist
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
        }

        public void ClearDatabase(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM [JPChecklists]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPStudents]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPBulletins]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPMessages]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPMeetupGroups]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPMeetupEvents]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPOutsideContacts]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPApplications]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPHires]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPStudentLocations]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPLatestContacts]");
            context.Database.ExecuteSqlCommand("DELETE FROM [AspNetUsers]");
            context.Database.ExecuteSqlCommand("DELETE FROM [JPLatestContacts]");
        }

    }
}
