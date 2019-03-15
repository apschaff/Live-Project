# Live Project
### Introduction
For the past two weeks, I have worked in an AGILE team building a full scale C# MVC web application. As part of this project I worked on front end and backend user stories. This was a large existing project with much of the design and functionality completed but in need of adjustment or modification to clean up code or meet needs. Working on an existing project provided opportunity for me to get up to speed with an existing codebase as well as adding my own modifications to meet the needs of the project and stakeholders. 

I have provided brief descriptions of the work I did on this project below. 

### Back-End Stories
The seed method of this project was becoming congested, therefore two of the stories I worked on involved cleaning up the code within the project seed method. 

All of the DELETE FROM statements were initially created within the Seed method. I removed them from the seed method and placed them within their own method which I called from within the seed method.

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
        
I removed the previous code creating the admin role outside of the seed method into its own method then called it from within the seed method.

    public void CreateAdmin(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)); 
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
        }

### Front-End Stories

For this story the card borders and shading were too thick. I went into the style sheet and decreased the border and shading thickness to make the cards look more graceful. 

    .bulletin-container {
        background-color: #1a2b51;
        border: 2px solid #ff9c00;
        box-shadow: 3px 3px 2px #222;
        color: #ff9c00;
        padding: 50px 50px 35px;
        margin: 20px;
    }
    
The Admin dropdown functionality on the navbar had been created but not styled. In the stylesheet, I adjusted it's placement to line it up with the other navbar buttons and set the static and hover colors to match the rest of the navbar.



    .JPnavbarInverse .JPnavbarStyling .btn:hover {
        color: #fff;
    }

    .JPnavbarInverse .JPnavbarStyling .btn {
        margin-top: 8px;
        background-color: #142953;
    }

    .JPnavbarInverse .JPnavbarStyling .dropdown-menu {
        background-color: #142953;
    }

    .JPnavbarInverse .JPnavbarStyling .dropdown-menu > li > a {
        color: #ff9600;
        background-color: #142953;
    }

    .JPnavbarInverse .JPnavbarStyling .dropdown-menu > li > a:hover {
        color: #fff;
        background-color: #142953;
    }
    
    

###
