using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.EntityRequiredPropValues
{
    public class EmployeeRequiredPropValues
    {
        public int EmpId { get; set; }
        public string EmpAccount { get; set; }
        public string EmpPassword { get; set; }
        public bool IsAccessDenied { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool PasswordHashed { get; set; }
    }
}
