using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class AccountListQueryParamsDA
    {
        public int DeptId = 0;
        public string Kw = "";
        public int ListMode = 0;
        public PagedListQueryParamsDA PagedParams;
        public AuthenticationQueryParamsDA AuthParams;

        public AccountListQueryParamsDA()
        {
            PagedParams = new PagedListQueryParamsDA();
            AuthParams = new AuthenticationQueryParamsDA();
        }
    }
}
