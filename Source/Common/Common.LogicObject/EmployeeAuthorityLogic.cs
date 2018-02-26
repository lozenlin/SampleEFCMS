// ===============================================================================
// EmployeeAuthorityLogic of SampleEFCMS
// https://github.com/lozenlin/SampleEFCMS
//
// EmployeeAuthorityLogic.cs
//
// ===============================================================================
// Copyright (c) 2018 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using Common.DataAccess;
using Common.DataAccess.EmployeeAuthority;
using Common.DataAccess.EF;
using Common.DataAccess.EF.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.DataAccess.EF.EntityRequiredPropValues;

namespace Common.LogicObject
{
    /// <summary>
    /// 帳號與權限
    /// </summary>
    public class EmployeeAuthorityLogic
    {
        /// <summary>
        /// 目標資料的擁有者帳號
        /// </summary>
        public string OwnerAccountOfDataExamined
        {
            get { return ownerAccountOfDataExamined; }
            set { ownerAccountOfDataExamined = value; }
        }
        protected string ownerAccountOfDataExamined = "";

        /// <summary>
        /// 目標資料的擁有者部門
        /// </summary>
        public int OwnerDeptIdOfDataExamined
        {
            get { return ownerDeptIdOfDataExamined; }
            set { ownerDeptIdOfDataExamined = value; }
        }
        protected int ownerDeptIdOfDataExamined = 0;

        protected IAuthenticationConditionProvider authCondition;
        protected ICustomEmployeeAuthorizationResult custEmpAuthResult;
        protected EmployeeAuthorizations authorizations = null;
        protected ILog logger = null;
        /// <summary>
        /// 為作業項目中的最上層頁面
        /// </summary>
        protected bool isTopPageOfOperation = true;
        protected int opIdOfPage = 0;
        protected string empAccount = "";
        protected string roleName = "";
        protected bool isRoleAdmin = false;
        protected int deptId = 0;
        protected string dbErrMsg = "";
        
        /// <summary>
        /// 帳號與權限
        /// </summary>
        public EmployeeAuthorityLogic()
        {
            this.authorizations = new EmployeeAuthorizations();
            logger = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// 帳號與權限
        /// </summary>
        public EmployeeAuthorityLogic(IAuthenticationConditionProvider authCondition)
            : this()
        {
            this.authCondition = authCondition;

            if (authCondition != null)
            {
                opIdOfPage = authCondition.GetOpIdOfPage();
                empAccount = authCondition.GetEmpAccount();
                roleName = authCondition.GetRoleName();
                isRoleAdmin = authCondition.IsInRole("admin");
                deptId = authCondition.GetDeptId();

                if (authCondition is ICustomEmployeeAuthorizationResult)
                {
                    custEmpAuthResult = (ICustomEmployeeAuthorizationResult)authCondition;
                }
            }
        }

        public void SetCustomEmployeeAuthorizationResult(ICustomEmployeeAuthorizationResult custEmpAuthResult)
        {
            this.custEmpAuthResult = custEmpAuthResult;
        }

        /// <summary>
        /// 初始化最上層頁面授權結果
        /// </summary>
        public void InitialAuthorizationResultOfTopPage()
        {
            InitialAuthorizationResult(true);
        }

        /// <summary>
        /// 初始化下層頁面授權結果
        /// </summary>
        public void InitialAuthorizationResultOfSubPages()
        {
            InitialAuthorizationResult(false);
        }

        /// <summary>
        /// 初始化授權結果
        /// </summary>
        protected virtual void InitialAuthorizationResult(bool isTopPageOfOperation)
        {
            this.isTopPageOfOperation = isTopPageOfOperation;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                //取得指定作業代碼的後端身分可使用權限
                EmployeeRoleOperationsDesc roleOp = empAuthDao.GetEmployeeRoleOperationsDescDataOfOp(roleName, opIdOfPage);

                //從資料集載入身分的授權設定
                LoadRoleAuthorizationsFrom(roleOp);
            }

            if (custEmpAuthResult != null)
            {
                //自訂帳號授權結果
                EmployeeAuthorizationsWithOwnerInfoOfDataExamined authAndOwner = custEmpAuthResult.InitialAuthorizationResult(isTopPageOfOperation, authorizations);
                ownerAccountOfDataExamined = authAndOwner.OwnerAccountOfDataExamined;
                ownerDeptIdOfDataExamined = authAndOwner.OwnerDeptIdOfDataExamined;
                this.authorizations = authAndOwner;

                if (authAndOwner.IsTopPageOfOperationChanged)
                {
                    this.isTopPageOfOperation = authAndOwner.IsTopPageOfOperation;
                }

                return;
            }
        }

        /// <summary>
        /// 從資料集載入身分的授權設定
        /// </summary>
        public bool LoadRoleAuthorizationsFrom(EmployeeRoleOperationsDesc roleOp)
        {
            if (isRoleAdmin)
            {
                // admin, open all
                authorizations.CanRead = true;
                authorizations.CanEdit = true;

                authorizations.CanReadSubItemOfSelf = true;
                authorizations.CanEditSubItemOfSelf = true;
                authorizations.CanAddSubItemOfSelf = true;
                authorizations.CanDelSubItemOfSelf = true;

                authorizations.CanReadSubItemOfCrew = true;
                authorizations.CanEditSubItemOfCrew = true;
                authorizations.CanDelSubItemOfCrew = true;

                authorizations.CanReadSubItemOfOthers = true;
                authorizations.CanEditSubItemOfOthers = true;
                authorizations.CanDelSubItemOfOthers = true;
            }
            else
            {
                if (roleOp == null)
                {
                    logger.Info("parameter of LoadRoleAuthorizationsFrom() is empty.");
                    return false;
                }

                // load settings
                authorizations.ImportDataFrom(roleOp);
            }

            return true;
        }

        /// <summary>
        /// 檢查是否可開啟此頁面
        /// </summary>
        public virtual bool CanOpenThisPage()
        {
            bool result = false;

            if (isTopPageOfOperation)
            {
                result = authorizations.CanRead;
            }
            else
            {
                if (authorizations.CanReadSubItemOfOthers)
                {
                    result = true;
                }
                else if (authorizations.CanReadSubItemOfCrew
                    && deptId > 0
                    && deptId == ownerDeptIdOfDataExamined)
                {
                    result = true;
                }
                else if (authorizations.CanReadSubItemOfSelf 
                    && empAccount != "" 
                    && empAccount == ownerAccountOfDataExamined)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 檢查是否可編輯此頁面
        /// </summary>
        public virtual bool CanEditThisPage()
        {
            return CanEditThisPage(isTopPageOfOperation, ownerAccountOfDataExamined, ownerDeptIdOfDataExamined);
        }

        /// <summary>
        /// 檢查是否可編輯此頁面
        /// </summary>
        public virtual bool CanEditThisPage(bool useTopRule, string ownerAccount, int ownerDeptId)
        {
            bool result = false;

            if (useTopRule)
            {
                result = authorizations.CanEdit;
            }
            else
            {
                if (authorizations.CanEditSubItemOfOthers)
                {
                    result = true;
                }
                else if (authorizations.CanEditSubItemOfCrew
                    && deptId > 0
                    && deptId == ownerDeptId)
                {
                    result = true;
                }
                else if (authorizations.CanEditSubItemOfSelf
                    && empAccount != ""
                    && empAccount == ownerAccount)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 檢查是否可刪除此頁面
        /// </summary>
        public virtual bool CanDelThisPage(string ownerAccount, int ownerDeptId)
        {
            bool result = false;

            if (authorizations.CanDelSubItemOfOthers)
            {
                result = true;
            }
            else if (authorizations.CanDelSubItemOfCrew
                && deptId > 0
                && deptId == ownerDeptId)
            {
                result = true;
            }
            else if (authorizations.CanDelSubItemOfSelf
                && empAccount != ""
                && empAccount == ownerAccount)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 檢查是否可在此頁面新增子項目
        /// </summary>
        public virtual bool CanAddSubItemInThisPage()
        {
            return authorizations.CanAddSubItemOfSelf;
        }

        /// <summary>
        /// 可閱讀任何人的子項目
        /// </summary>
        public bool CanReadSubItemOfOthers()
        {
            return authorizations.CanReadSubItemOfOthers;
        }

        /// <summary>
        /// 可修改任何人的子項目
        /// </summary>
        public bool CanEditSubItemOfOthers()
        {
            return authorizations.CanEditSubItemOfOthers;
        }

        /// <summary>
        /// 可閱讀同部門的子項目
        /// </summary>
        public bool CanReadSubItemOfCrew()
        {
            return authorizations.CanReadSubItemOfCrew;
        }

        /// <summary>
        /// 可修改同部門的子項目
        /// </summary>
        public bool CanEditSubItemOfCrew()
        {
            return authorizations.CanEditSubItemOfCrew;
        }

        /// <summary>
        /// 可閱讀自己的子項目
        /// </summary>
        public bool CanReadSubItemOfSelf()
        {
            return authorizations.CanReadSubItemOfSelf;
        }

        /// <summary>
        /// 可修改自己的子項目
        /// </summary>
        public bool CanEditSubItemOfSelf()
        {
            return authorizations.CanEditSubItemOfSelf;
        }

        // DataAccess functions

        /// <summary>
        /// DB command 執行後的錯誤訊息
        /// </summary>
        public string GetDbErrMsg()
        {
            return dbErrMsg;
        }

        #region BackEndLog DataAccess functions

        /// <summary>
        /// 新增後端操作記錄
        /// </summary>
        public bool InsertBackEndLogData(BackEndLogData data)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                insResult = empAuthDao.Insert<BackEndLog>(new BackEndLog()
                {
                    EmpAccount = data.EmpAccount,
                    Description = data.Description,
                    IP = data.IP,
                    OpDate = DateTime.Now
                });

                dbErrMsg = empAuthDao.GetErrMsg();

            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 取得後端操作記錄清單
        /// </summary>
        public List<BackEndLogForBackend> GetBackEndLogList(BackEndLogListQueryParams param)
        {
            List<BackEndLogForBackend> entities = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                BackEndLogListQueryParamsDA paramDA = param.GenBackEndLogListQueryParamsDA();
                entities = empAuthDao.GetBackEndLogList(paramDA);
                dbErrMsg = empAuthDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        #endregion

        #region Employee DataAccess functions

        /// <summary>
        /// 取得員工登入用資料
        /// </summary>
        public EmployeeToLogin GetEmployeeDataToLogin(string empAccount)
        {
            EmployeeToLogin entity = null;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetEmployeeDataToLogin(empAccount);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        public EmployeeForBackend GetEmployeeData(string empAccount)
        {
            EmployeeForBackend entity = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetEmployeeDataForBackend(empAccount);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        public EmployeeForBackend GetEmployeeData(int empId)
        {
            EmployeeForBackend entity = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetEmployeeDataForBackend(empId);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得員工身分名稱
        /// </summary>
        public string GetRoleNameOfEmp(string empAccount)
        {
            string roleName = "";

            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_GetRoleName cmdInfo = new spEmployee_GetRoleName()
            {
                EmpAccount = empAccount
            };
            roleName = cmd.ExecuteScalar<string>(cmdInfo, "-1");
            dbErrMsg = cmd.GetErrMsg();

            if (roleName == "-1")
            {
                roleName = "";
            }

            return roleName;
        }

        /// <summary>
        /// 更新員工本次登入資訊
        /// </summary>
        public bool UpdateEmployeeLoginInfo(string empAccount, string thisLoginIP)
        {
            bool result = false;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                Employee entity = empAuthDao.Get<Employee>(emp => emp.EmpAccount == empAccount);

                //備份上次的登入資訊,記錄這次的
                entity.LastLoginTime = entity.ThisLoginTime;
                entity.LastLoginIP = entity.ThisLoginIP;
                entity.ThisLoginTime = DateTime.Now;
                entity.ThisLoginIP = thisLoginIP;

                result = empAuthDao.Update();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得帳號名單
        /// </summary>
        public List<EmployeeForBackend> GetAccountList(AccountListQueryParams param)
        {
            List<EmployeeForBackend> entities = null;
            AccountListQueryParamsDA paramDA = param.GenAccountListQueryParamsDA();

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entities = empAuthDao.GetEmployeeListForBackend(paramDA);
                dbErrMsg = empAuthDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 刪除員工資料
        /// </summary>
        public bool DeleteEmployeeData(int empId)
        {
            bool result = false;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                Employee entity = new Employee()
                {
                    EmpId = empId
                };

                result = empAuthDao.Delete<Employee>(entity);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 新增員工資料
        /// </summary>
        public bool InsertEmployeeData(AccountParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                insResult = empAuthDao.Insert<Employee>(new Employee()
                {
                    EmpAccount = param.EmpAccount,
                    EmpPassword = param.EmpPassword,
                    EmpName = param.EmpName,
                    Email = param.Email,
                    Remarks = param.Remarks,
                    DeptId = param.DeptId,
                    RoleId = param.RoleId,
                    IsAccessDenied = param.IsAccessDenied,
                    StartDate = param.StartDate,
                    EndDate = param.EndDate,
                    OwnerAccount = param.OwnerAccount,
                    PasswordHashed = param.PasswordHashed,
                    DefaultRandomPassword = param.DefaultRandomPassword,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                });

                dbErrMsg = empAuthDao.GetErrMsg();

                if (insResult.IsSuccess)
                {
                    param.EmpId = (int)insResult.NewId;
                }
                else if (empAuthDao.GetSqlErrNumber() == 2601)
                {
                    param.HasAccountBeenUsed = true;
                }
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 更新員工資料
        /// </summary>
        public bool UpdateEmployeeData(AccountParams param)
        {
            bool result = false;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                Employee entity = empAuthDao.GetEmptyEntity<Employee>(new EmployeeRequiredPropValues()
                {
                    EmpId = param.EmpId,
                    EmpAccount = "",
                    EmpPassword = ""
                });

                entity.EmpPassword = param.EmpPassword;
                entity.EmpName = param.EmpName;
                entity.Email = param.Email;
                entity.Remarks = param.Remarks;
                entity.DeptId = param.DeptId;
                entity.RoleId = param.RoleId;
                entity.IsAccessDenied = param.IsAccessDenied;
                entity.StartDate = param.StartDate;
                entity.EndDate = param.EndDate;
                entity.OwnerAccount = param.OwnerAccount;
                entity.PasswordHashed = param.PasswordHashed;
                entity.DefaultRandomPassword = param.DefaultRandomPassword;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = empAuthDao.Update();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 更新員工密碼
        /// </summary>
        public bool UpdateEmployeePassword(string empAccount, string empPassword)
        {
            bool result = false;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                Employee entity = empAuthDao.Get<Employee>(emp => emp.EmpAccount == empAccount);

                entity.EmpPassword = empPassword;
                entity.PasswordHashed = true;
                entity.MdfAccount = empAccount;
                entity.MdfDate = DateTime.Now;

                result = empAuthDao.Update();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 更新員工的重置密碼用唯一值
        /// </summary>
        public bool UpdateEmployeePasswordResetKey(string empAccount, string passwordResetKey)
        {
            bool result = false;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                Employee entity = empAuthDao.Get<Employee>(emp => emp.EmpAccount == empAccount);

                entity.PasswordResetKey = passwordResetKey;
                entity.PasswordResetKeyDate = DateTime.Now;
                entity.MdfAccount = empAccount;
                entity.MdfDate = DateTime.Now;

                result = empAuthDao.Update();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 以重置密碼用唯一值取得員工登入用資料
        /// </summary>
        public Employee GetEmployeeDataToLoginByPasswordResetKey(string passwordResetKey)
        {
            Employee entity = null;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.Get<Employee>(emp => emp.PasswordResetKey == passwordResetKey);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        #endregion

        #region Operation DataAccess functions

        /// <summary>
        /// 用共用元件類別名稱取得後端作業選項資訊
        /// </summary>
        public OperationOpInfo GetOperationOpInfoByCommonClass(string commonClass)
        {
            OperationOpInfo entity = null;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetOperationOpInfoByCommonClass(commonClass);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 用超連結網址取得後端作業選項資訊
        /// </summary>
        public OperationOpInfo GetOperationOpInfoByLinkUrl(string linkUrl)
        {
            OperationOpInfo entity = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetOperationOpInfoByLinkUrl(linkUrl);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得後端作業選項身分授權的巢狀清單
        /// </summary>
        public List<OperationWithRoleAuth> GetOperationWithRoleAuthNestedList(string roleName)
        {
            List<OperationWithRoleAuth> entities = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                List<Operations> topOps = empAuthDao.GetList<Operations>(op => op.ParentId == null && !op.IsHideSelf)
                    .OrderBy(op => op.SortNo).ToList();
                List<Operations> subOps = empAuthDao.GetList<Operations>(op => op.ParentId != null && !op.IsHideSelf)
                    .OrderBy(op => op.SortNo).ToList();
                List<EmployeeRoleOperationsDesc> roleAuthItems = empAuthDao.GetList<EmployeeRoleOperationsDesc>(ro => ro.RoleName == roleName)
                    .ToList();

                if(topOps != null && subOps != null && roleAuthItems != null)
                {
                    entities = topOps.ConvertAll<OperationWithRoleAuth>(op =>
                    {
                        // top item
                        OperationWithRoleAuth opAuth = new OperationWithRoleAuth();
                        opAuth.ImportDataFrom(op);

                        EmployeeRoleOperationsDesc roleAuthItem = roleAuthItems.Find(ro => ro.OpId == op.OpId);

                        if (roleAuthItem != null)
                        {
                            opAuth.ImportDataFrom(roleAuthItem);
                        }

                        // sub item
                        opAuth.SubItems = subOps.Where(subOp => subOp.ParentId == op.OpId)
                            .Select(subOp =>
                            {
                                OperationWithRoleAuth subOpAuth = new OperationWithRoleAuth();
                                subOpAuth.ImportDataFrom(subOp);

                                EmployeeRoleOperationsDesc subRoleAuthItem = roleAuthItems.Find(ro => ro.OpId == subOp.OpId);

                                if (subRoleAuthItem != null)
                                {
                                    subOpAuth.ImportDataFrom(subRoleAuthItem);
                                }

                                return subOpAuth;
                            }).ToList();

                        return opAuth;
                    });
                }

                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得後端作業選項資料
        /// </summary>
        public OperationForBackend GetOperationData(int opId)
        {
            OperationForBackend entity = null;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetOperationDataForBackend(opId);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得用來組成麵包屑節點連結的後端作業選項資料
        /// </summary>
        public OperationHtmlAnchorData GetOperationHtmlAnchorData(int opId, bool useEnglishSubject)
        {
            OperationHtmlAnchorData result = null;
            OperationForBackend op = GetOperationData(opId);

            if (op != null)
            {
                string opSubject = op.OpSubject;
                string englishSubject = op.EnglishSubject;

                if (useEnglishSubject && !string.IsNullOrEmpty(englishSubject))
                {
                    opSubject = englishSubject;
                }

                result = new OperationHtmlAnchorData();
                result.Subject = opSubject;
                result.LinkUrl = op.LinkUrl;
                result.IconImageFileUrl = op.IconImageFile;
                result.Html = string.Format("<a href=\"{0}\">{1}</a>", result.LinkUrl, result.Subject);
            }

            return result;
        }

        /// <summary>
        /// 取得後端作業選項清單
        /// </summary>
        public DataSet GetOperationList(OpListQueryParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_GetList cmdInfo = new spOperations_GetList()
            {
                ParentId = param.ParentId,
                CultureName = param.CultureName,
                Kw = param.Kw,
                BeginNum = param.PagedParams.BeginNum,
                EndNum = param.PagedParams.EndNum,
                SortField = param.PagedParams.SortField,
                IsSortDesc = param.PagedParams.IsSortDesc,
                CanReadSubItemOfOthers = true,
                CanReadSubItemOfCrew = true,
                CanReadSubItemOfSelf = true,
                MyAccount = "",
                MyDeptId = 0
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();
            param.PagedParams.RowCount = cmdInfo.RowCount;

            return ds;
        }

        /// <summary>
        /// 刪除後端作業選項
        /// </summary>
        public bool DeleteOperationData(OpParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_DeleteData cmdInfo = new spOperations_DeleteData() { OpId = param.OpId };

            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            if (!result && cmd.GetSqlErrNumber() == 50000 && cmd.GetSqlErrState() == 2)
            {
                param.IsThereSubitemOfOp = true;
            }

            return result;
        }

        /// <summary>
        /// 加大後端作業選項的排序編號
        /// </summary>
        public bool IncreaseOperationSortNo(int opId, string mdfAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_IncreaseSortNo cmdInfo = new spOperations_IncreaseSortNo()
            {
                OpId = opId,
                MdfAccount = mdfAccount
            };

            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 減小後端作業選項的排序編號
        /// </summary>
        public bool DecreaseOperationSortNo(int opId, string mdfAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_DecreaseSortNo cmdInfo = new spOperations_DecreaseSortNo()
            {
                OpId = opId,
                MdfAccount = mdfAccount
            };

            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 取得後端作業選項階層資訊
        /// </summary>
        public List<OperationLevelInfo> GetOperationLevelInfo(int opId)
        {
            List<OperationLevelInfo> entities = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entities = empAuthDao.GetOperationLevelInfo(opId);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得後端作業選項最大排序編號
        /// </summary>
        public int GetOperationMaxSortNo(int parentId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_GetMaxSortNo cmdInfo = new spOperations_GetMaxSortNo() { ParentId = parentId };

            int errCode = -1;
            int result = cmd.ExecuteScalar<int>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 新增後端作業選項
        /// </summary>
        public bool InsertOperationData(OpParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_InsertData cmdInfo = new spOperations_InsertData()
            {
                ParentId = param.ParentId,
                OpSubject = param.OpSubject,
                LinkUrl = param.LinkUrl,
                IsNewWindow = param.IsNewWindow,
                IconImageFile = param.IconImageFile,
                SortNo = param.SortNo,
                IsHideSelf = param.IsHideSelf,
                CommonClass = param.CommonClass,
                PostAccount = param.PostAccount,
                EnglishSubject = param.EnglishSubject
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            if (result)
            {
                param.OpId = cmdInfo.OpId;
            }

            return result;
        }

        /// <summary>
        /// 更新後端作業選項
        /// </summary>
        public bool UpdateOperaionData(OpParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_UpdateData cmdInfo = new spOperations_UpdateData()
            {
                OpId = param.OpId,
                OpSubject = param.OpSubject,
                LinkUrl = param.LinkUrl,
                IsNewWindow = param.IsNewWindow,
                IconImageFile = param.IconImageFile,
                SortNo = param.SortNo,
                IsHideSelf = param.IsHideSelf,
                CommonClass = param.CommonClass,
                MdfAccount = param.PostAccount,
                EnglishSubject = param.EnglishSubject
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        #endregion

        #region EmployeeRole DataAccess functions

        /// <summary>
        /// 取得選擇用員工身分清單
        /// </summary>
        public List<EmployeeRoleToSelect> GetEmployeeRoleListToSelect()
        {
            List<EmployeeRoleToSelect> entities = null;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entities = empAuthDao.GetList<EmployeeRole>()
                    .OrderBy(r => r.SortNo)
                    .AsEnumerable()
                    .Select(r => new EmployeeRoleToSelect()
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName,
                        RoleDisplayName = r.RoleDisplayName,
                        DisplayText = string.Format("{0} ({1})", r.RoleDisplayName, r.RoleName)
                    }).ToList();

                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得員工身分清單
        /// </summary>
        public List<EmployeeRoleForBackend> GetEmployeeRoleList(RoleListQueryParams param)
        {
            List<EmployeeRoleForBackend> entities = null;
            RoleListQueryParamsDA paramDA = param.GenRoleListQueryParamsDA();

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entities = empAuthDao.GetEmployeeRoleListForBackend(paramDA);
                dbErrMsg = empAuthDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 刪除員工身分
        /// </summary>
        public bool DeleteEmployeeRoleData(RoleParams param)
        {
            bool result = false;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                result = empAuthDao.DeleteEmployeeRoleData(param.RoleId);
                dbErrMsg = empAuthDao.GetErrMsg();

                if (!result && empAuthDao.GetSqlErrNumber() == 50000 && empAuthDao.GetSqlErrState() == 2)
                {
                    param.IsThereAccountsOfRole = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 取得員工身分最大排序編號
        /// </summary>
        public int GetEmployeeRoleMaxSortNo()
        {
            int result = 0;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                result = empAuthDao.GetEmployeeRoleMaxSortNo();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 取得員工身分資料
        /// </summary>
        public EmployeeRoleForBackend GetEmployeeRoleData(int roleId)
        {
            EmployeeRoleForBackend entity = null;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetEmployeeRoleDataForBackend(roleId);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 新增員工身分資料
        /// </summary>
        public bool InsertEmployeeRoleData(RoleParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                EmployeeRole entity = new EmployeeRole()
                {
                    RoleName = param.RoleName,
                    RoleDisplayName = param.RoleDisplayName,
                    SortNo = param.SortNo,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = empAuthDao.InsertEmployeeRoleData(entity, param.CopyPrivilegeFromRoleName);
                dbErrMsg = empAuthDao.GetErrMsg();

                if (insResult.IsSuccess)
                {
                    param.RoleId = entity.RoleId;
                }
                else if (empAuthDao.GetSqlErrNumber() == 50000 && empAuthDao.GetSqlErrState() == 2)
                {
                    param.HasRoleBeenUsed = true;
                }
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 更新員工身分資料
        /// </summary>
        public bool UpdateEmployeeRoleData(RoleParams param)
        {
            bool result = false;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                EmployeeRole entity = empAuthDao.GetEmptyEntity<EmployeeRole>(new EmployeeRoleRequiredPropValues()
                {
                    RoleId = param.RoleId,
                    RoleName = ""
                });

                entity.RoleDisplayName = param.RoleDisplayName;
                entity.SortNo = param.SortNo;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = empAuthDao.Update();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        #endregion

        #region EmployeeRole-Operations DataAccess functions

        /// <summary>
        /// 儲存員工身分後端作業授權清單
        /// </summary>
        public bool SaveListOfEmployeeRolePrivileges(RolePrivilegeParams param)
        {
            bool result = false;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                List<EmployeeRoleOperationsDesc> empRoleOps = param.GenEmpRoleOps();
                result = empAuthDao.SaveListOfEmployeeRolePrivileges(empRoleOps);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        #endregion

        #region Department DataAccess functions

        /// <summary>
        /// 取得選擇用部門清單
        /// </summary>
        public List<Department> GetDepartmentListToSelect()
        {
            List<Department> entities = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entities = empAuthDao.GetList<Department>()
                    .OrderBy(dept => dept.SortNo).ToList();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entities;
        }

        /// <summary>
        /// 取得部門清單
        /// </summary>
        public List<DepartmentForBackend> GetDepartmentList(DeptListQueryParams param)
        {
            List<DepartmentForBackend> entities = null;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                DeptListQueryParamsDA paramDA = param.GenDeptListQueryParamsDA();
                entities = empAuthDao.GetDepartmentListForBackend(paramDA);
                dbErrMsg = empAuthDao.GetErrMsg();
                param.PagedParams.RowCount = paramDA.PagedParams.RowCount;
            }

            return entities;
        }

        /// <summary>
        /// 刪除部門資料
        /// </summary>
        public bool DeleteDepartmentData(DeptParams param)
        {
            bool result = false;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                result = empAuthDao.DeleteDepartmentData(param.DeptId);
                dbErrMsg = empAuthDao.GetErrMsg();

                if (!result && empAuthDao.GetSqlErrNumber() == 50000 && empAuthDao.GetSqlErrState() == 2)
                {
                    param.IsThereAccountsOfDept = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 取得部門資料
        /// </summary>
        public DepartmentForBackend GetDepartmentData(int deptId)
        {
            DepartmentForBackend entity = null;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                entity = empAuthDao.GetDepartmentDataForBackend(deptId);
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return entity;
        }

        /// <summary>
        /// 取得部門最大排序編號
        /// </summary>
        public int GetDepartmentMaxSortNo()
        {
            int result = 0;

            using(EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                result = empAuthDao.GetDepartmentMaxSortNo();
                dbErrMsg = empAuthDao.GetErrMsg();
            }

            return result;
        }

        /// <summary>
        /// 新增部門資料
        /// </summary>
        public bool InsertDepartmentData(DeptParams param)
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                Department entity = new Department()
                {
                    DeptName = param.DeptName,
                    SortNo = param.SortNo,
                    PostAccount = param.PostAccount,
                    PostDate = DateTime.Now
                };

                insResult = empAuthDao.InsertDepartmentData(entity);
                dbErrMsg = empAuthDao.GetErrMsg();

                if (insResult.IsSuccess)
                {
                    param.DeptId = (int)insResult.NewId;
                }
                else if (empAuthDao.GetSqlErrNumber() == 50000 && empAuthDao.GetSqlErrState() == 2)
                {
                    param.HasDeptNameBeenUsed = true;
                }
            }

            return insResult.IsSuccess;
        }

        /// <summary>
        /// 更新部門資料
        /// </summary>
        public bool UpdateDepartmentData(DeptParams param)
        {
            bool result = false;

            using (EmployeeAuthorityDataAccess empAuthDao = new EmployeeAuthorityDataAccess())
            {
                Department entity = empAuthDao.GetEmptyEntity<Department>(new DepartmentRequiredPropValues()
                {
                    DeptId = param.DeptId,
                    DeptName = ""
                });

                entity.DeptName = param.DeptName;
                entity.SortNo = param.SortNo;
                entity.MdfAccount = param.PostAccount;
                entity.MdfDate = DateTime.Now;

                result = empAuthDao.UpdateDepartmentData(entity);
                dbErrMsg = empAuthDao.GetErrMsg();

                if (!result && empAuthDao.GetSqlErrNumber() == 50000 && empAuthDao.GetSqlErrState() == 2)
                {
                    param.HasDeptNameBeenUsed = true;
                }
            }

            return result;
        }

        #endregion
    }
}
