using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleDescendant
    {
        public System.Guid ArticleId { get; set; }
        public Nullable<int> ArticleLevelNo { get; set; }
    }
}
