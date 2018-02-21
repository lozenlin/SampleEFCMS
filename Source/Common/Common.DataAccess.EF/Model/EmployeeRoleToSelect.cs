using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class EmployeeRoleToSelect
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDisplayName { get; set; }
        public string DisplayText { get; set; }
    }
}
