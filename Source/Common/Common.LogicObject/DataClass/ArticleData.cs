using Common.DataAccess.EF.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// Article data for front-stage
    /// </summary>
    public class ArticleData
    {
        public Guid? ArticleId;
        public Guid? ParentId;
        public int ArticleLevelNo;
        public string ArticleAlias;
        public string BannerPicFileName;
        public int LayoutModeId;
        public int ShowTypeId;
        public string LinkUrl;
        public string LinkTarget;
        public string ControlName;
        public bool IsHideSelf;
        public bool IsHideChild;
        public DateTime StartDate;
        public DateTime EndDate;
        public int SortNo;
        public bool SubjectAtBannerArea;
        public DateTime PublishDate;
        public bool IsShowInUnitArea;
        public bool IsShowInSitemap;
        public string SortFieldOfFrontStage;
        public bool IsSortDescOfFrontStage;
        public bool IsListAreaShowInFrontStage;
        public bool IsAttAreaShowInFrontStage;
        public bool IsPicAreaShowInFrontStage;
        public bool IsVideoAreaShowInFrontStage;
        public string ArticleSubject;
        public string ArticleContext;
        public bool IsShowInLang;
        public string Subtitle;
        public string PublisherName;
        public string PostAccount;
        public DateTime PostDate;
        public string MdfAccount;
        public DateTime MdfDate;
        public Guid? Lv1Id;
        public Guid? Lv2Id;
        public Guid? Lv3Id;
        public bool IsPreviewMode = false;

        public void ImportDataFrom(ArticleForFrontend artFE)
        {
            ParentId = artFE.ParentId;
            ArticleLevelNo = artFE.ArticleLevelNo.Value;
            ArticleAlias = artFE.ArticleAlias;
            BannerPicFileName = artFE.BannerPicFileName ?? "";
            LayoutModeId = artFE.LayoutModeId.Value;
            ShowTypeId = artFE.ShowTypeId.Value;
            LinkUrl = artFE.LinkUrl;
            LinkTarget = artFE.LinkTarget;
            ControlName = artFE.ControlName;
            IsHideSelf = artFE.IsHideSelf;
            IsHideChild = artFE.IsHideChild;
            StartDate = artFE.StartDate.Value;
            EndDate = artFE.EndDate.Value;
            SortNo = artFE.SortNo.Value;
            SubjectAtBannerArea = artFE.SubjectAtBannerArea;
            PublishDate = artFE.PublishDate.Value;
            IsShowInUnitArea = artFE.IsShowInUnitArea;
            IsShowInSitemap = artFE.IsShowInSitemap;
            SortFieldOfFrontStage = artFE.SortFieldOfFrontStage;
            IsSortDescOfFrontStage = artFE.IsSortDescOfFrontStage;
            IsListAreaShowInFrontStage = artFE.IsListAreaShowInFrontStage;
            IsAttAreaShowInFrontStage = artFE.IsAttAreaShowInFrontStage;
            IsPicAreaShowInFrontStage = artFE.IsPicAreaShowInFrontStage;
            IsVideoAreaShowInFrontStage = artFE.IsVideoAreaShowInFrontStage;
            ArticleSubject = artFE.ArticleSubject;
            ArticleContext = artFE.ArticleContext;
            IsShowInLang = artFE.IsShowInLang;
            Subtitle = artFE.Subtitle;
            PublisherName = artFE.PublisherName;
            PostAccount = artFE.PostAccount;
            PostDate = artFE.PostDate.Value;

            if (!artFE.MdfDate.HasValue)
            {
                MdfAccount = PostAccount;
                MdfDate = PostDate;
            }
            else
            {
                MdfAccount = artFE.MdfAccount;
                MdfDate = artFE.MdfDate.Value;
            }
        }
    }
}
