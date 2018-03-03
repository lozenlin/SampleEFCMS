using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class SearchDataSourceForFrontend
    {
        public System.Guid ArticleId { get; set; }
        public System.Guid SubId { get; set; }
        public string CultureName { get; set; }
        public string ArticleSubject { get; set; }
        public string ArticleContext { get; set; }
        public int ReadCount { get; set; }
        public string LinkUrl { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        public string BreadcrumbData { get; set; }
        public Nullable<System.Guid> Lv1ArticleId { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }

        public int MatchesTotal { get; set; }
        public int RowNum { get; set; }
    }
}
