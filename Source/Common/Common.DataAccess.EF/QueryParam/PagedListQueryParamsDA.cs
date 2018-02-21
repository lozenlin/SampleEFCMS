using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class PagedListQueryParamsDA
    {
        public int BeginNum;
        public int EndNum;
        public string SortField = "";
        public bool IsSortDesc = false;
        public int RowCount;

        public int GetSkipCount()
        {
            int skipCount = BeginNum - 1;

            if (skipCount < 0)
                skipCount = 0;

            return skipCount;
        }

        public int GetTakeCount()
        {
            int takeCount = EndNum - BeginNum + 1;

            if (takeCount < 0 || BeginNum <= 0)
                takeCount = 0;

            return takeCount;
        }
    }
}
