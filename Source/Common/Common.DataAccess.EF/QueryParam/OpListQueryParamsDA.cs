using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class OpListQueryParamsDA
    {
        public int ParentId;	// 0:root
        public string CultureName;
        public string Kw;
        public PagedListQueryParamsDA PagedParams;
        public AuthenticationQueryParamsDA AuthParams;

        public OpListQueryParamsDA()
        {
            PagedParams = new PagedListQueryParamsDA();
            AuthParams = new AuthenticationQueryParamsDA();
        }
    }
}
