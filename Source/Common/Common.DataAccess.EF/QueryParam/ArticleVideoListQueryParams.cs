using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    public class ArticleVideoListQueryParams
    {
        public Guid ArticleId;
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public ArticleVideoListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }
    }
}
