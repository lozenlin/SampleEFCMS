// ===============================================================================
// ArticlePublisherDataAccess of SampleEFCMS
// https://github.com/lozenlin/SampleEFCMS
//
// ArticlePublisherDataAccess.cs
//
// ===============================================================================
// Copyright (c) 2018 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using Common.DataAccess.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class ArticlePublisherDataAccess : DataAccessBase
    {
        public ArticlePublisherDataAccess() : base()
        {
        }

        #region 網頁內容

        /// <summary>
        /// 取得作業選單用指定語系的網頁內容清單
        /// </summary>
        public List<ArticleMultiLangForOpMenu> GetArticleMultiLangListForOpMenu(Guid parentId, string cultureName)
        {
            Logger.Debug("GetArticleMultiLangListForOpMenu(parentId)");

            List<ArticleMultiLangForOpMenu> entities = null;

            try
            {
                entities = (from am in cmsCtx.ArticleMultiLang
                            from a in cmsCtx.Article
                            where am.ArticleId == a.ArticleId
                             && am.CultureName == cultureName
                             && a.ParentId == parentId
                            select new ArticleMultiLangForOpMenu()
                            {
                                ArticleId = am.ArticleId,
                                ArticleSubject = am.ArticleSubject,
                                IsHideSelf = a.IsHideSelf
                            }).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entities;
        }

        /// <summary>
        /// 取得後台用網頁內容資料
        /// </summary>
        public ArticleForBackend GetArticleDataForBackend(Guid articleId)
        {
            Logger.Debug("GetArticleDataForBackend(articleId)");
            ArticleForBackend entity = null;

            try
            {
                entity = (from a in cmsCtx.Article
                          from e in cmsCtx.Employee
                          where a.PostAccount == e.PostAccount
                             && a.ArticleId == articleId
                          select new ArticleForBackend()
                          {
                              ArticleId = a.ArticleId,
                              ParentId = a.ParentId,
                              ArticleLevelNo = a.ArticleLevelNo,
                              ArticleAlias = a.ArticleAlias,
                              BannerPicFileName = a.BannerPicFileName,
                              LayoutModeId = a.LayoutModeId,
                              ShowTypeId = a.ShowTypeId,
                              LinkUrl = a.LinkUrl,
                              LinkTarget = a.LinkTarget,
                              ControlName = a.ControlName,
                              SubItemControlName = a.SubItemControlName,
                              IsHideSelf = a.IsHideSelf,
                              IsHideChild = a.IsHideChild,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              SortNo = a.SortNo,
                              DontDelete = a.DontDelete,
                              PostAccount = a.PostAccount,
                              PostDate = a.PostDate,
                              MdfAccount = a.MdfAccount,
                              MdfDate = a.MdfDate,
                              SubjectAtBannerArea = a.SubjectAtBannerArea,
                              PublishDate = a.PublishDate,
                              IsShowInUnitArea = a.IsShowInUnitArea,
                              IsShowInSitemap = a.IsShowInSitemap,
                              SortFieldOfFrontStage = a.SortFieldOfFrontStage,
                              IsSortDescOfFrontStage = a.IsSortDescOfFrontStage,
                              IsListAreaShowInFrontStage = a.IsListAreaShowInFrontStage,
                              IsAttAreaShowInFrontStage = a.IsAttAreaShowInFrontStage,
                              IsPicAreaShowInFrontStage = a.IsPicAreaShowInFrontStage,
                              IsVideoAreaShowInFrontStage = a.IsVideoAreaShowInFrontStage,
                              SubItemLinkUrl = a.SubItemLinkUrl,
                              PostDeptId = e.DeptId ?? 0
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        #endregion
    }
}
