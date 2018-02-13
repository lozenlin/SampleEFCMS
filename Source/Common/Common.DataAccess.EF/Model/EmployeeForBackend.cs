using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class EmployeeForBackend
    {
        public int EmpId { get; set; }
        public string EmpAccount { get; set; }
        public string EmpPassword { get; set; }
        public string EmpName { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public bool IsAccessDenied { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string OwnerAccount { get; set; }
        public Nullable<System.DateTime> ThisLoginTime { get; set; }
        public string ThisLoginIP { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string LastLoginIP { get; set; }
        public bool PasswordHashed { get; set; }
        public string DefaultRandomPassword { get; set; }

        public int DeptId { get; set; }
        public string DeptName { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDisplayName { get; set; }

        public int OwnerDeptId { get; set; }
        public string RoleDisplayText { get; set; }

        public EmployeeForBackend(Employee emp)
        {
            EmpId = emp.EmpId;
            EmpAccount = emp.EmpAccount;
            EmpPassword = emp.EmpPassword;
            EmpName = emp.EmpName;
            Email = emp.Email;
            Remarks = emp.Remarks;
            IsAccessDenied = emp.IsAccessDenied;
            PostAccount = emp.PostAccount;
            PostDate = emp.PostDate;
            MdfAccount = emp.MdfAccount;
            MdfDate = emp.MdfDate;
            StartDate = emp.StartDate;
            EndDate = emp.EndDate;
            OwnerAccount = emp.OwnerAccount;
            ThisLoginTime = emp.ThisLoginTime;
            ThisLoginIP = emp.ThisLoginIP;
            LastLoginTime = emp.LastLoginTime;
            LastLoginIP = emp.LastLoginIP;
            PasswordHashed = emp.PasswordHashed;
            DefaultRandomPassword = emp.DefaultRandomPassword;

            if(emp.Department != null)
            {
                Department dept = emp.Department;

                DeptId = dept.DeptId;
                DeptName = dept.DeptName;
            }

            if(emp.EmployeeRole != null)
            {
                EmployeeRole role = emp.EmployeeRole;

                RoleId = role.RoleId;
                RoleName = role.RoleName;
                RoleDisplayName = role.RoleDisplayName;
                RoleDisplayText = string.Format("{0} ({1})", RoleDisplayName, RoleName);
            }
    }
}
}
