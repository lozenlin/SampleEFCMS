using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleForBEList
    {
        public System.Guid ArticleId { get; set; }
        public string ArticleSubject { get; set; }
        public int ReadCount { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }

        public bool IsHideSelf { get; set; }
        public bool IsHideChild { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> SortNo { get; set; }
        public bool DontDelete { get; set; }

        public bool IsShowInLangZhTw { get; set; }
        public bool IsShowInLangEn { get; set; }
        public int PostDeptId { get; set; }
        public string PostDeptName { get; set; }
        public int RowNum { get; set; }
    }
}
