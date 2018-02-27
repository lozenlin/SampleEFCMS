using Common.DataAccess.EF.Model;
using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_wucBreadcrumb : System.Web.UI.UserControl
{
    protected FrontendPageCommon c;
    protected ArticlePublisherLogic artPub;
    protected FrontendBasePage basePage;
    protected ArticleData articleData;

    #region Public properties and methods

    public bool ShowCurrentNode
    {
        get { return showCurrentNode; }
        set { showCurrentNode = value; }
    }
    private bool showCurrentNode = true;

    public string CustomCurrentNodeText
    {
        get { return customCurrentNodeText; }
        set { customCurrentNodeText = value; }
    }
    private string customCurrentNodeText = "";

    public string CustomRouteHtml
    {
        get { return customRouteHtml; }
        set { customRouteHtml = value; }
    }
    private string customRouteHtml = "";

    public string GetBreadcrumbTextItemHtml(string subject)
    {
        return string.Format("<li class='active'>{0}</li>", subject);
    }

    public string GetBreadcrumbLinkItemHtml(string subject, string title, string href)
    {
        return string.Format("<li><a href='{0}' title='{1}'>{2}</a></li>", href, title, subject);
    }

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new FrontendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(null);
        basePage = (FrontendBasePage)this.Page;
        articleData = basePage.GetArticleData();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DisplayBreadcrumb();
        }
    }

    private void DisplayBreadcrumb()
    {
        if (!articleData.ArticleId.HasValue)
            return;

        // add home node
        ltrBreadcrumb.Text += GetBreadcrumbLinkItemHtml(Resources.Lang.btnHome, Resources.Lang.btnHome_Hint, "Index.aspx?l=" + c.qsLangNo.ToString());

        if (!string.IsNullOrEmpty(customRouteHtml))
        {
            ltrBreadcrumb.Text += customRouteHtml;
        }
        else
        {
            Guid articleId = articleData.ArticleId.Value;
            List<ArticleMultiLangLevelInfo> levelInfos = artPub.GetArticleMultiLangLevelInfo(articleId, c.qsCultureNameOfLangNo);

            if (levelInfos != null)
            {
                int total = levelInfos.Count;

                for (int i = total - 1; i >= 0; i--)
                {
                    ArticleMultiLangLevelInfo levelInfo = levelInfos[i];

                    Guid itemId = levelInfo.ArticleId;

                    if (itemId == Guid.Empty)
                    {
                        continue;
                    }

                    string itemSubject = levelInfo.ArticleSubject;
                    bool isHideSelf = levelInfo.IsHideSelf;
                    bool isShowInLang = levelInfo.IsShowInLang;
                    DateTime startDate = levelInfo.StartDate.Value;
                    DateTime endDate = levelInfo.EndDate.Value;
                    int showTypeId = levelInfo.ShowTypeId.Value;
                    string linkUrl = levelInfo.LinkUrl;

                    if (startDate <= DateTime.Today && DateTime.Today <= endDate
                        && !isHideSelf
                        && isShowInLang)
                    {
                        if (i == 0)
                        {
                            if (showCurrentNode)
                            {
                                if (!string.IsNullOrEmpty(customCurrentNodeText))
                                {
                                    ltrBreadcrumb.Text += GetBreadcrumbTextItemHtml(customCurrentNodeText);
                                }
                                else
                                {
                                    ltrBreadcrumb.Text += GetBreadcrumbTextItemHtml(itemSubject);
                                }
                            }
                        }
                        else
                        {
                            string href = StringUtility.GetLinkUrlOfShowType(itemId, c.qsLangNo, showTypeId, linkUrl);
                            ltrBreadcrumb.Text += GetBreadcrumbLinkItemHtml(itemSubject, itemSubject, href);
                        }
                    }
                }
            }
        }
    }
}