using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class EmployeeRoleForBackend
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDisplayName { get; set; }
        public Nullable<int> SortNo { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }

        public Nullable<int> PostDeptId { get; set; }

        public EmployeeRoleForBackend()
        {

        }

        public EmployeeRoleForBackend(EmployeeRole empRole)
        {
            RoleId = empRole.RoleId;
            RoleName = empRole.RoleName;
            RoleDisplayName = empRole.RoleDisplayName;
            SortNo = empRole.SortNo;
            PostAccount = empRole.PostAccount;
            PostDate = empRole.PostDate;
            MdfAccount = empRole.MdfAccount;
            MdfDate = empRole.MdfDate;
        }
    }
}
