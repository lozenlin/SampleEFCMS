using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class ArticleUpdateIsAreaShowInFrontStageParamsDA
    {
        public Guid ArticleId;
        public string AreaName;
        public bool IsShowInFrontStage;
        public string MdfAccount;
        public AuthenticationUpdateParamsDA AuthUpdateParams;

        public ArticleUpdateIsAreaShowInFrontStageParamsDA()
        {
            AuthUpdateParams = new AuthenticationUpdateParamsDA();
        }
    }
}
