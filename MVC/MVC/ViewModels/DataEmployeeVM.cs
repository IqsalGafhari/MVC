using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    public class DataEmployeeVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DepartmentName { get; set; }
        public string StreetAddress { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }


        public override string ToString()
        {
            return $"{Id} - {FullName} - {Email} - {Phone} - {DepartmentName} - {StreetAddress} - {CountryName} - {RegionName}";
        }
    }

    
}
