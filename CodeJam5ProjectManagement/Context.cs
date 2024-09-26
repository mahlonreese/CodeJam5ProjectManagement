using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeJam5ProjectManagement
{
    class ProjectDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Story> Stories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString:
            "Server=localhost;Port=5432;User Id=postgres;Password=password;Database=kanban;Include Error Detail=true;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, FirstName = "Mahlon", LastName = "Reese"},
                new Employee { EmployeeId = 2, FirstName = "Jonathan", LastName = "Lun"},
                new Employee { EmployeeId = 3, FirstName = "Cyber", LastName = "Justin"}

            );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = 1, StatusName = "Backlog"},
                new Status { StatusId = 2, StatusName = "In Progress"},
                new Status { StatusId = 3, StatusName = "Ready For Testing"},
                new Status { StatusId = 4, StatusName = "Completed"}
            );

            modelBuilder.Entity<Story>().HasData(
                new Story { StoryId = 1, StatusId = 1, EmployeeId = null, StoryName = "Fix Bug"},
                new Story { StoryId = 2, StatusId = 2, EmployeeId = 3, StoryName = "Hack Mainframe"}
            );
        }

    }
    
}
