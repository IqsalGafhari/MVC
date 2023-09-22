using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    public class DataByDepartmentVM
    {
        public string DepartmentName { get; set; }
        public int TotalEmployee { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }

        public override string ToString()
        {
            return $"{DepartmentName} - {TotalEmployee} - {MinSalary} - {MaxSalary} - ";
        }
    }
}
