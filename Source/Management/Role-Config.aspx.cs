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

public partial class Role_Config : System.Web.UI.Page
{
    protected RoleCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new RoleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfSubPages();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Authenticate
            if (c.qsAct == ConfigFormAction.edit && !empAuth.CanEditThisPage()
                || c.qsAct == ConfigFormAction.add && !empAuth.CanAddSubItemInThisPage())
            {
                string jsClose = "closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "invalid", jsClose, true);
                return;
            }

            LoadUIData();
            DisplayRoleData();
            txtRoleName.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        rfvSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_IntegerOnly;
        rfvRoleName.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        revRoleName.ErrorMessage = "*" + Resources.Lang.ErrMsg_RoleNameLimit;
        ltrRoleNameComment.Text = "(" + Resources.Lang.Role_RoleNameComment + ")";
        rfvRoleDisplayName.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;

        LoadCopyPrivilegeFromUIData();
    }

    private void LoadCopyPrivilegeFromUIData()
    {
        ddlCopyPrivilegeFrom.Items.Clear();
        List<EmployeeRoleToSelect> roles = empAuth.GetEmployeeRoleListToSelect();

        if (roles != null)
        {
            // remove admin
            EmployeeRoleToSelect adminData = roles.Find(r => r.RoleName == "admin");

            if(adminData != null)
            {
                roles.Remove(adminData);
            }
            
            ddlCopyPrivilegeFrom.DataTextField = "DisplayText";
            ddlCopyPrivilegeFrom.DataValueField = "RoleName";
            ddlCopyPrivilegeFrom.DataSource = roles;
            ddlCopyPrivilegeFrom.DataBind();
        }

        ddlCopyPrivilegeFrom.Items.Insert(0, new ListItem(Resources.Lang.CopyOption_DontCopy, ""));
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = Resources.Lang.Role_Title_AddNew;
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format(Resources.Lang.Role_Title_Edit_Format, c.qsRoleId);
    }

    private void DisplayRoleData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            EmployeeRoleForBackend role = empAuth.GetEmployeeRoleData(c.qsRoleId);

            if (role != null)
            {
                txtSortNo.Text = role.SortNo.ToString();
                txtRoleName.Text = role.RoleName;
                txtRoleName.Enabled = false;
                txtRoleDisplayName.Text = role.RoleDisplayName;
                CopyPrivilegeArea.Visible = false;

                //modification info
                ltrPostAccount.Text = role.PostAccount;
                ltrPostDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", role.PostDate);

                if (role.MdfDate.HasValue)
                {
                    ltrMdfAccount.Text = role.MdfAccount;
                    ltrMdfDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", role.MdfDate);
                }

                btnSave.Visible = true;
            }
        }
        else if (c.qsAct == ConfigFormAction.add)
        {
            int newSortNo = empAuth.GetEmployeeRoleMaxSortNo() + 10;
            txtSortNo.Text = newSortNo.ToString();

            btnSave.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;

        try
        {
            txtRoleName.Text = txtRoleName.Text.Trim();
            txtRoleDisplayName.Text = txtRoleDisplayName.Text.Trim();

            RoleParams param = new RoleParams()
            {
                RoleName = txtRoleName.Text,
                RoleDisplayName = txtRoleDisplayName.Text,
                SortNo = Convert.ToInt32(txtSortNo.Text.Trim()),
                PostAccount = c.GetEmpAccount()
            };

            bool result = false;

            if (c.qsAct == ConfigFormAction.add)
            {
                if (ddlCopyPrivilegeFrom.SelectedIndex > 0)
                    param.CopyPrivilegeFromRoleName = ddlCopyPrivilegeFrom.SelectedValue;

                result = empAuth.InsertEmployeeRoleData(param);

                if (!result)
                {
                    if (param.HasRoleBeenUsed)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_RoleNameHasBeenUsed);
                    }
                    else
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddFailed);
                    }
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.RoleId = c.qsRoleId;
                result = empAuth.UpdateEmployeeRoleData(param);

                if (!result)
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_UpdateFailed);
                }
            }

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Config"), true);
            }

            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = c.GetEmpAccount(),
                Description = string.Format("．{0}　．儲存身分/Save role[{1}]　RoleId[{2}]　結果/result[{3}]", Title, txtRoleName.Text, param.RoleId, result),
                IP = c.GetClientIP()
            });
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);
            Master.ShowErrorMsg(ex.Message);
        }
    }
}