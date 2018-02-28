using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.EntityRequiredPropValues
{
    public class ArticleRequiredPropValues
    {
        public System.Guid ArticleId { get; set; }
        public bool IsHideSelf { get; set; }
        public bool IsHideChild { get; set; }
        public bool DontDelete { get; set; }
        public bool SubjectAtBannerArea { get; set; }
        public bool IsShowInUnitArea { get; set; }
        public bool IsShowInSitemap { get; set; }
        public bool IsSortDescOfFrontStage { get; set; }
        public bool IsListAreaShowInFrontStage { get; set; }
        public bool IsAttAreaShowInFrontStage { get; set; }
        public bool IsPicAreaShowInFrontStage { get; set; }
        public bool IsVideoAreaShowInFrontStage { get; set; }
    }
}
