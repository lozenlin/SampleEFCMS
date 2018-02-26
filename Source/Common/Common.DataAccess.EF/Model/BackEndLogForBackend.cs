using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class BackEndLogForBackend
    {
        public int Seqno { get; set; }
        public string EmpAccount { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> OpDate { get; set; }
        public string IP { get; set; }

        public string EmpName { get; set; }
        public Nullable<int> DeptId { get; set; }
        public int RowNum { get; set; }
    }
}
