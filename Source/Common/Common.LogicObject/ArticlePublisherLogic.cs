// ===============================================================================
// ArticlePublisherLogic of SampleEFCMS
// https://github.com/lozenlin/SampleEFCMS
//
// ArticlePublisherLogic.cs
//
// ===============================================================================
// Copyright (c) 2018 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using Common.DataAccess;
using Common.DataAccess.ArticlePublisher;
using Common.DataAccess.EF;
using Common.DataAccess.EF.EntityRequiredPropValues;
using Common.DataAccess.EF.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// 網頁內容發佈(上稿)
    /// </summary>
    public class ArticlePublisherLogic : ICustomEmployeeAuthorizationResult
    {
        protected ILog logger = null;
        protected string dbErrMsg = "";
        protected IAuthenticationConditionProvider authCondition;

        /// <summary>
        /// 網頁內容發佈(上稿)
        /// </summary>
        public ArticlePublisherLogic()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// 網頁內容發佈(上稿)
        /// </summary>
        public ArticlePublisherLogic(IAuthenticationConditionProvider authCondition)
            : this()
        {
            this.authCondition = authCondition;
        }

        // DataAccess functions

        /// <summary>
        /// DB command 執行後的錯誤訊息
        /// </summary>
        public string GetDbErrMsg()
        {
            return dbErrMsg;
        }

        #region Article DataAccess functions

        /// <summary>
        /// 取得後台用網頁內容資料
        /// </summary>
        public ArticleForBackend GetArticleDataForBackend(Guid articleId)
        {
            ArticleForBackend entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.GetArticleDataForBackend(articleId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得後台用網頁內容的多國語系資料
        /// </summary>
        public ArticleMultiLang GetArticleMultiLangDataForBackend(Guid articleId, string cultureName)
        {
            ArticleMultiLang entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.Get<ArticleMultiLang>(articleId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得前台用網頁內容資料
        /// </summary>
        public ArticleForFrontend GetArticleDataForFrontend(Guid articleId, string cultureName)
        {
            ArticleForFrontend entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.GetArticleDataForFrontend(articleId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得網頁內容最大排序編號
        /// </summary>
        public int GetArticleMaxSortNo(Guid parentId)
        {
            int result = 0;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.GetArticleMaxSortNo(parentId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 新增網頁內容
        /// </summary>
        public bool InsertArticleData(ArticleParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                Article entity = new Article()
                {
                    ArticleId = param.ArticleId,
                    ParentId = param.ParentId,
                    ArticleAlias = param.ArticleAlias,
                    BannerPicFileName = param.BannerPicFileName,
                    LayoutModeId = param.LayoutModeId,
                    ShowTypeId = param.ShowTypeId,
                    LinkUrl = param.LinkUrl,
                    LinkTarget = param.LinkTarget,
                    ControlName = param.ControlName,
                    SubItemControlName = param.SubItemControlName,
                    IsHideSelf = param.IsHideSelf,
                    IsHideChild = param.IsHideChild,
                    StartDate = param.StartDate,
                    EndDate = param.EndDate,
                    SortNo = param.SortNo,
                    DontDelete = param.DontDelete,
                    PostAccount = param.PostAccount,
                    SubjectAtBannerArea = param.SubjectAtBannerArea,
                    PublishDate = param.PublishDate,
                    IsShowInUnitArea = param.IsShowInUnitArea,
                    IsShowInSitemap = param.IsShowInSitemap,
                    SortFieldOfFrontStage = param.SortFieldOfFrontStage,
                    IsSortDescOfFrontStage = param.IsSortDescOfFrontStage,
                    IsListAreaShowInFrontStage = param.IsListAreaShowInFrontStage,
                    IsAttAreaShowInFrontStage = param.IsAttAreaShowInFrontStage,
                    IsPicAreaShowInFrontStage = param.IsPicAreaShowInFrontStage,
                    IsVideoAreaShowInFrontStage = param.IsVideoAreaShowInFrontStage,
                    SubItemLinkUrl = param.SubItemLinkUrl
                };

                insResult = artPubDao.InsertArticleData(entity);
                dbErrMsg = artPubDao.GetErrMsg();

                if (!insResult.IsSuccess)
                {
                    if (artPubDao.GetSqlErrNumber() == 50000 && artPubDao.GetSqlErrState() == 2)
                    {
                        param.HasIdBeenUsed = true;
                    }
                    else if (artPubDao.GetSqlErrNumber() == 50000 && artPubDao.GetSqlErrState() == 3)
                    {
                        param.HasAliasBeenUsed = true;
                    }
                }
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 新增網頁內容的多國語系資料
        /// </summary>
        public bool InsertArticleMultiLangData(ArticleMultiLangParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleMultiLang entity = new ArticleMultiLang()
                {
                    ArticleId = param.ArticleId,
                    CultureName = param.CultureName,
                    ArticleSubject = param.ArticleSubject,
                    ArticleContext = param.ArticleContext,
                    IsShowInLang = param.IsShowInLang,
                    PostAccount = param.PostAccount,
                    Subtitle = param.Subtitle,
                    PublisherName = param.PublisherName,
                    TextContext = param.TextContext,
                    PostDate = DateTime.Now
                };

                insResult = artPubDao.Insert<ArticleMultiLang>(entity);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 更新網頁內容
        /// </summary>
        public bool UpdateArticleData(ArticleParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                Article entity = artPubDao.GetEmptyEntity<Article>(new ArticleRequiredPropValues()
                {
                    ArticleId = param.ArticleId,
                    IsHideSelf = !param.IsHideSelf,
                    IsHideChild = !param.IsHideChild,
                    DontDelete = !param.DontDelete,
                    SubjectAtBannerArea = !param.SubjectAtBannerArea,
                    IsShowInUnitArea = !param.IsShowInUnitArea,
                    IsShowInSitemap = !param.IsShowInSitemap,
                    IsSortDescOfFrontStage = !param.IsSortDescOfFrontStage,
                    IsListAreaShowInFrontStage = !param.IsListAreaShowInFrontStage,
                    IsAttAreaShowInFrontStage = !param.IsAttAreaShowInFrontStage,
                    IsPicAreaShowInFrontStage = !param.IsPicAreaShowInFrontStage,
                    IsVideoAreaShowInFrontStage = !param.IsVideoAreaShowInFrontStage
                });

                entity.ArticleAlias = param.ArticleAlias;
                entity.BannerPicFileName = param.BannerPicFileName;
                entity.LayoutModeId = param.LayoutModeId;
                entity.ShowTypeId = param.ShowTypeId;
                entity.LinkUrl = param.LinkUrl;
                entity.LinkTarget = param.LinkTarget;
                entity.ControlName = param.ControlName;
                entity.SubItemControlName = param.SubItemControlName;
                entity.IsHideSelf = param.IsHideSelf;
                entity.IsHideChild = param.IsHideChild;
                entity.StartDate = param.StartDate;
                entity.EndDate = param.EndDate;
                entity.SortNo = param.SortNo;
                entity.DontDelete = param.DontDelete;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;
                entity.SubjectAtBannerArea = param.SubjectAtBannerArea;
                entity.PublishDate = param.PublishDate;
                entity.IsShowInUnitArea = param.IsShowInUnitArea;
                entity.IsShowInSitemap = param.IsShowInSitemap;
                entity.SortFieldOfFrontStage = param.SortFieldOfFrontStage;
                entity.IsSortDescOfFrontStage = param.IsSortDescOfFrontStage;
                entity.IsListAreaShowInFrontStage = param.IsListAreaShowInFrontStage;
                entity.IsAttAreaShowInFrontStage = param.IsAttAreaShowInFrontStage;
                entity.IsPicAreaShowInFrontStage = param.IsPicAreaShowInFrontStage;
                entity.IsVideoAreaShowInFrontStage = param.IsVideoAreaShowInFrontStage;
                entity.SubItemLinkUrl = param.SubItemLinkUrl;

                result = artPubDao.UpdateArticleData(entity);
                dbErrMsg = artPubDao.GetErrMsg();

                if (!result)
                {
                    if (artPubDao.GetSqlErrNumber() == 50000 && artPubDao.GetSqlErrState() == 3)
                    {
                        param.HasAliasBeenUsed = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 更新網頁內容的多國語系資料
        /// </summary>
        public bool UpdateArticleMultiLangData(ArticleMultiLangParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleMultiLang entity = artPubDao.GetEmptyEntity<ArticleMultiLang>(new ArticleMultiLangRequiredPropValues()
                {
                    ArticleId = param.ArticleId,
                    CultureName = param.CultureName,
                    IsShowInLang = !param.IsShowInLang
                });

                entity.ArticleSubject = param.ArticleSubject;
                entity.ArticleContext = param.ArticleContext;
                entity.IsShowInLang = param.IsShowInLang;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;
                entity.Subtitle = param.Subtitle;
                entity.PublisherName = param.PublisherName;
                entity.TextContext = param.TextContext;

                result = artPubDao.Update();
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得作業選單用指定語系的網頁內容清單
        /// </summary>
        public List<ArticleMultiLangForOpMenu> GetArticleMultiLangListForOpMenu(Guid parentId, string cultureName)
        {
            List<ArticleMultiLangForOpMenu> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticleMultiLangListForOpMenu(parentId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得後台用指定語系的網頁內容清單
        /// </summary>
        public List<ArticleForBEList> GetArticleMultiLangListForBackend(ArticleListQueryParams param)
        {
            List<ArticleForBEList> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleListQueryParamsDA paramDA = param.GenArticleListQueryParamsDA();
                entities = artPubDao.GetArticleMultiLangListForBackend(paramDA);
                dbErrMsg = artPubDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 取得前台用的有效網頁內容清單
        /// </summary>
        public List<ArticleForFEList> GetArticleValidListForFrontend(ArticleValidListQueryParams param)
        {
            List<ArticleForFEList> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleValidListQueryParamsDA paramDA = param.GenArticleValidListQueryParamsDA();
                entities = artPubDao.GetArticleValidListForFrontend(paramDA);
                dbErrMsg = artPubDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 刪除網頁內容
        /// </summary>
        public bool DeleteArticleData(Guid articleId)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.DeleteArticleData(articleId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 加大網頁內容的排序編號
        /// </summary>
        public bool IncreaseArticleSortNo(Guid articleId, string mdfAccount)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.IncreaseArticleSortNo(articleId, mdfAccount);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 減小網頁內容的排序編號
        /// </summary>
        public bool DecreaseArticleSortNo(Guid articleId, string mdfAccount)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.DecreaseArticleSortNo(articleId, mdfAccount);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得指定語系的網頁內容階層資料
        /// </summary>
        public List<ArticleMultiLangLevelInfo> GetArticleMultiLangLevelInfo(Guid articleId, string cultureName)
        {
            List<ArticleMultiLangLevelInfo> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticleMultiLangLevelInfoList(articleId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 更新網頁內容的指定區域是否在前台顯示
        /// </summary>
        public bool UpdateArticleIsAreaShowInFrontStage(ArticleUpdateIsAreaShowInFrontStageParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleUpdateIsAreaShowInFrontStageParamsDA paramDA = param.GenArticleUpdateIsAreaShowInFrontStageParamsDA();
                result = artPubDao.UpdateArticleIsAreaShowInFrontStage(paramDA);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 更新網頁內容的前台子項目排序欄位
        /// </summary>
        public bool UpdateArticleSortFieldOfFrontStage(ArticleUpdateSortFieldOfFrontStageParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleUpdateSortFieldOfFrontStageParamsDA paramDA = param.GenArticleUpdateSortFieldOfFrontStageParamsDA();
                result = artPubDao.UpdateArticleSortFieldOfFrontStage(paramDA);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 依網址別名取得網頁代碼
        /// </summary>
        public Guid? GetArticleIdByAlias(string articleAlias)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_GetArticleIdByAlias cmdInfo = new spArticle_GetArticleIdByAlias() { ArticleAlias = articleAlias };

            Guid errCode = new Guid("093F6F50-FC1C-42A9-927B-595A39F6C8D9");
            Guid result = cmd.ExecuteScalar<Guid>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            if (result == errCode)
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// 依超連結網址取得網頁代碼
        /// </summary>
        public Guid? GetArticleIdByLinkUrl(string linkUrl)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_GetArticleIdByLinkUrl cmdInfo = new spArticle_GetArticleIdByLinkUrl() { LinkUrl = linkUrl };

            Guid errCode = new Guid("093F6F50-FC1C-42A9-927B-595A39F6C8D9");
            Guid result = cmd.ExecuteScalar<Guid>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            if (result == errCode)
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// 取得指定網頁內容的前幾層網頁代碼
        /// </summary>
        public DataSet GetArticleTopLevelIds(Guid articleId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_GetTopLevelIds cmdInfo = new spArticle_GetTopLevelIds() { ArticleId = articleId };

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 增加網頁內容的多國語系資料被點閱次數
        /// </summary>
        public bool IncreaseArticleMultiLangReadCount(Guid articleId, string cultureName)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleMultiLang entity = artPubDao.Get<ArticleMultiLang>(articleId, cultureName);

                if (entity != null)
                {
                    entity.ReadCount++;
                    result = artPubDao.Update();
                    dbErrMsg = artPubDao.GetErrMsg();
                }
            }

            return result;
        }

        /// <summary>
        /// 取得使用在單元區的有效網頁清單
        /// </summary>
        public List<ArticleForFEUnitArea> GetArticleValidListForUnitArea(Guid parentId, string cultureName, bool isShowInUnitArea)
        {
            List<ArticleForFEUnitArea> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticleValidListForUnitArea(parentId, cultureName, isShowInUnitArea);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得使用在側邊區塊的有效網頁清單
        /// </summary>
        public List<ArticleForFESideSection> GetArticleValidListForSideSection(Guid parentId, string cultureName)
        {
            List<ArticleForFESideSection> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticleValidListForSideSection(parentId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得使用在網站導覽的有效網頁清單
        /// </summary>
        public List<ArticleForFESitemap> GetArticleValidListForSitemap(Guid parentId, string cultureName)
        {
            List<ArticleForFESitemap> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticleValidListForSitemap(parentId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得網頁的所有子網頁
        /// </summary>
        public List<ArticleDescendant> GetArticleDescendants(Guid articleId)
        {
            List<ArticleDescendant> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticleDescendants(articleId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        #endregion

        #region AttachFile DataAccess functions

        /// <summary>
        /// 取得後台用附件檔案資料
        /// </summary>
        public AttachFile GetAttachFileDataForBackend(Guid attId)
        {
            AttachFile entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.Get<AttachFile>(attId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得後台用附件檔案的多國語系資料
        /// </summary>
        public AttachFileMultiLang GetAttachFileMultiLangDataForBackend(Guid attId, string cultureName)
        {
            AttachFileMultiLang entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.Get<AttachFileMultiLang>(attId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得附件檔案的最大排序編號
        /// </summary>
        public int GetAttachFileMaxSortNo(Guid? articleId)
        {
            int result = 0;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.GetAttachFileMaxSortNo(articleId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 新增附件檔案資料
        /// </summary>
        public bool InsertAttachFileData(AttachFileParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                AttachFile entity = new AttachFile()
                {
                    AttId = param.AttId,
                    ArticleId = param.ArticleId,
                    FilePath = param.FilePath,
                    FileSavedName = param.FileSavedName,
                    FileSize = param.FileSize,
                    SortNo = param.SortNo,
                    FileMIME = param.FileMIME,
                    DontDelete = param.DontDelete,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = artPubDao.Insert<AttachFile>(entity);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 新增附件檔案的多國語系資料
        /// </summary>
        public bool InsertAttachFileMultiLangData(AttachFileMultiLangParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                AttachFileMultiLang entity = new AttachFileMultiLang()
                {
                    AttId = param.AttId,
                    CultureName = param.CultureName,
                    AttSubject = param.AttSubject,
                    IsShowInLang = param.IsShowInLang,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = artPubDao.Insert<AttachFileMultiLang>(entity);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 更新附件檔案資料
        /// </summary>
        public bool UpdateAttachFileData(AttachFileParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                AttachFile entity = artPubDao.GetEmptyEntity<AttachFile>(new AttachFileRequiredPropValues()
                {
                    AttId = param.AttId,
                    DontDelete = !param.DontDelete
                });

                entity.FilePath = param.FilePath;
                entity.FileSavedName = param.FileSavedName;
                entity.FileSize = param.FileSize;
                entity.SortNo = param.SortNo;
                entity.FileMIME = param.FileMIME;
                entity.DontDelete = param.DontDelete;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = artPubDao.Update();
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 更新附件檔案的多國語系資料
        /// </summary>
        public bool UpdateAttachFileMultiLangData(AttachFileMultiLangParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                AttachFileMultiLang entity = artPubDao.GetEmptyEntity<AttachFileMultiLang>(new AttachFileMultiLangRequiredPropValues()
                {
                    AttId = param.AttId,
                    CultureName = param.CultureName,
                    IsShowInLang = !param.IsShowInLang
                });

                entity.AttSubject = param.AttSubject;
                entity.IsShowInLang = param.IsShowInLang;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = artPubDao.Update();
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 刪除附件檔案資料
        /// </summary>
        public bool DeleteAttachFileData(Guid attId)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.DeleteAttachFileData(attId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得後台用指定語系的附件檔案清單
        /// </summary>
        public List<AttachFileForBEList> GetAttachFileMultiLangListForBackend(AttachFileListQueryParams param)
        {
            List<AttachFileForBEList> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                AttachFileListQueryParamsDA paramDA = param.GenAttachFileListQueryParamsDA();
                entities = artPubDao.GetAttachFileMultiLangListForBackend(paramDA);
                dbErrMsg = artPubDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 加大附件檔案的排序編號
        /// </summary>
        public bool IncreaseAttachFileSortNo(Guid attId, string mdfAccount)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.IncreaseAttachFileSortNo(attId, mdfAccount);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 減小附件檔案的排序編號
        /// </summary>
        public bool DecreaseAttachFileSortNo(Guid attId, string mdfAccount)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.DecreaseAttachFileSortNo(attId, mdfAccount);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 增加附件檔案的多國語系資料被點閱次數
        /// </summary>
        public bool IncreaseAttachFileMultiLangReadCount(Guid attId, string cultureName)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                AttachFileMultiLang entity = artPubDao.Get<AttachFileMultiLang>(attId, cultureName);

                if (entity != null)
                {
                    entity.ReadCount++;

                    result = artPubDao.Update();
                    dbErrMsg = artPubDao.GetErrMsg();
                }
            }

            return result;
        }

        /// <summary>
        /// 取得前台用附件檔案清單
        /// </summary>
        public List<AttachFileForFrontend> GetAttachFileListForFrontend(Guid articleId, string cultureName)
        {
            List<AttachFileForFrontend> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetAttachFileListForFrontend(articleId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        #endregion

        #region ArticlePicture DataAccess functions

        /// <summary>
        /// 取得後台用網頁照片資料
        /// </summary>
        public ArticlePicture GetArticlePictureDataForBackend(Guid picId)
        {
            ArticlePicture entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.Get<ArticlePicture>(picId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得後台用網頁照片的多國語系資料
        /// </summary>
        public ArticlePictureMultiLang GetArticlePictureMultiLangDataForBackend(Guid picId, string cultureName)
        {
            ArticlePictureMultiLang entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.Get<ArticlePictureMultiLang>(picId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得網頁照片的最大排序編號
        /// </summary>
        public int GetArticlePictureMaxSortNo(Guid? articleId)
        {
            int result = 0;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.GetArticlePictureMaxSortNo(articleId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 刪除網頁照片資料
        /// </summary>
        public bool DeleteArticlePictureData(Guid picId)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.DeleteArticlePictureData(picId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 新增網頁照片資料
        /// </summary>
        public bool InsertArticlePictureData(ArticlePictureParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticlePicture entity = new ArticlePicture()
                {
                    PicId = param.PicId,
                    ArticleId = param.ArticleId,
                    FileSavedName = param.FileSavedName,
                    FileSize = param.FileSize,
                    SortNo = param.SortNo,
                    FileMIME = param.FileMIME,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = artPubDao.Insert<ArticlePicture>(entity);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 新增網頁照片的多國語系資料
        /// </summary>
        public bool InsertArticlePictureMultiLangData(ArticlePictureMultiLangParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticlePictureMultiLang entity = new ArticlePictureMultiLang()
                {
                    PicId = param.PicId,
                    CultureName = param.CultureName,
                    PicSubject = param.PicSubject,
                    IsShowInLang = param.IsShowInLang,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = artPubDao.Insert<ArticlePictureMultiLang>(entity);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 更新網頁照片資料
        /// </summary>
        public bool UpdateArticlePictureData(ArticlePictureParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticlePicture entity = artPubDao.GetEmptyEntity<ArticlePicture>(new ArticlePictureRequiredPropValues()
                {
                    PicId = param.PicId
                });

                entity.FileSavedName = param.FileSavedName;
                entity.FileSize = param.FileSize;
                entity.SortNo = param.SortNo;
                entity.FileMIME = param.FileMIME;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = artPubDao.Update();
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 更新網頁照片的多國語系資料
        /// </summary>
        public bool UpdateArticlePictureMultiLangData(ArticlePictureMultiLangParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticlePictureMultiLang entity = artPubDao.GetEmptyEntity<ArticlePictureMultiLang>(new ArticlePictureMultiLangRequiredPropValues()
                {
                    PicId = param.PicId,
                    CultureName = param.CultureName,
                    IsShowInLang = !param.IsShowInLang
                });

                entity.PicSubject = param.PicSubject;
                entity.IsShowInLang = param.IsShowInLang;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = artPubDao.Update();
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得後台用指定語系的網頁照片清單
        /// </summary>
        public List<ArticlePictureForBEList> GetArticlePicutreMultiLangListForBackend(ArticlePictureListQueryParams param)
        {
            List<ArticlePictureForBEList> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticlePictureListQueryParamsDA paramDA = param.GenArticlePictureListQueryParamsDA();
                entities = artPubDao.GetArticlePicutreMultiLangListForBackend(paramDA);
                dbErrMsg = artPubDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 取得前台用網頁照片清單
        /// </summary>
        public List<ArticlePictureForFrontend> GetArticlePictureListForFrontend(Guid articleId, string cultureName)
        {
            List<ArticlePictureForFrontend> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticlePictureListForFrontend(articleId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        #endregion

        #region ArticleVideo DataAccess functions

        /// <summary>
        /// 取得後台用網頁影片資料
        /// </summary>
        public ArticleVideo GetArticleVideoDataForBackend(Guid vidId)
        {
            ArticleVideo entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.Get<ArticleVideo>(vidId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得後台用網頁影片的多國語系資料
        /// </summary>
        public ArticleVideoMultiLang GetArticleVideoMultiLangDataForBackend(Guid vidId, string cultureName)
        {
            ArticleVideoMultiLang entity = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entity = artPubDao.Get<ArticleVideoMultiLang>(vidId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得網頁影片的最大排序編號
        /// </summary>
        public int GetArticleVideoMaxSortNo(Guid articleId)
        {
            int result = 0;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.GetArticleVideoMaxSortNo(articleId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 新增網頁影片資料
        /// </summary>
        public bool InsertArticleVideoData(ArticleVideoParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleVideo entity = new ArticleVideo()
                {
                    VidId = param.VidId,
                    ArticleId = param.ArticleId,
                    SortNo = param.SortNo,
                    VidLinkUrl = param.VidLinkUrl,
                    SourceVideoId = param.SourceVideoId,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = artPubDao.Insert<ArticleVideo>(entity);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 新增網頁影片的多國語系資料
        /// </summary>
        public bool InsertArticleVideoMultiLangData(ArticleVideoMultiLangParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleVideoMultiLang entity = new ArticleVideoMultiLang()
                {
                    VidId = param.VidId,
                    CultureName = param.CultureName,
                    VidSubject = param.VidSubject,
                    VidDesc = param.VidDesc,
                    IsShowInLang = param.IsShowInLang,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = artPubDao.Insert<ArticleVideoMultiLang>(entity);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 更新網頁影片資料
        /// </summary>
        public bool UpdateArticleVideoData(ArticleVideoParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleVideo entity = artPubDao.GetEmptyEntity<ArticleVideo>(new ArticleVideoRequiredPropValues()
                {
                    VidId = param.VidId
                });

                entity.SortNo = param.SortNo;
                entity.VidLinkUrl = param.VidLinkUrl;
                entity.SourceVideoId = param.SourceVideoId;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = artPubDao.Update();
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 更新網頁影片的多國語系資料
        /// </summary>
        public bool UpdateArticleVideoMultiLangData(ArticleVideoMultiLangParams param)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleVideoMultiLang entity = artPubDao.GetEmptyEntity<ArticleVideoMultiLang>(new ArticleVideoMultiLangRequiredPropValues()
                {
                    VidId = param.VidId,
                    CultureName = param.CultureName,
                    IsShowInLang = !param.IsShowInLang
                });

                entity.VidSubject = param.VidSubject;
                entity.VidDesc = param.VidDesc;
                entity.IsShowInLang = param.IsShowInLang;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = artPubDao.Update();
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得後台用指定語系的網頁影片清單
        /// </summary>
        public List<ArticleVideoForBEList> GetArticleVideoMultiLangListForBackend(ArticleVideoListQueryParams param)
        {
            List<ArticleVideoForBEList> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                ArticleVideoListQueryParamsDA paramDA = param.GenArticleVideoListQueryParamsDA();
                entities = artPubDao.GetArticleVideoMultiLangListForBackend(paramDA);
                dbErrMsg = artPubDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 刪除網頁影片資料
        /// </summary>
        public bool DeleteArticleVideoData(Guid vidId)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.DeleteArticleVideoData(vidId);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得前台用網頁影片清單
        /// </summary>
        public List<ArticleVideoForFrontend> GetArticleVideoListForFrontend(Guid articleId, string cultureName)
        {
            List<ArticleVideoForFrontend> entities = null;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                entities = artPubDao.GetArticleVideoListForFrontend(articleId, cultureName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return entities;
        }

        #endregion

        #region Keyword DataAccess functions

        /// <summary>
        /// 儲存搜尋關鍵字
        /// </summary>
        public bool SaveKeywordData(string cultureName, string kw)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.SaveKeywordData(cultureName, kw);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得前台用搜尋關鍵字
        /// </summary>
        public DataSet GetKeywordListForFrontend(string cultureName, string kw, int topCount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spKeyword_GetListForFrontend cmdInfo = new spKeyword_GetListForFrontend()
            {
                CultureName = cultureName,
                Kw = kw,
                TopCount = topCount
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        #endregion

        #region SearchDataSource DataAccess functions

        /// <summary>
        /// 建立搜尋用資料來源
        /// </summary>
        public bool BuildSearchDataSource(string mainLinkUrl)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.BuildSearchDataSource(mainLinkUrl);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得搜尋用資料來源清單
        /// </summary>
        /// <returns></returns>
        public DataSet GetSearchDataSourceList(SearchResultListQueryParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spSearchDataSource_GetList cmdInfo = new spSearchDataSource_GetList()
            {
                Keywords = param.Keywords,
                CultureName = param.CultureName,
                BeginNum = param.PagedParams.BeginNum,
                EndNum = param.PagedParams.EndNum,
                SortField = param.PagedParams.SortField,
                IsSortDesc = param.PagedParams.IsSortDesc
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            param.PagedParams.RowCount = cmdInfo.RowCount;
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        #endregion

        #region msdb DataAccess functions

        /// <summary>
        /// 指示 SQL Server Agent 立即執行作業
        /// </summary>
        public bool CallSqlServerAgentJob(string jobName)
        {
            bool result = false;

            using (ArticlePublisherDataAccess artPubDao = new ArticlePublisherDataAccess())
            {
                result = artPubDao.CallSqlServerAgentJob(jobName);
                dbErrMsg = artPubDao.GetErrMsg();
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 從資料集載入身分的授權設定
        /// </summary>
        protected EmployeeAuthorizations LoadRoleAuthorizationsFromDataSet(EmployeeAuthorizations authorizations, EmployeeRoleOperationsDesc roleOp, bool isRoleAdmin)
        {
            if (isRoleAdmin)
            {
                // admin, open all
                authorizations.CanRead = true;
                authorizations.CanEdit = true;

                authorizations.CanReadSubItemOfSelf = true;
                authorizations.CanEditSubItemOfSelf = true;
                authorizations.CanAddSubItemOfSelf = true;
                authorizations.CanDelSubItemOfSelf = true;

                authorizations.CanReadSubItemOfCrew = true;
                authorizations.CanEditSubItemOfCrew = true;
                authorizations.CanDelSubItemOfCrew = true;

                authorizations.CanReadSubItemOfOthers = true;
                authorizations.CanEditSubItemOfOthers = true;
                authorizations.CanDelSubItemOfOthers = true;
            }
            else
            {
                if (roleOp == null)
                {
                    // no data, close all
                    authorizations.CanRead = false;
                    authorizations.CanEdit = false;

                    authorizations.CanReadSubItemOfSelf = false;
                    authorizations.CanEditSubItemOfSelf = false;
                    authorizations.CanAddSubItemOfSelf = false;
                    authorizations.CanDelSubItemOfSelf = false;

                    authorizations.CanReadSubItemOfCrew = false;
                    authorizations.CanEditSubItemOfCrew = false;
                    authorizations.CanDelSubItemOfCrew = false;

                    authorizations.CanReadSubItemOfOthers = false;
                    authorizations.CanEditSubItemOfOthers = false;
                    authorizations.CanDelSubItemOfOthers = false;
                }
                else
                {
                    // load settings
                    authorizations.ImportDataFrom(roleOp);
                }
            }

            return authorizations;
        }

        #region ICustomEmployeeAuthorizationResult

        public EmployeeAuthorizationsWithOwnerInfoOfDataExamined InitialAuthorizationResult(bool isTopPageOfOperation, EmployeeAuthorizations authorizations)
        {
            EmployeeAuthorizationsWithOwnerInfoOfDataExamined authAndOwner = new EmployeeAuthorizationsWithOwnerInfoOfDataExamined(authorizations);

            bool gotOpAuth = false;
            Guid initArticleId= authCondition.GetArticleId();
            Guid curArticleId = initArticleId;
            Guid? curParentId = null;
            int curArticleLevelNo;
            string linkUrl = "";
            bool isRoot = false;
            bool isRoleAdmin = authCondition.IsInRole("admin");

            // get article info
            ArticleForBackend article = GetArticleDataForBackend(curArticleId);

            if (article != null)
            {
                if (!article.ParentId.HasValue)
                {
                    isRoot = true;
                }
                else
                {
                    curParentId = article.ParentId;
                }

                curArticleLevelNo = article.ArticleLevelNo.Value;

                authAndOwner.OwnerAccountOfDataExamined = article.PostAccount;
                authAndOwner.OwnerDeptIdOfDataExamined = article.PostDeptId;
            }

            if (isRoot || isRoleAdmin)
            {
                return authAndOwner;
            }

            do
            {
                OperationOpInfo opInfo = null;
                string dbErrMsg = "";

                using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
                {
                    if (curParentId.HasValue)
                    {
                        // get opId by LinkUrl
                        linkUrl = string.Format("Article-Node.aspx?artid={0}", curArticleId);
                        opInfo = empAuthDao.GetOperationOpInfoByLinkUrl(linkUrl);
                    }
                    else
                    {
                        // get opId of root
                        opInfo = empAuthDao.GetOperationOpInfoByCommonClass("ArticleCommonOfBackend");
                    }

                    dbErrMsg = empAuthDao.GetErrMsg();
                }

                if (opInfo != null)
                {
                    int opId = opInfo.OpId;

                    // get authorizations
                    using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
                    {
                        EmployeeRoleOperationsDesc roleOp = empAuthDao.GetEmployeeRoleOperationsDescDataOfOp(authCondition.GetRoleName(), opId);

                        if (roleOp != null)
                        {
                            //檢查權限, 只允許 CanRead=true

                            if (roleOp.CanRead)
                            {
                                authAndOwner = (EmployeeAuthorizationsWithOwnerInfoOfDataExamined)LoadRoleAuthorizationsFromDataSet(authAndOwner, roleOp, isRoleAdmin);
                                gotOpAuth = true;
                            }
                        }
                    }
                }

                if (!gotOpAuth)
                {
                    if (!curParentId.HasValue)
                    {
                        // this is root
                        break;
                    }

                    // get parent info
                    ArticleForBackend parent = GetArticleDataForBackend(curParentId.Value);

                    if (parent == null)
                    {
                        logger.Error(string.Format("can not get article data of {0}", curParentId.Value));
                        break;
                    }

                    // move to parent level
                    curArticleId = curParentId.Value;

                    if (!parent.ParentId.HasValue)
                    {
                        curParentId = null;
                    }
                    else
                    {
                        curParentId = parent.ParentId;
                    }

                    curArticleLevelNo = parent.ArticleLevelNo.Value;
                }
            } while (!gotOpAuth);

            if (isTopPageOfOperation && curArticleId != initArticleId)
            {
                // notice that the authorizations belong to parent, so this page is not top page of operation.
                authAndOwner.IsTopPageOfOperation = false;
                authAndOwner.IsTopPageOfOperationChanged = true;
            }

            return authAndOwner;
        }

        #endregion
    }
}
