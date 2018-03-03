using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public class ArticleForFEListWithThumb : ArticleForFEList
    {
        public string PicId { get; set; }
        public string PicSubject { get; set; }

        public ArticleForFEListWithThumb()
        {
        }

        public ArticleForFEListWithThumb(ArticleForFEList data)
        {
            ArticleId = data.ArticleId;
            ArticleSubject = data.ArticleSubject;
            PublisherName = data.PublisherName;
            TextContext = data.TextContext;
            ArticleAlias = data.ArticleAlias;
            ShowTypeId = data.ShowTypeId;
            LinkUrl = data.LinkUrl;
            LinkTarget = data.LinkTarget;
            StartDate = data.StartDate;
            SortNo = data.SortNo;
            PostDate = data.PostDate;
            MdfDate = data.MdfDate;
            PublishDate = data.PublishDate;
            RowNum = data.RowNum;
        }
    }
}
