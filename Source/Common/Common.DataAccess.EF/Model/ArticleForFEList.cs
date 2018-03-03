using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleForFEList
    {
        public System.Guid ArticleId { get; set; }
        public string ArticleSubject { get; set; }
        public string PublisherName { get; set; }
        public string TextContext { get; set; }

        public string ArticleAlias { get; set; }
        public Nullable<int> ShowTypeId { get; set; }
        public string LinkUrl { get; set; }
        public string LinkTarget { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<int> SortNo { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }

        public int RowNum { get; set; }
    }
}
