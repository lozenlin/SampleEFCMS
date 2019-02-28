using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.QueryParam
{
    public class RoleListQueryParams
    {
        public string Kw = "";
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public RoleListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }
    }
}
