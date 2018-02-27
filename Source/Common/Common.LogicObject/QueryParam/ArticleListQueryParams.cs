using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class ArticleListQueryParams
    {
        public Guid ParentId;
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public ArticleListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }

        public ArticleListQueryParamsDA GenArticleListQueryParamsDA()
        {
            ArticleListQueryParamsDA result = new ArticleListQueryParamsDA()
            {
                ParentId = ParentId,
                CultureName = CultureName,
                Kw = Kw,
                PagedParams = PagedParams.GenPagedListQueryParamsDA(),
                AuthParams = AuthParams.GenAuthenticationQueryParamsDA()
            };

            return result;
        }
    }
}
