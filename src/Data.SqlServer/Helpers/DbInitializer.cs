using System;
using System.Linq;
using Template.Data.SqlServer.Models;
using Template.Domain.Entities.Corporate;
using Template.Domain.Enums.Corporate;

namespace Template.Data.SqlServer.Helpers
{
    public static class DbInitializer
    {
        public static void Initialize(TemplateDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                // DB has been seeded
                return;
            }

            var users = new User[]
            {
                new User { FirstName = "Carson", LastName = "Alexander", Email = "carson.alexander@ddr.com.br", Phone = "+55 11 91234-5678", Password = "ghjhjttes", ProfileImage = "profiles/img/carson.alexander", Gender = Gender.Male, BirthDate = DateTime.Now.AddYears(-30).AddMonths(-1).AddDays(-12), AccessAttempts = 0, LastAccessDate = DateTime.Now, LastAcceptanceTermsDate = DateTime.Now.AddDays(-45), Blocked = false, AddedDate = DateTime.Now.AddDays(-45), ModifiedDate = DateTime.Now.AddDays(-45), DeletedDate = null, IPAddress = "127.0.0.1", Active = true },
                new User { FirstName = "Meredith", LastName = "Alonso",  Email = "ma@ddr.com.br", Phone = "+55 11 93245-5678", Password = "dfgdfgerter", ProfileImage = "profiles/img/meredith.alonso", Gender = Gender.Female, BirthDate = DateTime.Now.AddYears(-25).AddMonths(-6).AddDays(12), AccessAttempts = 0, LastAccessDate = DateTime.Now, LastAcceptanceTermsDate = DateTime.Now.AddDays(-25), Blocked = false, AddedDate = DateTime.Now.AddDays(-25), ModifiedDate = DateTime.Now.AddDays(-25), DeletedDate = DateTime.Now.AddDays(-45), IPAddress = "192.168.1.2", Active = false },
                new User { FirstName = "Arturo", LastName = "Anand",     Email = "a.anand@ddr.com.br", Phone = "+55 11 94675-5678", Password = "cvbbfger", ProfileImage = "profiles/img/arturo.anand", Gender = Gender.Male, BirthDate = DateTime.Now.AddYears(-23).AddMonths(-2).AddDays(-7), AccessAttempts = 3, LastAccessDate = DateTime.Now.AddDays(-10), LastAcceptanceTermsDate = DateTime.Now.AddDays(-15), Blocked = true, AddedDate = DateTime.Now.AddDays(-15), ModifiedDate = DateTime.Now.AddDays(-15), DeletedDate = null, IPAddress = "192.168.1.2", Active = true },
                new User { FirstName = "Gytis", LastName = "Barzdukas",  Email = "gytis.b@ddr.com.br", Phone = "+55 11 91145-5678", Password = "cvbgeger", ProfileImage = "profiles/img/gytis.barzdukas", Gender = Gender.Female, BirthDate = DateTime.Now.AddYears(-20).AddMonths(-0).AddDays(5), AccessAttempts = 3, LastAccessDate = DateTime.Now.AddDays(-5), LastAcceptanceTermsDate = DateTime.Now.AddDays(-5), Blocked = true, AddedDate = DateTime.Now.AddDays(-5), ModifiedDate = DateTime.Now.AddDays(-5), DeletedDate = null, IPAddress = "192.168.1.123", Active = true },
                new User { FirstName = "Yan", LastName = "Li",           Email = "yan.li@ddr.com.br", Phone = "+55 11 99945-5678", Password = "bvnvrgr", ProfileImage = "profiles/img/yan.li", Gender = Gender.Male, BirthDate = DateTime.Now.AddYears(-20).AddMonths(-3).AddDays(4), AccessAttempts = 3, LastAccessDate = DateTime.Now, LastAcceptanceTermsDate = DateTime.Now.AddDays(-4), Blocked = true, AddedDate = DateTime.Now.AddDays(-4), ModifiedDate = DateTime.Now.AddDays(-4), DeletedDate = DateTime.Now.AddDays(-90), IPAddress = "192.168.1.345", Active = false },
                new User { FirstName = "Peggy", LastName = "Justice",    Email = "pejus@ddr.com.br", Phone = "+55 11 97777-5678", Password = "fghfghrty", ProfileImage = "profiles/img/peggy.justice", Gender = Gender.Female, BirthDate = DateTime.Now.AddYears(-22).AddMonths(-9).AddDays(-25), AccessAttempts = 0, LastAccessDate = DateTime.Now, LastAcceptanceTermsDate = DateTime.Now.AddDays(-46), Blocked = false, AddedDate = DateTime.Now.AddDays(-46), ModifiedDate = DateTime.Now.AddDays(-46), DeletedDate = null, IPAddress = "192.168.1.762", Active = true },
                new User { FirstName = "Laura", LastName = "Norman",     Email = "l.norman@ddr.com.br", Phone = "+55 11 92342-5678", Password = "werr3r34", ProfileImage = "profiles/img/laura.norman", Gender = Gender.Female, BirthDate = DateTime.Now.AddYears(-20).AddMonths(-8).AddDays(-1), AccessAttempts = 0, LastAccessDate = DateTime.Now.AddDays(-1), LastAcceptanceTermsDate = DateTime.Now.AddDays(-49), Blocked = false, AddedDate = DateTime.Now.AddDays(-49), ModifiedDate = DateTime.Now.AddDays(-49), DeletedDate = null, IPAddress = "192.168.1.23423", Active = true },
                new User { FirstName = "Nino", LastName = "Olivetto",    Email = "n.olivetto@ddr.com.br", Phone = "+55 11 97867-5678", Password = "wer4334r", ProfileImage = "profiles/img/nino.olivetto", Gender = Gender.Male, BirthDate = DateTime.Now.AddYears(-24).AddMonths(-7).AddDays(-9), AccessAttempts = 0, LastAccessDate = DateTime.Now, LastAcceptanceTermsDate = DateTime.Now.AddDays(-40), Blocked = false, AddedDate = DateTime.Now.AddDays(-40), ModifiedDate = DateTime.Now.AddDays(-40), DeletedDate = null, IPAddress = "192.168.143.24565", Active = true }
            };
            foreach (User s in users)
            {
                context.Users.Add(s);
            }

            context.SaveChanges();

            ////var courses = new Course[]
            ////{
            ////new Course{CourseID=1050,Title="Chemistry",Credits=3,},
            ////new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
            ////new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
            ////new Course{CourseID=1045,Title="Calculus",Credits=4,},
            ////new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
            ////new Course{CourseID=2021,Title="Composition",Credits=3,},
            ////new Course{CourseID=2042,Title="Literature",Credits=4,}
            ////};
            ////foreach (Course c in courses)
            ////{
            ////    context.Courses.Add(c);
            ////}
            ////context.SaveChanges();

            ////var enrollments = new Enrollment[]
            ////{
            ////new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            ////new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            ////new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            ////new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            ////new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            ////new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            ////new Enrollment{StudentID=3,CourseID=1050},
            ////new Enrollment{StudentID=4,CourseID=1050,},
            ////new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            ////new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            ////new Enrollment{StudentID=6,CourseID=1045},
            ////new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            ////};
            ////foreach (Enrollment e in enrollments)
            ////{
            ////    context.Enrollments.Add(e);
            ////}
            ////context.SaveChanges();
        }
    }
}
