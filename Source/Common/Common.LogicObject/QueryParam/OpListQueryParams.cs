using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class OpListQueryParams
    {
        public int ParentId;	// 0:root
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;

        public OpListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
        }

        public OpListQueryParamsDA GenOpListQueryParamsDA()
        {
            OpListQueryParamsDA result = new OpListQueryParamsDA()
            {
                ParentId = ParentId,
                CultureName = CultureName,
                Kw = Kw,
                PagedParams = PagedParams.GenPagedListQueryParamsDA(),
                AuthParams = new AuthenticationQueryParamsDA()
                {
                    CanReadSubItemOfOthers = true,
                    CanReadSubItemOfCrew = true,
                    CanReadSubItemOfSelf = true
                }
            };

            return result;
        }
    }
}
