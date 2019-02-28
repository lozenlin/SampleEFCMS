using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    public class ArticleMultiLangParams
    {
        public Guid ArticleId;
        public string CultureName;
        public string ArticleSubject;
        public string ArticleContext;
        public bool IsShowInLang;
        public string PostAccount;
        public string Subtitle;
        public string PublisherName;
        public string TextContext;
    }
}
