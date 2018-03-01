using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class ArticlePictureListQueryParams
    {
        public Guid ArticleId;
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public ArticlePictureListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }

        public ArticlePictureListQueryParamsDA GenArticlePictureListQueryParamsDA()
        {
            ArticlePictureListQueryParamsDA result = new ArticlePictureListQueryParamsDA()
            {
                ArticleId = ArticleId,
                CultureName = CultureName,
                Kw = Kw,
                PagedParams = PagedParams.GenPagedListQueryParamsDA(),
                AuthParams = AuthParams.GenAuthenticationQueryParamsDA()
            };

            return result;
        }
    }
}
