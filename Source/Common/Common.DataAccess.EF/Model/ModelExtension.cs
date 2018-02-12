using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public partial class Department : IHasIdentityCol<int>
    {
        public int GetIdentityColValue()
        {
            return DeptId;
        }
    }

    public partial class EmployeeRole : IHasIdentityCol<int>
    {
        public int GetIdentityColValue()
        {
            return RoleId;
        }
    }

    public partial class Employee : IHasIdentityCol<int>
    {
        public int GetIdentityColValue()
        {
            return EmpId;
        }
    }

    public partial class Operations : IHasIdentityCol<int>
    {
        public int GetIdentityColValue()
        {
            return OpId;
        }
    }

    public partial class Keyword : IHasIdentityCol<int>
    {
        public int GetIdentityColValue()
        {
            return Seqno;
        }
    }
}
