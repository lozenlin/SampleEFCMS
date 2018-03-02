using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticlePictureForFrontend
    {
        public System.Guid PicId { get; set; }
        public string PicSubject { get; set; }

        public string FileSavedName { get; set; }
        public Nullable<int> SortNo { get; set; }
    }
}
