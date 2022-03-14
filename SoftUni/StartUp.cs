using SoftUni.Data;
using SoftUni.Models;

using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new SoftUniContext();

            string result = RemoveTown(db);

            Console.WriteLine(result);
        }

        // Problem 03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.MiddleName + " " + e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FullName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString();
        }

        // Problem 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString();
        }

        // Problem 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.DepartmentId == 6)
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DepartmentName = e.Department.Name,
                    Salary = e.Salary
                })
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }

            return sb.ToString();
        }

        // Problem 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee employeeNakov = context
                .Employees
                .First(e => e.LastName == "Nakov");

            employeeNakov.Address = newAddress;

            context.SaveChanges();

            var addresses = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine(address);
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Select(e => new
                {
                    EmployeeFullName = e.FirstName + " " + e.LastName,
                    ManagerFullName = e.Manager.FirstName + " " + e.Manager.LastName,
                    Projects = e.Projects
                    .Where(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003)
                    .Take(10)
                    .Select(p => new
                    {
                        ProjectName = p.Name,
                        StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = p.EndDate.HasValue
                            ? p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                            : "not finished",
                    })
                    .ToList()
                })
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmployeeFullName} - Manager: {e.ManagerFullName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context
                .Addresses
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count()
                })
                .OrderByDescending(a => a.EmployeeCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var emp = context
                .Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.Projects
                    .Select(p => new
                    {
                        ProjectName = p.Name
                    })
                    .OrderBy(p => p.ProjectName)
                    .ToList()
                })
                .ToList();

            foreach (var e in emp)
            {
                sb.AppendLine($"{e.FullName} - {e.JobTitle}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"{p.ProjectName}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context
                .Departments
                .Where(d => d.Employees.Count() > 5)
                .OrderBy(d => d.Employees.Count())
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFullName = d.Manager.FirstName + " " + d.Manager.LastName,
                    DepartmentEmployees = d.Employees
                        .Select(e => new
                        {
                            EmployeeFirstName = e.FirstName,
                            EmployeeLastName = e.LastName,
                            JobTitle = e.JobTitle
                        })
                        .OrderBy(e => e.EmployeeFirstName)
                        .ThenBy(e => e.EmployeeLastName)
                        .ToList()
                })
                .ToList();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManagerFullName}");

                foreach (var e in d.DepartmentEmployees)
                {
                    sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.JobTitle}");
                }
                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context
                .Projects
                .AsEnumerable()
                .Select(p => new
                {
                    ProjectName = p.Name,
                    ProjectDescription = p.Description,
                    ProjectStartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt")
                })
                .OrderByDescending(p => p.ProjectStartDate)
                .Take(10)
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.ProjectName}")
                  .AppendLine($"{p.ProjectDescription}")
                  .AppendLine($"{p.ProjectStartDate}");
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            IQueryable<Employee> employeesToUpdate = context
                .Employees
                .Where(e => e.Department.Name == "Engineering"
                         || e.Department.Name == "Tool Design"
                         || e.Department.Name == "Marketing"
                         || e.Department.Name == "Information Services");

            foreach (var e in employeesToUpdate)
            {
                e.Salary *= 1.12m;
            }

            context.SaveChanges();

            var employees = employeesToUpdate
            .Select(e => new
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Salary = e.Salary
            })
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 13
        public static string GetEmployeesByFirstNameStartingWith(SoftUniContext context, string substring)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.FirstName.ToLower().StartsWith(substring.ToLower()))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary})");
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToDelete = context
                .Projects
                .Where(p => p.ProjectId == 2);

            context.Projects.RemoveRange(projectToDelete);
            context.SaveChanges();

            var projects = context
                .Projects
                .Select(p => p.Name)
                .ToList();

            return String.Join(Environment.NewLine, projects);
        }

        // Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context
                .Towns
                .First(t => t.Name == "Seattle");

            IQueryable<Address> addressesToDelete = context
                .Addresses
                .Where(a => a.TownId == townToDelete.TownId);

            int addressesCount = addressesToDelete.Count();

            IQueryable<Employee> employeesToUpdate = context
                .Employees
                .Where(e => addressesToDelete.Any(a => a.AddressId == e.AddressId));

            foreach (Employee e in employeesToUpdate)
            {
                e.AddressId = null;
            }

            foreach (Address a in addressesToDelete)
            {
                context.Addresses.Remove(a);
            }

            context.Towns.Remove(townToDelete);
            context.SaveChanges();

            return $"{addressesCount} addresses in {townToDelete.Name} were deleted";
        }
    }
}