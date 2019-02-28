using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    public class ArticlePictureMultiLangParams
    {
        public Guid PicId;
        public string CultureName;
        public string PicSubject;
        public bool IsShowInLang;
        public string PostAccount;
    }
}
