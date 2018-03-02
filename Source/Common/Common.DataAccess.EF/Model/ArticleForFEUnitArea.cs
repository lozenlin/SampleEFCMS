using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleForFEUnitArea
    {
        public System.Guid ArticleId { get; set; }
        public string ArticleSubject { get; set; }

        public string ArticleAlias { get; set; }
        public Nullable<int> ShowTypeId { get; set; }
        public string LinkUrl { get; set; }
        public string LinkTarget { get; set; }
        public bool IsHideChild { get; set; }
    }
}
