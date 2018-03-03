using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
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

        public ArticleValidListQueryParamsDA GenArticleValidListQueryParamsDA()
        {
            ArticleValidListQueryParamsDA result = new ArticleValidListQueryParamsDA()
            {
                ParentId = ParentId,
                CultureName = CultureName,
                Kw = Kw,
                PagedParams = PagedParams.GenPagedListQueryParamsDA()
            };

            return result;
        }
    }
}
