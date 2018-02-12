using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class InsertResult
    {
        public bool IsSuccess { get; set; }
        public object NewId { get; set; }
    }
}
