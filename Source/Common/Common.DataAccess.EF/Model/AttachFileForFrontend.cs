using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class AttachFileForFrontend
    {
        public System.Guid AttId { get; set; }
        public string AttSubject { get; set; }
        public int ReadCount { get; set; }

        public string FileSavedName { get; set; }
        public int FileSize { get; set; }
        public Nullable<int> SortNo { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }
    }
}
