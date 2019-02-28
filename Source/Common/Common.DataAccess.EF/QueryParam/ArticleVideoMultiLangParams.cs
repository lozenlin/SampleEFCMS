using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    public class ArticleVideoMultiLangParams
    {
        public Guid VidId;
        public string CultureName;
        public string VidSubject;
        public string VidDesc;
        public bool IsShowInLang;
        public string PostAccount;
    }
}
