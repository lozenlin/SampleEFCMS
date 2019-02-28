using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// 2019/02/28, lozenlin, modify, ArticleId 改為 Guid?
    /// </history>
    public class ArticleVideoParams
    {
        public Guid VidId;
        public Guid? ArticleId;
        public int SortNo;
        public string VidLinkUrl;
        public string SourceVideoId;
        public string PostAccount;
    }
}
