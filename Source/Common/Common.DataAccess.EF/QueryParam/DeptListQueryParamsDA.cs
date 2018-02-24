using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class DeptListQueryParamsDA
    {
        public string Kw = "";
        public PagedListQueryParamsDA PagedParams;
        public AuthenticationQueryParamsDA AuthParams;

        public DeptListQueryParamsDA()
        {
            PagedParams = new PagedListQueryParamsDA();
            AuthParams = new AuthenticationQueryParamsDA();
        }
    }
}
