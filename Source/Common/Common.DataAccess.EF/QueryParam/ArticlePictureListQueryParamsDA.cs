using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class ArticlePictureListQueryParamsDA
    {
        public Guid ArticleId;
        public string CultureName;
        public string Kw;
        public PagedListQueryParamsDA PagedParams;
        public AuthenticationQueryParamsDA AuthParams;

        public ArticlePictureListQueryParamsDA()
        {
            PagedParams = new PagedListQueryParamsDA();
            AuthParams = new AuthenticationQueryParamsDA();
        }
    }
}
