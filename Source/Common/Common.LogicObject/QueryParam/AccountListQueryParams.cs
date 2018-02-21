using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class AccountListQueryParams
    {
        public int DeptId = 0;
        public string Kw = "";
        public int ListMode = 0;
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public AccountListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }

        public AccountListQueryParamsDA GenAccountListQueryParamsDA()
        {
            AccountListQueryParamsDA result = new AccountListQueryParamsDA()
            {
                DeptId = DeptId,
                Kw = Kw,
                ListMode = ListMode,
                PagedParams = PagedParams.GenPagedListQueryParamsDA(),
                AuthParams = AuthParams.GenAuthenticationQueryParamsDA()
            };

            return result;
        }
    }
}
