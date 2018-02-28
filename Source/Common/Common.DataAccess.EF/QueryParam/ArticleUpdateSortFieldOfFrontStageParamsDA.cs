using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class ArticleUpdateSortFieldOfFrontStageParamsDA
    {
        public Guid ArticleId;
        public string SortFieldOfFrontStage;
        public bool IsSortDescOfFrontStage;
        public string MdfAccount;
        public AuthenticationUpdateParamsDA AuthUpdateParams;

        public ArticleUpdateSortFieldOfFrontStageParamsDA()
        {
            AuthUpdateParams = new AuthenticationUpdateParamsDA();
        }
    }
}
