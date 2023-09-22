using System;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using LINQ;
using System.ComponentModel.DataAnnotations;

namespace LINQ;

public class Program
{   
    // membuat string untuk configurasi koneksi ke database
    static string connectionString = "Data Source=IQSALGAFHARI;Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";
    static SqlConnection? connection;
    private static void Main()
    {
        var choice = true;
        while (choice)
        {
            Console.WriteLine("1. List all regions");
            Console.WriteLine("2. List all countries");
            Console.WriteLine("3. List all locations");
            Console.WriteLine("4. List regions with Where");
            Console.WriteLine("5. Join tables regions and countries and locations");
            Console.WriteLine("6. Data Employee");
            Console.WriteLine("7. Department info");
            Console.WriteLine("10. Exit");
            Console.Write("Enter your choice: ");
            var input = Console.ReadLine();
            choice = Menu(input);
        }
    }

    public static bool Menu(string input)
    {
        switch (input)
        {
            case "1":
                var region = new Region();
                var regions = region.GetAll();
                GeneralMenu.List(regions, "regions");
                break;
            case "2":
                var country = new Country();
                var countries = country.GetAll();
                GeneralMenu.List(countries, "countries");
                break;
            case "3":
                var location = new Location();
                var locations = location.GetAll();
                GeneralMenu.List(locations, "locations");
                break;
            case "4":
                var region2 = new Region();
                string input2 = Console.ReadLine();
                var result = region2.GetAll().Where(r => r.Name.Contains(input2)).ToList();
                GeneralMenu.List(result, "regions");
                break;
            case "5":
                var country3 = new Country();
                var region3 = new Region();
                var location3 = new Location();

                var getCountry = country3.GetAll();
                var getRegion = region3.GetAll();
                var getLocation = location3.GetAll();

                var resultJoin = (from r in getRegion
                                  join c in getCountry on r.Id equals c.RegionId
                                  join l in getLocation on c.Id equals l.CountryId
                                  select new RegionAndCountryVM
                                  {
                                      CountryId = c.Id,
                                      CountryName = c.Name,
                                      RegionId = r.Id,
                                      RegionName = r.Name,
                                      City = l.City
                                  }).ToList();

                /*var resultJoin2 = getRegion.Join(getCountry,
                                                 r => r.Id,
                                                 c => c.RegionId,
                                                 (r, c) => new { r, c })
                                           .Join(getLocation,
                                                 rc => rc.c.Id,
                                                 l => l.CountryId,
                                                 (rc, l) => new RegionAndCountryVM
                                                 {
                                                     CountryId = rc.c.Id,
                                                     CountryName = rc.c.Name,
                                                     RegionId = rc.r.Id,
                                                     RegionName = rc.r.Name,
                                                     City = l.City
                                                 }).ToList();

                /*foreach (var item in resultJoin2)
                {
                    Console.WriteLine($"{item.Id} - {item.NameRegion} - {item.NameCountry} - {item.RegionId}");
                }*/ 

                GeneralMenu.List(resultJoin, "regions and countries");
                break;
            case "6":
                var getEmployee6 = new Employee().GetAll();
                var getDepartment6 = new Department().GetAll();
                var getLocation6 = new Location().GetAll();
                var getCountry6 = new Country().GetAll();
                var getRegion6 = new Region().GetAll();             
                var dataEmployee = (from e in getEmployee6
                                    join d in getDepartment6 on e.DepartmentId equals d.Id
                                    join l in getLocation6 on d.LocationId equals l.Id
                                    join c in getCountry6 on  l.CountryId equals c.Id
                                    join r in getRegion6 on c.RegionId equals r.Id                                    
                                    select new DataEmployeeVM
                                    {
                                        Id = e.Id,
                                        FullName = e.FirstName + " " + e.LastName,
                                        Email = e.Email,
                                        Phone = e.PhoneNumber,
                                        DepartmentName = d.Name,
                                        StreetAddress = l.StreetAddress,
                                        CountryName = c.Name,
                                        RegionName = r.Name
                                    }).ToList();                
                GeneralMenu.List(dataEmployee, "Data Employee");
                break;
            case "7":                
                var getEmployee7 = new Employee().GetAll();
                var getDepartment7 = new Department().GetAll();                
                var departmentInfo = (from e in getEmployee7
                                      join d in getDepartment7 on e.DepartmentId equals d.Id
                                      group e by new { d.Name, e.DepartmentId } into groupED                                      
                                      select new DataByDepartmentVM
                                      {
                                          DepartmentName = groupED.Key.Name,
                                          TotalEmployee = groupED.Count(),                                         
                                          MinSalary = groupED.Min(e => e.Salary),
                                          MaxSalary = groupED.Max(e => e.Salary)
                                      }).ToList();                
                GeneralMenu.List(departmentInfo, "Department Info");
                break;
            case "10":
                return false;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }

        return true;
    }


}

