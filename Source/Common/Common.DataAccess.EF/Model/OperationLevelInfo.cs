using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class OperationLevelInfo
    {
        public int OpId { get; set; }
        public string OpSubject { get; set; }
        public string IconImageFile { get; set; }
        public string EnglishSubject { get; set; }

        public int LevelNum { get; set; }
    }
}
