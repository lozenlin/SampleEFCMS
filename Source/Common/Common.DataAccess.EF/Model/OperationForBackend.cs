using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class OperationForBackend
    {
        public int OpId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string OpSubject { get; set; }
        public string LinkUrl { get; set; }
        public bool IsNewWindow { get; set; }
        public string IconImageFile { get; set; }
        public Nullable<int> SortNo { get; set; }
        public bool IsHideSelf { get; set; }
        public string CommonClass { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }
        public string EnglishSubject { get; set; }

        public string PostName { get; set; }
        public string PostDeptName { get; set; }
    }
}
