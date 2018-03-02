using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleVideoForBEList
    {
        public System.Guid VidId { get; set; }
        public string VidSubject { get; set; }
        public string VidDesc { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }

        public Nullable<int> SortNo { get; set; }
        public string VidLinkUrl { get; set; }
        public string SourceVideoId { get; set; }

        public bool IsShowInLangZhTw { get; set; }
        public bool IsShowInLangEn { get; set; }
        public int PostDeptId { get; set; }
        public string PostDeptName { get; set; }
        public int RowNum { get; set; }
    }
}
