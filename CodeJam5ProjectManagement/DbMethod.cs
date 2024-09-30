//Great job! I added a check so it will check for an employees existence or not. I really enjoyed the project. 

using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeJam5ProjectManagement
{
    
    class DbMethod
    {

        public static void AddStory()
        {
            using (var context = new ProjectDbContext())
            {
                Console.WriteLine("Enter StatusID:");
                var statusId = Console.ReadLine() ?? "";

                Console.WriteLine("Enter Employee First Name:");
                var employeeFirstName = Console.ReadLine() ?? "";

                Console.WriteLine("Enter Emlpoyee Last Name");
                var employeeLastName = Console.ReadLine() ?? "";

                Console.WriteLine("Enter Storyname:");
                var storyName = Console.ReadLine() ?? "";
                //statusId, employeeID, storyName

                Employee e = context.Employees.FirstOrDefault(
                    a =>
                    a.FirstName == employeeFirstName &&
                    a.LastName == employeeLastName
                );

                // New check for employee existence
                if (e == null)
                {
                    Console.WriteLine("Employee not found. Please check the names and try again.");
                    return; // Exit the method if employee is not found
                }

                Story newStory = new Story
                {
                    StoryName = storyName,
                    StatusId = int.Parse(statusId),
                    EmployeeId = e.EmployeeId
                };

                context.Stories.Add(newStory);
                context.SaveChanges();

            }
        }

        public static void ViewAllStories()
        {
            using (var context = new ProjectDbContext())
            {

                var allStories = context.Stories
                .Include(s => s.status)
                .Include(e => e.employee)
                .ToList();
                foreach (Story s in allStories) {   
                    Console.WriteLine($"{s.StoryName} - {s.employee?.FirstName}");
                }

                var groupedStories = allStories
                .GroupBy(s => s.status.StatusName)
                .ToDictionary(g => g.Key, g => g.ToList());

                var table = new Table();
                table.AddColumn("Backlog");
                table.AddColumn("In Progresss");
                table.AddColumn("Ready For Testing");
                table.AddColumn("Completed");

                int maxStoriesInAnyColumn = groupedStories.Values.Max(g => g.Count);
                
                for(int i = 0; i < maxStoriesInAnyColumn; i++) {
                     var backlogStory = groupedStories.ContainsKey("Backlog") && i < groupedStories["Backlog"].Count
                        ? $"{groupedStories["Backlog"][i].StoryName} - {groupedStories["Backlog"][i].employee?.FirstName}"
                        : "";
                
                    var inProgressStory = groupedStories.ContainsKey("In Progress") && i < groupedStories["In Progress"].Count
                        ? $"{groupedStories["In Progress"][i].StoryName} - {groupedStories["In Progress"][i].employee?.FirstName}"
                        : "";
                        
                    var readyForTestingStory = groupedStories.ContainsKey("Ready For Testing") && i < groupedStories["Ready For Testing"].Count
                        ? $"{groupedStories["Ready For Testing"][i].StoryName} - {groupedStories["Ready For Testing"][i].employee?.FirstName}"
                        : "";
                        
                    var completedStory = groupedStories.ContainsKey("Completed") && i < groupedStories["Completed"].Count
                        ? $"{groupedStories["Completed"][i].StoryName} - {groupedStories["Completed"][i].employee?.FirstName}"
                        : "";

            // Add the row with stories to the table
                    table.AddRow(backlogStory, inProgressStory, readyForTestingStory, completedStory);
                }

                AnsiConsole.Write(table);
            }
        }

        public static void DeleteStory()
        {
            using (var context = new ProjectDbContext())
            {
                Console.WriteLine("Enter the Story you want to delete");
                string storyName= Console.ReadLine()?.Trim();

                try
                {
                    if (string.IsNullOrEmpty(storyName))
                    {
                        Console.WriteLine("Story name cannot be empty, Please enter a vaild story name");

                        return;
                    }
  

                }
                catch {}

                

                var existingStory= context.Stories.FirstOrDefault(s=> s.StoryName == storyName);

                        if (existingStory != null)
                        {
                            context.Stories.Remove(existingStory);
                            context.SaveChanges();
                            Console.WriteLine("Story deleted successfully");
                        }
                        else
                        {
                            Console.Write("Story not found");
                        }

                
                
                
                
                //take in name of story to delete
                
            }
            
        }

    
        public static void MoveStory()
        {
            using (var context = new ProjectDbContext())
            {
                // change statusId
                Console.WriteLine("Enter story you want to move");
                string storyName = Console.ReadLine() ?? "";
                
                Console.WriteLine("Enter new status");
                string statusName = Console.ReadLine().ToLower() ?? "";

                Story story = context.Stories.FirstOrDefault(a => a.StoryName == storyName);
                
                int statusId = story.StatusId;
                bool ret = false;

                switch (statusName)
                {
                    case "backLog":
                        statusId = 1;
                        break;
                    case "in progress":
                        statusId = 2;
                        break;
                    case "ready for testing":
                        statusId = 3;
                        break;
                    case "completed":
                        statusId = 4;
                        break;
                    default:
                        Console.WriteLine("Status does not exist, current status will remain the same \n");
                        ret = true;
                        break;
                }

                if (ret)
                {
                    return;
                }

                if (story != null) {
                    story.StatusId = statusId;
                    context.SaveChanges();

                    Console.WriteLine($"Successfully moved story to: {statusName}\n");
                }

                
            }

        }

        public static void ViewSpecificStory()
        {
            using (var context = new ProjectDbContext())
            {
                 //view the details of a specific story
                Console.WriteLine("Select a story to view its details");
                string storyName = Console.ReadLine();

                var storyDetails = context.Stories.Include(s => s.status).Include(s => s.employee).FirstOrDefault(s => s.StoryName == storyName);
                    

                if(storyDetails != null)
                {
                    Console.WriteLine($"Story Name: {storyDetails.StoryName}");
                    Console.WriteLine($"Story Status: {storyDetails.status.StatusName}");
                    Console.WriteLine($"Employee Name: {storyDetails.employee?.FirstName} {storyDetails.employee?.LastName}");
                    
                }
                else
                {
                    Console.WriteLine("Story Not Found");
                }
            }
        }

        public static void ViewEmployeeStories()
        {
            using (var context = new ProjectDbContext())
            {
                //view all stories associated with a specific employee
                Console.WriteLine("Enter employee first name");
                string employeeFirstName = Console.ReadLine() ?? "";
                
                Console.WriteLine("Enter employee last name");
                string employeeLastName = Console.ReadLine() ?? "";

                Employee employee = context.Employees
                .Include(employee => employee.stories)
                .ThenInclude(s => s.status)
                .FirstOrDefault
                (
                    e =>
                    e.FirstName == employeeFirstName &&
                    e.LastName == employeeLastName
                );

                if (employee != null) {
                    foreach(Story s in employee.stories) {
                        Console.WriteLine($"{s.StoryName}\n{s.status.StatusName} \n");
                    }
                }

                

            }
        }

        public static void AssignEmployee() {
            using (var context = new ProjectDbContext()) {
                Console.WriteLine("Enter story you want to assign");
                string storyName = Console.ReadLine() ?? "";

                Console.WriteLine("Enter Employee you want to assign story to");
                Console.WriteLine("First name:");
                string employeeFirstName = Console.ReadLine() ?? "";

                Console.WriteLine("Last name:");
                string employeeLastName = Console.ReadLine() ?? "";

                var story = context.Stories
                .Include(e => e.employee)
                .FirstOrDefault(s => s.StoryName == storyName);

                var employee = context.Employees.FirstOrDefault(
                    e =>
                    e.FirstName == employeeFirstName &&
                    e.LastName == employeeLastName
                );

                story.employee = employee;
                context.SaveChanges();


            }
        }


    }
}
        
