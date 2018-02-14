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

        public void ImportDataFrom(Operations op)
        {
            OpId = op.OpId;
            ParentId = op.ParentId;
            OpSubject = op.OpSubject;
            LinkUrl = op.LinkUrl;
            IsNewWindow = op.IsNewWindow;
            IconImageFile = op.IconImageFile;
            EnglishSubject = op.EnglishSubject;
        }

        public void ImportDataFrom(EmployeeRoleOperationsDesc roleAuthItem)
        {
            CanRead = roleAuthItem.CanRead;
            CanEdit = roleAuthItem.CanEdit;
            CanReadSubItemOfSelf = roleAuthItem.CanReadSubItemOfSelf;
            CanEditSubItemOfSelf = roleAuthItem.CanEditSubItemOfSelf;
            CanAddSubItemOfSelf = roleAuthItem.CanAddSubItemOfSelf;
            CanDelSubItemOfSelf = roleAuthItem.CanDelSubItemOfSelf;
            CanReadSubItemOfCrew = roleAuthItem.CanReadSubItemOfCrew;
            CanEditSubItemOfCrew = roleAuthItem.CanEditSubItemOfCrew;
            CanDelSubItemOfCrew = roleAuthItem.CanDelSubItemOfCrew;
            CanReadSubItemOfOthers = roleAuthItem.CanReadSubItemOfOthers;
            CanEditSubItemOfOthers = roleAuthItem.CanEditSubItemOfOthers;
            CanDelSubItemOfOthers = roleAuthItem.CanDelSubItemOfOthers;
            PostAccount = roleAuthItem.PostAccount;
            PostDate = roleAuthItem.PostDate;
            MdfAccount = roleAuthItem.MdfAccount;
            MdfDate = roleAuthItem.MdfDate;
        }
    }
}
