using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    public class ArticleValidListQueryParams
    {
        public Guid ParentId;
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;

        public ArticleValidListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
        }
    }
}
