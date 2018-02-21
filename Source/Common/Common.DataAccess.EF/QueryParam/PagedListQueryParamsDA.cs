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
    }
}
