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
    public class AttachFileParams
    {
        public Guid AttId;
        public Guid? ArticleId;
        public string FilePath;
        public string FileSavedName;
        public int FileSize;
        public int SortNo;
        public string FileMIME;
        public bool DontDelete;
        public string PostAccount;
    }
}
