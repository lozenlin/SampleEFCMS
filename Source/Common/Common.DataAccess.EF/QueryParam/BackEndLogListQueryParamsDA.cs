using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class BackEndLogListQueryParamsDA
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string Account;  // 空字串:全部
        public bool IsAccKw;
        public string IP;   // 空字串:全部
        public bool IsIpHeadKw;
        public string DescKw;   // 空字串:全部
        public int RangeMode;   // 0:全部, 1:登入相關
        public PagedListQueryParamsDA PagedParams;
        public AuthenticationQueryParamsDA AuthParams;

        public BackEndLogListQueryParamsDA()
        {
            PagedParams = new PagedListQueryParamsDA();
            AuthParams = new AuthenticationQueryParamsDA();
        }
    }
}
