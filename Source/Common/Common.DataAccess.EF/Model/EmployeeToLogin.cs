using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class EmployeeToLogin
    {
        public string EmpPassword { get; set; }
        public bool IsAccessDenied { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool PasswordHashed { get; set; }
        public string RoleName { get; set; }
    }
}
