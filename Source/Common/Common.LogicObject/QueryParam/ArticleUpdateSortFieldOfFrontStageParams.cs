using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class ArticleUpdateSortFieldOfFrontStageParams
    {
        public Guid ArticleId;
        public string SortFieldOfFrontStage;
        public bool IsSortDescOfFrontStage;
        public string MdfAccount;
        public AuthenticationUpdateParams AuthUpdateParams;

        public ArticleUpdateSortFieldOfFrontStageParams()
        {
            AuthUpdateParams = new AuthenticationUpdateParams();
        }

        public ArticleUpdateSortFieldOfFrontStageParamsDA GenArticleUpdateSortFieldOfFrontStageParamsDA()
        {
            ArticleUpdateSortFieldOfFrontStageParamsDA result = new ArticleUpdateSortFieldOfFrontStageParamsDA()
            {
                ArticleId = ArticleId,
                SortFieldOfFrontStage = SortFieldOfFrontStage,
                IsSortDescOfFrontStage = IsSortDescOfFrontStage,
                MdfAccount = MdfAccount,
                AuthUpdateParams = AuthUpdateParams.GenAuthenticationUpdateParamsDA()
            };

            return result;
        }
    }
}
