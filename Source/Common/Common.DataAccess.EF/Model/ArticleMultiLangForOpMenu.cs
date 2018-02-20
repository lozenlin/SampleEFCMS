using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleMultiLangForOpMenu
    {
        public System.Guid ArticleId { get; set; }
        public string ArticleSubject { get; set; }
        public bool IsHideSelf { get; set; }
    }
}
