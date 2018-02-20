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


        #endregion
    }
}
