using Common.DataAccess.EF.Model;
using Common.DataAccess.EF.QueryParam;
using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Article : FrontendBasePage
{
    protected FrontendPageCommon c;
    protected ArticlePublisherLogic artPub;
    protected IMasterArticleSettings masterSettings;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new FrontendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        if (!c.RetrieveArticleIdAndData())
        {
            Response.Redirect(c.ERROR_PAGE);
        }

        articleData = c.GetArticleData();
        artPub = new ArticlePublisherLogic(null);
        masterSettings = (IMasterArticleSettings)this.Master;
        masterSettings.ShowReturnToListButton = true;

        if (articleData.ParentId == Guid.Empty)
        {
            masterSettings.SetReturnToListUrl(string.Format("Index.aspx?l={0}", c.qsLangNo));
        }
        else
        {
            masterSettings.SetReturnToListUrl(string.Format("Article.aspx?artid={0}&l={1}", articleData.ParentId, c.qsLangNo));
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        ucDataPager.MaxItemCountOfPage = 10;
        ucDataPager.MaxDisplayCountInPageCodeArea = 5;
        ucDataPager.LinkUrlToReload = string.Concat(Request.AppRelativeCurrentExecutionFilePath, "?", Request.QueryString);
        ucDataPager.Initialize(0, 0);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HandleShowType();
        }
    }

    private void HandleShowType()
    {
        switch (articleData.ShowTypeId)
        {
            case 1:
                // page
                DisplayArticle();
                break;
            case 2:
                // to sub-page
                RedirectToSubPage();
                break;
            case 3:
                // URL
                if (articleData.LinkUrl == "")
                {
                    DisplayArticle();
                }
                else
                {
                    string url = c.AppendCurrentQueryString(articleData.LinkUrl);
                    Response.Redirect(url);
                }
                break;
            case 4:
                // use control
                if (articleData.ControlName == "")
                {
                    DisplayArticle();
                }
                else
                {
                    Control ctl = LoadControl("~/LayoutControls/" + articleData.ControlName + ".ascx");
                    ControlArea.Controls.Add(ctl);
                }
                break;
            default:
                c.LoggerOfUI.ErrorFormat("invalid showTypeId:{0}", articleData.ShowTypeId);
                Response.Redirect(c.ERROR_PAGE);
                break;
        }
    }

    private void DisplayArticle()
    {
        DisplaySubitems();

        ltrArticleContext.Text = articleData.ArticleContext;
    }

    private void DisplaySubitems()
    {
        SubitemsArea.Visible = articleData.IsListAreaShowInFrontStage;

        if (!SubitemsArea.Visible)
            return;

        ArticleValidListQueryParams param = new ArticleValidListQueryParams()
        {
            ParentId = articleData.ArticleId.Value,
            CultureName = c.qsCultureNameOfLangNo,
            Kw = ""
        };

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 0,
            EndNum = 0,
            SortField = articleData.SortFieldOfFrontStage,
            IsSortDesc = articleData.IsSortDescOfFrontStage
        };

        // get total of items
        artPub.GetArticleValidListForFrontend(param);

        // update pager and get begin end of item numbers
        int itemTotalCount = param.PagedParams.RowCount;
        ucDataPager.Initialize(itemTotalCount, c.qsPageCode);

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = ucDataPager.BeginItemNumberOfPage,
            EndNum = ucDataPager.EndItemNumberOfPage,
            SortField = articleData.SortFieldOfFrontStage,
            IsSortDesc = articleData.IsSortDescOfFrontStage
        };

        List<ArticleForFEList> subitems = artPub.GetArticleValidListForFrontend(param);

        if (subitems != null)
        {
            rptSubitems.DataSource = subitems;
            rptSubitems.DataBind();
        }
    }

    protected void rptSubitems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ArticleForFEList artData = (ArticleForFEList)e.Item.DataItem;

        Guid articleId = artData.ArticleId;
        string articleSubject = artData.ArticleSubject;
        int showTypeId = artData.ShowTypeId.Value;
        string linkUrl = artData.LinkUrl;
        string linkTarget = artData.LinkTarget;
        string destUrl = StringUtility.GetLinkUrlOfShowType(articleId, c.qsLangNo, showTypeId, linkUrl);

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");
        btnItem.HRef = destUrl;
        btnItem.Title = articleSubject;

        if (!string.IsNullOrEmpty(linkTarget))
        {
            btnItem.Target = linkTarget;
        }
    }

    private void RedirectToSubPage()
    {
        PagedListQueryParams pagedParams = new PagedListQueryParams()
        {
            BeginNum = 1,
            EndNum = 999999999,
            SortField = articleData.SortFieldOfFrontStage,
            IsSortDesc = articleData.IsSortDescOfFrontStage
        };

        List<ArticleForFEList> subitems = artPub.GetArticleValidListForFrontend(new ArticleValidListQueryParams()
        {
            ParentId = articleData.ArticleId.Value,
            CultureName = c.qsCultureNameOfLangNo,
            Kw = "",
            PagedParams = pagedParams
        });

        if (subitems != null && subitems.Count > 0)
        {
            ArticleForFEList artData = subitems[0];

            Guid articleId = artData.ArticleId;
            int showTypeId = artData.ShowTypeId.Value;
            string linkUrl = artData.LinkUrl;
            string destUrl = StringUtility.GetLinkUrlOfShowType(articleId, c.qsLangNo, showTypeId, linkUrl);

            Response.Redirect(destUrl);
        }
        else
        {
            c.LoggerOfUI.InfoFormat("there is no sub-items of article(id:[{0}])", articleData.ArticleId.Value);
            Response.Redirect(c.ERROR_PAGE);
        }
    }
}