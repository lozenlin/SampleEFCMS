using Common.DataAccess.EF.Model;
using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Operation_Node : BasePage
{
    protected OperationCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    private bool useEnglishSubject = false;
    private int levelNumOfThis = 0;
    private int maxLevelNum = 2;
    private int totalSubitems = 0;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new OperationCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        c.SelectMenuItem(c.qsId.ToString(), "");

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfTopPage();

        hud = Master.GetHeadUpDisplay();
        isBackendPage = true;

        if (c.seCultureNameOfBackend == "en")
        {
            useEnglishSubject = true;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RebuildBreadcrumbAndHeadOfHUD();
        Title = hud.GetHeadText() + " - " + Title;

        if (!IsPostBack)
        {
            if (!(c.IsInRole("admin") || c.IsInRole("guest")))
            {
                Response.Redirect(c.BACK_END_HOME);
            }

            LoadUIData();
            DisplayOperation();
        }
        else
        {
            //PostBack
            if (Master.FlagValue != "")
            {
                // message from config-form

                if (Master.FlagValue == "Config")
                {
                    DisplayOperation();
                    Master.RefreshOpMenu();
                }

                Master.FlagValue = "";
            }
        }
    }

    private void RebuildBreadcrumbAndHeadOfHUD()
    {
        string pageName = Resources.Lang.PageName_Operations;
        string pageUrl = "Operation-Node.aspx";

        if (c.qsId == 0)
        {
            //root
            hud.RebuildBreadcrumb(pageName, false);
            hud.SetHeadText(pageName);
        }
        else
        {
            StringBuilder sbBreadcrumbWoHome = new StringBuilder(100);

            // add root link
            sbBreadcrumbWoHome.Append(hud.GetBreadcrumbLinkItemHtml(pageName, pageName, pageUrl));
            // set url of BackToParent button
            hud.SetButtonAttribute(HudButtonNameEnum.BackToParent, HudButtonAttributeEnum.NavigateUrl, "~/" + pageUrl);

            List<OperationLevelInfo> levelInfos = empAuth.GetOperationLevelInfo(c.qsId);

            if (levelInfos != null)
            {
                int total = levelInfos.Count;

                for (int itemNum = total; itemNum >= 1; itemNum--)
                {

                    OperationLevelInfo opData = levelInfos[itemNum - 1];
                    string opSubject = opData.OpSubject;
                    string englishSubject = opData.EnglishSubject;
                    int opId = opData.OpId;
                    string url = string.Format("{0}?id={1}", pageUrl, opId);
                    int levelNum = opData.LevelNum;
                    string iconImageFile = opData.IconImageFile;

                    if (useEnglishSubject && !string.IsNullOrEmpty(englishSubject))
                    {
                        opSubject = englishSubject;
                    }

                    if (itemNum == 1)
                    {
                        levelNumOfThis = levelNum;
                        sbBreadcrumbWoHome.Append(hud.GetBreadcrumbTextItemHtml(opSubject));
                        // update head of HUD
                        hud.SetHeadText(opSubject);

                        if (!string.IsNullOrEmpty(iconImageFile))
                        {
                            iconImageFile = "~/BPImages/icon/" + iconImageFile;
                            hud.SetHeadIconImageUrl(iconImageFile);
                        }
                    }
                    else
                    {
                        sbBreadcrumbWoHome.Append(hud.GetBreadcrumbLinkItemHtml(opSubject, opSubject, url));

                        if (itemNum == 2)
                        {
                            // set url of BackToParent button
                            hud.SetButtonAttribute(HudButtonNameEnum.BackToParent, HudButtonAttributeEnum.NavigateUrl, "~/" + url);
                        }
                    }
                }
            }

            hud.RebuildBreadcrumb(sbBreadcrumbWoHome.ToString(), true);
        }
    }

    private void LoadUIData()
    {
        //HUD
        if (c.IsInRole("admin"))
        {
            if (levelNumOfThis < maxLevelNum)
            {
                hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
                hud.SetButtonAttribute(HudButtonNameEnum.AddNew, HudButtonAttributeEnum.JsInNavigateUrl,
                    string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600);", ConfigFormAction.add, c.qsId));
            }

            if (c.qsId > 0)
            {
                hud.SetButtonVisible(HudButtonNameEnum.Edit, true);
                hud.SetButtonAttribute(HudButtonNameEnum.Edit, HudButtonAttributeEnum.JsInNavigateUrl,
                    string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600);", ConfigFormAction.edit, c.qsId));
            }
        }

        //conditions UI

        //condition vlaues

        //columns of list
        btnSortSubject.Text = Resources.Lang.Col_Subject;
        hidSortSubject.Text = btnSortSubject.Text;
        btnSortIsNewWindow.Text = Resources.Lang.Col_OpenInNewWindow;
        hidSortIsNewWindow.Text = btnSortIsNewWindow.Text;
        btnSortCommonClass.Text = Resources.Lang.Col_CommonClass;
        hidSortCommonClass.Text = btnSortCommonClass.Text;
        btnSortSortNo.Text = Resources.Lang.Col_SortNo;
        hidSortSortNo.Text = btnSortSortNo.Text;

        c.DisplySortableCols(new string[] { 
            "Subject", "IsNewWindow", "CommonClass", 
            "SortNo"
        });
    }

    private void DisplayOperation()
    {
        DisplaySubitems();
        DisplayProperties();
    }

    private void DisplaySubitems()
    {
        if (levelNumOfThis >= maxLevelNum)
        {
            return;
        }

        SubitemArea.Visible = true;

        OpListQueryParams param = new OpListQueryParams()
        {
            ParentId = c.qsId,
            CultureName = c.seCultureNameOfBackend,
            Kw = ""
        };

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 1,
            EndNum = 999999999,
            SortField = c.qsSortField,
            IsSortDesc = c.qsIsSortDesc
        };

        List<OperationForBackend> subitems = empAuth.GetOperationList(param);

        if (subitems != null)
        {
            totalSubitems = subitems.Count;
            rptSubitems.DataSource = subitems;
            rptSubitems.DataBind();
        }
    }

    protected void rptSubitems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        OperationForBackend opData = (OperationForBackend)e.Item.DataItem;

        int opId = opData.OpId;
        string subject = opData.Subject;
        bool isNewWindow = opData.IsNewWindow;
        string iconImageFile = opData.IconImageFile;
        bool isHideSelf = opData.IsHideSelf;
        string commonClass = opData.CommonClass;

        HtmlTableRow ItemArea = (HtmlTableRow)e.Item.FindControl("ItemArea");

        if (isHideSelf)
        {
            ItemArea.Attributes["class"] = "table-danger";
        }

        LinkButton btnMoveDown = (LinkButton)e.Item.FindControl("btnMoveDown");
        btnMoveDown.ToolTip = Resources.Lang.btnMoveDown;

        LinkButton btnMoveUp = (LinkButton)e.Item.FindControl("btnMoveUp");
        btnMoveUp.ToolTip = Resources.Lang.btnMoveUp;

        int itemNum = e.Item.ItemIndex + 1;

        if (itemNum == 1)
        {
            btnMoveUp.Visible = false;
        }

        if (itemNum == totalSubitems)
        {
            btnMoveDown.Visible = false;
        }

        if (c.qsSortField != "")
        {
            btnMoveUp.Visible = false;
            btnMoveDown.Visible = false;
        }

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");

        btnItem.Title = subject;
        btnItem.HRef = string.Format("Operation-Node.aspx?id={0}", opId);

        HtmlImage imgItem = (HtmlImage)e.Item.FindControl("imgItem");

        if (iconImageFile != "")
        {
            imgItem.Src = "BPimages/icon/" + iconImageFile;
        }

        Literal ltrSubject = (Literal)e.Item.FindControl("ltrSubject");
        ltrSubject.Text = subject;

        Literal ltrIsNewWindow = (Literal)e.Item.FindControl("ltrIsNewWindow");
        ltrIsNewWindow.Text = isNewWindow ? Resources.Lang.IsNewWindow_Yes : Resources.Lang.IsNewWindow_No;

        Literal ltrCommonClass = (Literal)e.Item.FindControl("ltrCommonClass");
        string commonClassBadge = string.Format("<span class='badge badge-secondary' title='{0}'>{1}</span>", commonClass, Resources.Lang.CommonClass_HasValue);
        ltrCommonClass.Text = string.IsNullOrEmpty(commonClass) ? "" : commonClassBadge;

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600); return false;", ConfigFormAction.edit, opId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", opId.ToString(), subject);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + Resources.Lang.Operation_ConfirmDelete_Format + "');",
            subject);

        if (!c.IsInRole("admin"))
        {
            btnMoveDown.Visible = false;
            btnMoveUp.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
        }
    }

    protected void rptSubitems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        bool result = false;
        int opId = 0;

        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                opId = Convert.ToInt32(args[0]);
                string subject = args[1];
                OpParams param = new OpParams() { OpId = opId };

                result = empAuth.DeleteOperationData(param);

                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = c.GetEmpAccount(),
                    Description = string.Format("．刪除作業選項/Delete operation　．代碼/id[{0}]　標題/subject[{1}]　結果/result[{2}]", opId, subject, result),
                    IP = c.GetClientIP()
                });

                // log to file
                c.LoggerOfUI.InfoFormat("{0} deletes {1}, result: {2}", c.GetEmpAccount(), "op-" + opId.ToString() + "-" + subject, result);

                if (!result)
                {
                    if (param.IsThereSubitemOfOp)
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_ThereIsSubitemofOp);
                    else
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeleteOperationFailed);
                }

                break;
            case "MoveUp":
                opId = Convert.ToInt32(e.CommandArgument);
                result = empAuth.DecreaseOperationSortNo(opId, c.GetEmpAccount());
                break;
            case "MoveDown":
                opId = Convert.ToInt32(e.CommandArgument);
                result = empAuth.IncreaseOperationSortNo(opId, c.GetEmpAccount());
                break;
        }

        if (result)
        {
            DisplaySubitems();
            Master.RefreshOpMenu();
        }
    }

    private void DisplayProperties()
    {
        if (c.qsId == 0)
        {
            return;
        }

        PropertyArea.Visible = true;

        if (levelNumOfThis >= maxLevelNum)
        {
            PropertyDivider.Visible = false;
        }

        OperationForBackend op = empAuth.GetOperationData(c.qsId);

        if (op != null)
        {
            string iconImageFile = op.IconImageFile;

            if (iconImageFile != "")
            {
                imgIcon.Src = "BPimages/icon/" + iconImageFile;
            }

            ltrLinkUrl.Text = op.LinkUrl;

            bool isNewWindow = op.IsNewWindow;
            ltrIsNewWindow.Text = isNewWindow ? Resources.Lang.IsNewWindow_Yes : Resources.Lang.IsNewWindow_No;

            bool isHideSelf = op.IsHideSelf;
            ltrIsHideSelf.Text = isHideSelf ? Resources.Lang.IsHideSelf_Hide : Resources.Lang.IsHideSelf_Show;

            ltrCommonClass.Text = op.CommonClass;

            string mdfAccount = op.MdfAccount;
            DateTime mdfDate;

            if (!op.MdfDate.HasValue)
            {
                mdfAccount = op.PostAccount;
                mdfDate = op.PostDate.Value;
            }
            else
            {
                mdfDate = op.MdfDate.Value;
            }

            ltrMdfAccount.Text = mdfAccount;
            ltrMdfDate.Text = mdfDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        LinkButton btnSort = (LinkButton)sender;
        string sortField = btnSort.CommandArgument;
        bool isSortDesc = false;
        c.ChangeSortStateToNext(ref sortField, out isSortDesc);

        //重新載入頁面
        Response.Redirect(c.BuildUrlOfListPage(c.qsId, sortField, isSortDesc));
    }
}