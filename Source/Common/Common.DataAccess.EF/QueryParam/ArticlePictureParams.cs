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
    public class ArticlePictureParams
    {
        public Guid PicId;
        public Guid? ArticleId;
        public string FileSavedName;
        public int FileSize;
        public int SortNo;
        public string FileMIME;
        public string PostAccount;
    }
}
