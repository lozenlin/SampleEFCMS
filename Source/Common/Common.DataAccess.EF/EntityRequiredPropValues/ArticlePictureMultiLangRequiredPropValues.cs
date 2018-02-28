using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.EntityRequiredPropValues
{
    public class ArticlePictureMultiLangRequiredPropValues
    {
        public System.Guid PicId { get; set; }
        public string CultureName { get; set; }
        public bool IsShowInLang { get; set; }
    }
}
