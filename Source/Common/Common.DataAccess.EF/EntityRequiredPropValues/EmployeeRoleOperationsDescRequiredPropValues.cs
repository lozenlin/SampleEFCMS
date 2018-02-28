using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.EntityRequiredPropValues
{
    public class EmployeeRoleOperationsDescRequiredPropValues
    {
        public string RoleName { get; set; }
        public int OpId { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
        public bool CanReadSubItemOfSelf { get; set; }
        public bool CanEditSubItemOfSelf { get; set; }
        public bool CanAddSubItemOfSelf { get; set; }
        public bool CanDelSubItemOfSelf { get; set; }
        public bool CanReadSubItemOfCrew { get; set; }
        public bool CanEditSubItemOfCrew { get; set; }
        public bool CanDelSubItemOfCrew { get; set; }
        public bool CanReadSubItemOfOthers { get; set; }
        public bool CanEditSubItemOfOthers { get; set; }
        public bool CanDelSubItemOfOthers { get; set; }
    }
}
