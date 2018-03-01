using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class AttachFileForBEList
    {
        public System.Guid AttId { get; set; }
        public string AttSubject { get; set; }
        public int ReadCount { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }

        public string FilePath { get; set; }
        public string FileSavedName { get; set; }
        public int FileSize { get; set; }
        public Nullable<int> SortNo { get; set; }
        public string FileMIME { get; set; }
        public bool DontDelete { get; set; }

        public bool IsShowInLangZhTw { get; set; }
        public bool IsShowInLangEn { get; set; }
        public int PostDeptId { get; set; }
        public string PostDeptName { get; set; }
        public int RowNum { get; set; }
    }
}
