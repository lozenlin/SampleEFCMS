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
using Common.DataAccess.EF.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        /// <summary>
        /// 取得指定語系的網頁內容階層資料
        /// </summary>
        public List<ArticleMultiLangLevelInfo> GetArticleMultiLangLevelInfoList(Guid articleId, string cultureName)
        {
            Logger.Debug("GetArticleMultiLangLevelInfoList(articleId, cultureName)");
            List<ArticleMultiLangLevelInfo> entities = null;
            
            try
            {
                entities = new List<ArticleMultiLangLevelInfo>();
                ArticleMultiLangLevelInfo entity = null;
                Guid curArticleId = articleId;

                do
                {
                    entity = (from am in cmsCtx.ArticleMultiLang
                              from a in cmsCtx.Article
                              where am.ArticleId == a.ArticleId
                               && am.ArticleId == curArticleId
                               && am.CultureName == cultureName
                              select new ArticleMultiLangLevelInfo()
                              {
                                  ArticleId = am.ArticleId,
                                  ParentId = a.ParentId,
                                  ArticleSubject = am.ArticleSubject,
                                  ArticleLevelNo = a.ArticleLevelNo,
                                  ShowTypeId = a.ShowTypeId,
                                  LinkUrl = a.LinkUrl,
                                  LinkTarget = a.LinkTarget,
                                  IsHideSelf = a.IsHideSelf,
                                  IsShowInLang = am.IsShowInLang,
                                  StartDate = a.StartDate,
                                  EndDate = a.EndDate
                              }).FirstOrDefault();

                    if (entity == null)
                    {
                        throw new Exception(string.Format("there is no data of curArticleId[{0}].", curArticleId));
                    }

                    entities.Add(entity);

                    if (entity.ParentId.HasValue)
                        curArticleId = entity.ParentId.Value;

                } while (entity.ParentId.HasValue);

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
        /// 取得後台用指定語系的網頁內容清單
        /// </summary>
        public List<ArticleForBEList> GetArticleMultiLangListForBackend(ArticleListQueryParamsDA param)
        {
            Logger.Debug("GetArticleMultiLangListForBackend(param)");
            List<ArticleForBEList> entities = null;

            try
            {
                var tempQuery = from am in cmsCtx.ArticleMultiLang
                                join a in cmsCtx.Article on am.ArticleId equals a.ArticleId
                                join e in cmsCtx.Employee.Include(emp => emp.Department) on am.PostAccount equals e.EmpAccount
                                into amGroup
                                from e in amGroup.DefaultIfEmpty()
                                where a.ParentId == param.ParentId
                                    && am.CultureName == param.CultureName
                                select new ArticleForBEList()
                                {
                                    ArticleId = am.ArticleId,
                                    ArticleSubject = am.ArticleSubject,
                                    ReadCount = am.ReadCount,
                                    PostAccount = am.PostAccount,
                                    PostDate = am.PostDate,
                                    MdfAccount = am.MdfAccount,
                                    MdfDate = am.MdfDate,

                                    IsHideSelf = a.IsHideSelf,
                                    IsHideChild = a.IsHideChild,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    SortNo = a.SortNo,
                                    DontDelete = a.DontDelete,

                                    IsShowInLangZhTw = fnArticle_IsShowInLang(am.ArticleId, "zh-TW"),
                                    IsShowInLangEn = fnArticle_IsShowInLang(am.ArticleId, "en"),
                                    PostDeptId = e.DeptId ?? 0,
                                    PostDeptName = e.Department.DeptName
                                };

                // Query conditions

                if (!param.AuthParams.CanReadSubItemOfOthers)
                {
                    tempQuery = tempQuery.Where(obj =>
                        param.AuthParams.CanReadSubItemOfCrew && obj.PostDeptId == param.AuthParams.MyDeptId
                        || param.AuthParams.CanReadSubItemOfSelf && obj.PostAccount == param.AuthParams.MyAccount);
                }

                if(param.Kw != "")
                {
                    tempQuery = tempQuery.Where(obj => obj.ArticleSubject.Contains(param.Kw));
                }

                // total
                param.PagedParams.RowCount = tempQuery.Count();

                // sorting
                if (param.PagedParams.SortField != "")
                {
                    tempQuery = tempQuery.OrderBy(param.PagedParams.SortField, param.PagedParams.IsSortDesc);
                }
                else
                {
                    // default
                    tempQuery = tempQuery.OrderBy(obj => obj.SortNo);
                }

                // paging
                int skipCount = param.PagedParams.GetSkipCount();
                int takeCount = param.PagedParams.GetTakeCount();

                if (skipCount > 0)
                {
                    tempQuery = tempQuery.Skip(skipCount);
                }

                if (takeCount >= 0)
                {
                    tempQuery = tempQuery.Take(takeCount);
                }

                // result
                entities = tempQuery.ToList();
                int rowIndex = 0;

                foreach (var entity in entities)
                {
                    entity.RowNum = skipCount + rowIndex + 1;
                    rowIndex++;
                }
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
        /// 取得網頁的所有子網頁資訊
        /// </summary>
        public List<ArticleDescendant> GetArticleDescendants(Guid articleId)
        {
            Logger.Debug("GetArticleDescendants(articleId)");
            List<ArticleDescendant> entities = null;
            List<ArticleDescendant> descendants = null;
            Guid curArticleId = articleId;
            int curLevelNo = 0;

            try
            {
                entities = new List<ArticleDescendant>();

                // get current info
                ArticleDescendant entity = cmsCtx.Article.Where(obj => obj.ArticleId == articleId)
                    .Select(obj => new ArticleDescendant()
                    {
                        ArticleId = obj.ArticleId,
                        ArticleLevelNo = obj.ArticleLevelNo
                    }).FirstOrDefault();

                if (entity != null)
                {
                    curLevelNo = entity.ArticleLevelNo.Value;
                    entities.Add(entity);

                    do
                    {
                        List<Guid?> parentIds = entities.Where(obj => obj.ArticleLevelNo == curLevelNo)
                            .Select(obj => (Guid?)obj.ArticleId).ToList();

                        descendants = (from a in cmsCtx.Article
                                       where parentIds.Contains(a.ParentId)
                                       select new ArticleDescendant()
                                       {
                                           ArticleId = a.ArticleId,
                                           ArticleLevelNo = a.ArticleLevelNo
                                       }).ToList();

                        if (descendants.Count > 0)
                        {
                            entities.InsertRange(0, descendants);
                        }

                        curLevelNo++;

                    } while (descendants.Count > 0);
                }

            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entities;
        }

        #endregion

        #region Custom database function

        // reference: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/how-to-call-custom-database-functions

        [DbFunction("CmsModel.Store", "fnArticle_IsShowInLang")]
        public bool fnArticle_IsShowInLang(Guid ArticleId, string CultureName)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        #endregion
    }
}
