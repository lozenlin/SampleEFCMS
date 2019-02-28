using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    public class OpListQueryParams
    {
        public int ParentId;	// 0:root
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public OpListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams()
            {
                CanReadSubItemOfOthers = true,
                CanReadSubItemOfCrew = true,
                CanReadSubItemOfSelf = true
            };
        }
    }
}
