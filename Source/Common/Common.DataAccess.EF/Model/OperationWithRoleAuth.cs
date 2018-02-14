using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class OperationWithRoleAuth
    {
        public int OpId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string OpSubject { get; set; }
        public string LinkUrl { get; set; }
        public bool IsNewWindow { get; set; }
        public string IconImageFile { get; set; }
        public string EnglishSubject { get; set; }

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
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }

        public List<OperationWithRoleAuth> SubItems { get; set; }

        public OperationWithRoleAuth()
        {
            SubItems = new List<OperationWithRoleAuth>();
        }
    }
}
