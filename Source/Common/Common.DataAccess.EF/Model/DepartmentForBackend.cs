using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class DepartmentForBackend
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public Nullable<int> SortNo { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }

        public int PostDeptId { get; set; }
        public int EmpTotal { get; set; }
        public int RowNum { get; set; }
    }
}
