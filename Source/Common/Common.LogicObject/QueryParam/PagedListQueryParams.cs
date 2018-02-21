using Common.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class PagedListQueryParams
    {
        public int BeginNum;
        public int EndNum;
        public string SortField = "";
        public bool IsSortDesc = false;
        public int RowCount;

        public PagedListQueryParamsDA GenPagedListQueryParamsDA()
        {
            PagedListQueryParamsDA result = new PagedListQueryParamsDA()
            {
                BeginNum = BeginNum,
                EndNum = EndNum,
                SortField = SortField,
                IsSortDesc = IsSortDesc,
                RowCount = RowCount
            };

            return result;
        }
    }
}
