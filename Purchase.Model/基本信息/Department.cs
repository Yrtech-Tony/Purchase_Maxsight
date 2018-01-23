using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    public class Department
    {
        public Department() { }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string InUserId { get; set; }
        public DateTime InDateTime { get; set; }
    }
}
