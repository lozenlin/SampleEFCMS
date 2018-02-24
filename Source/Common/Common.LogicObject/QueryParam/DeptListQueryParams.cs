using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class DeptListQueryParams
    {
        public string Kw = "";
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public DeptListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }

        public DeptListQueryParamsDA GenDeptListQueryParamsDA()
        {
            DeptListQueryParamsDA result = new DeptListQueryParamsDA()
            {
                Kw = Kw,
                PagedParams = PagedParams.GenPagedListQueryParamsDA(),
                AuthParams = AuthParams.GenAuthenticationQueryParamsDA()
            };

            return result;
        }
    }
}
