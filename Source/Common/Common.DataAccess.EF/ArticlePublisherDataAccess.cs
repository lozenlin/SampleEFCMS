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

        /// <summary>
        /// 刪除網頁內容
        /// </summary>
        public bool DeleteArticleData(Guid articleId)
        {
            Logger.Debug("DeleteArticleData(articleId)");
            DbContextTransaction tran = null;

            try
            {
                tran = cmsCtx.Database.BeginTransaction();

                // delete attachment
                cmsCtx.Database.ExecuteSqlCommand(@"
delete from dbo.AttachFileMultiLang
where exists(
	select *
	from dbo.AttachFile af
	where af.AttId=AttachFileMultiLang.AttId and af.ArticleId=@p0
)", articleId);

                cmsCtx.Database.ExecuteSqlCommand("delete from dbo.AttachFile where ArticleId = @p0", articleId);

                // delete picture
                cmsCtx.Database.ExecuteSqlCommand(@"
delete from dbo.ArticlePictureMultiLang
where exists(
	select *
	from dbo.ArticlePicture ap
	where ap.PicId=ArticlePictureMultiLang.PicId and ap.ArticleId=@p0
)", articleId);

                cmsCtx.Database.ExecuteSqlCommand("delete from dbo.ArticlePicture where ArticleId = @p0", articleId);

                // delete video
                cmsCtx.Database.ExecuteSqlCommand(@"
delete from dbo.ArticleVideoMultiLang
where exists(
	select *
	from dbo.ArticleVideo av
	where av.VidId=ArticleVideoMultiLang.VidId and av.ArticleId=@p0
)", articleId);

                cmsCtx.Database.ExecuteSqlCommand("delete from dbo.ArticleVideo where ArticleId = @p0", articleId);

                // delete multi language data
                cmsCtx.Database.ExecuteSqlCommand("delete from dbo.ArticleMultiLang where ArticleId = @p0", articleId);

                // delete main data
                Article entity = new Article() { ArticleId = articleId };
                cmsCtx.Entry<Article>(entity).State = EntityState.Deleted;

                cmsCtx.SaveChanges();

                tran.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;

                if (tran != null)
                    tran.Rollback();

                return false;
            }
            finally
            {
                if (tran != null)
                    tran.Dispose();
            }

            return true;
        }

        /// <summary>
        /// 加大網頁內容的排序編號
        /// </summary>
        public bool IncreaseArticleSortNo(Guid articleId, string mdfAccount)
        {
            Logger.Debug("IncreaseArticleSortNo(articleId, mdfAccount)");

            try
            {
                Article entity = cmsCtx.Article.Find(articleId);

                if (entity == null)
                {
                    throw new Exception("there is no data of articleId.");
                }

                // get bigger one
                Article biggerOne = cmsCtx.Article.Where(obj =>
                    obj.ParentId == entity.ParentId
                    && obj.ArticleId != entity.ArticleId
                    && obj.SortNo >= entity.SortNo)
                    .OrderBy(obj => obj.SortNo)
                    .FirstOrDefault();

                // there is no bigger one, exit
                if (biggerOne == null)
                {
                    return true;
                }

                int sortNo = entity.SortNo ?? 0;
                int biggerSortNo = biggerOne.SortNo ?? 0;

                // when the values are the same
                if (biggerSortNo == sortNo)
                {
                    biggerSortNo = sortNo + 1;
                }

                // swap
                entity.SortNo = biggerSortNo;
                entity.MdfAccount = mdfAccount;
                entity.MdfDate = DateTime.Now;

                biggerOne.SortNo = sortNo;
                biggerOne.MdfAccount = mdfAccount;
                biggerOne.MdfDate = DateTime.Now;

                cmsCtx.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 減小網頁內容的排序編號
        /// </summary>
        public bool DecreaseArticleSortNo(Guid articleId, string mdfAccount)
        {
            Logger.Debug("DecreaseArticleSortNo(articleId, mdfAccount)");

            try
            {
                Article entity = cmsCtx.Article.Find(articleId);

                if (entity == null)
                {
                    throw new Exception("there is no data of articleId.");
                }

                // get smaller one
                Article smallerOne = cmsCtx.Article.Where(obj =>
                    obj.ParentId == entity.ParentId
                    && obj.ArticleId != entity.ArticleId
                    && obj.SortNo <= entity.SortNo)
                    .OrderByDescending(obj => obj.SortNo)
                    .FirstOrDefault();

                // there is no smaller one, exit
                if (smallerOne == null)
                {
                    return true;
                }

                int sortNo = entity.SortNo ?? 0;
                int smallerSortNo = smallerOne.SortNo ?? 0;

                // when the values are the same
                if (smallerSortNo == sortNo)
                {
                    sortNo = smallerSortNo + 1;
                }

                // swap
                entity.SortNo = smallerSortNo;
                entity.MdfAccount = mdfAccount;
                entity.MdfDate = DateTime.Now;

                smallerOne.SortNo = sortNo;
                smallerOne.MdfAccount = mdfAccount;
                smallerOne.MdfDate = DateTime.Now;

                cmsCtx.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新網頁內容的指定區域是否在前台顯示
        /// </summary>
        public bool UpdateArticleIsAreaShowInFrontStage(ArticleUpdateIsAreaShowInFrontStageParamsDA param)
        {
            Logger.Debug("UpdateArticleIsAreaShowInFrontStage(param)");

            try
            {
                // get valid entity
                var tempQuery = from a in cmsCtx.Article
                                join e in cmsCtx.Employee on a.PostAccount equals e.EmpAccount
                                into articleGroup
                                from e in articleGroup.DefaultIfEmpty()
                                where a.ArticleId == param.ArticleId
                                    && (param.AuthUpdateParams.CanEditSubItemOfOthers
                                        || param.AuthUpdateParams.CanEditSubItemOfCrew && e.DeptId == param.AuthUpdateParams.MyDeptId
                                        || param.AuthUpdateParams.CanEditSubItemOfSelf && a.PostAccount == param.AuthUpdateParams.MyAccount)
                                select a;

                Article entity = tempQuery.FirstOrDefault();

                if (entity == null)
                {
                    throw new Exception("update failed");
                }

                switch (param.AreaName)
                {
                    case "ListArea":
                        entity.IsListAreaShowInFrontStage = param.IsShowInFrontStage;
                        break;
                    case "AttArea":
                        entity.IsAttAreaShowInFrontStage = param.IsShowInFrontStage;
                        break;
                    case "PicArea":
                        entity.IsPicAreaShowInFrontStage = param.IsShowInFrontStage;
                        break;
                    case "VideoArea":
                        entity.IsVideoAreaShowInFrontStage = param.IsShowInFrontStage;
                        break;
                }

                if (cmsCtx.Entry<Article>(entity).State == EntityState.Modified)
                {
                    entity.MdfAccount = param.MdfAccount;
                    entity.MdfDate = DateTime.Now;
                }

                cmsCtx.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新網頁內容的前台子項目排序欄位
        /// </summary>
        public bool UpdateArticleSortFieldOfFrontStage(ArticleUpdateSortFieldOfFrontStageParamsDA param)
        {
            Logger.Debug("UpdateArticleSortFieldOfFrontStage(param)");

            try
            {
                // get valid entity
                var tempQuery = from a in cmsCtx.Article
                                join e in cmsCtx.Employee on a.PostAccount equals e.EmpAccount
                                into articleGroup
                                from e in articleGroup.DefaultIfEmpty()
                                where a.ArticleId == param.ArticleId
                                    && (param.AuthUpdateParams.CanEditSubItemOfOthers
                                        || param.AuthUpdateParams.CanEditSubItemOfCrew && e.DeptId == param.AuthUpdateParams.MyDeptId
                                        || param.AuthUpdateParams.CanEditSubItemOfSelf && a.PostAccount == param.AuthUpdateParams.MyAccount)
                                select a;

                Article entity = tempQuery.FirstOrDefault();

                if (entity == null)
                {
                    throw new Exception("update failed");
                }

                entity.SortFieldOfFrontStage = param.SortFieldOfFrontStage;
                entity.IsSortDescOfFrontStage = param.IsSortDescOfFrontStage;
                entity.MdfAccount = param.MdfAccount;
                entity.MdfDate = DateTime.Now;

                cmsCtx.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return false;
            }

            return true;
        }

        #endregion

        #region Custom database function

        // reference: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/how-to-call-custom-database-functions

        [DbFunction("CmsModel.Store", "fnArticle_IsShowInLang")]
        private bool fnArticle_IsShowInLang(Guid ArticleId, string CultureName)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        #endregion
    }
}
