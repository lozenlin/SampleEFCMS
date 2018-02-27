using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleMultiLangLevelInfo
    {
        public System.Guid ArticleId { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public string ArticleSubject { get; set; }
        public Nullable<int> ArticleLevelNo { get; set; }
        public Nullable<int> ShowTypeId { get; set; }
        public string LinkUrl { get; set; }
        public string LinkTarget { get; set; }
        public bool IsHideSelf { get; set; }
        public bool IsShowInLang { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
