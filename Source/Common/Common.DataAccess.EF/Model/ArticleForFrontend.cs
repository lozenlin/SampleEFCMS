using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleForFrontend
    {
        public Nullable<System.Guid> ParentId { get; set; }
        public Nullable<int> ArticleLevelNo { get; set; }
        public string ArticleAlias { get; set; }
        public string BannerPicFileName { get; set; }
        public Nullable<int> LayoutModeId { get; set; }
        public Nullable<int> ShowTypeId { get; set; }
        public string LinkUrl { get; set; }
        public string LinkTarget { get; set; }
        public string ControlName { get; set; }
        public bool IsHideSelf { get; set; }
        public bool IsHideChild { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> SortNo { get; set; }
        public string PostAccount { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string MdfAccount { get; set; }
        public Nullable<System.DateTime> MdfDate { get; set; }
        public bool SubjectAtBannerArea { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        public bool IsShowInUnitArea { get; set; }
        public bool IsShowInSitemap { get; set; }
        public string SortFieldOfFrontStage { get; set; }
        public bool IsSortDescOfFrontStage { get; set; }
        public bool IsListAreaShowInFrontStage { get; set; }
        public bool IsAttAreaShowInFrontStage { get; set; }
        public bool IsPicAreaShowInFrontStage { get; set; }
        public bool IsVideoAreaShowInFrontStage { get; set; }

        public string ArticleSubject { get; set; }
        public string ArticleContext { get; set; }
        public int ReadCount { get; set; }
        public bool IsShowInLang { get; set; }
        public string Subtitle { get; set; }
        public string PublisherName { get; set; }
    }
}
