using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeJam5ProjectManagement
{
    [Table("employees")]
    public class Employee
    {
        [Column("employee_id")] 
        public int EmployeeId { get; set; }
        
        [Column("first_name", TypeName = "varchar(120)")]
        public string FirstName { get; set; }

        [Column("last_name", TypeName = "varchar(120)")]
        public string LastName { get; set; }

        public List<Story> stories { get; set; } = [];
    }

    [Table("statuses")]
    public class Status
    {
        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("status_name", TypeName = "varchar(100)")]
        public string StatusName {get; set; }
        
        public List<Story> stories { get; set; } = [];
    }

    [Table("stories")]
    public class Story
    {
        [Column("story_id")]
        public int StoryId { get; set; }

        [Column("story_name", TypeName = "varchar(50)")]
        public string StoryName { get; set; }

        [ForeignKey("StatusId")]
        [Column("status_id")]
        public int StatusId { get; set; }
        public Status status { get; set; }

        [ForeignKey("EmployeeId")]
        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        public Employee employee { get; set; }
    }

}
