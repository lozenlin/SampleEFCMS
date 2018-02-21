// ===============================================================================
// EmployeeAuthorityDataAccess of SampleEFCMS
// https://github.com/lozenlin/SampleEFCMS
//
// EmployeeAuthorityDataAccess.cs
//
// ===============================================================================
// Copyright (c) 2018 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using Common.DataAccess.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Common.DataAccess.EF
{
    public class EmployeeAuthorityDataAccess : DataAccessBase
    {
        public EmployeeAuthorityDataAccess() : base()
        {
        }

        #region 員工資料

        /// <summary>
        /// 取得員工登入用資料
        /// </summary>
        public EmployeeToLogin GetEmployeeDataToLogin(string empAccount)
        {
            Logger.Debug("GetEmployeeDataToLogin(empAccount)");

            EmployeeToLogin entity = null;

            try
            {
                entity = (from emp in cmsCtx.Employee
                          from role in cmsCtx.EmployeeRole
                          where emp.RoleId == role.RoleId
                             && emp.EmpAccount == empAccount
                          select new EmployeeToLogin()
                          {
                              EmpPassword = emp.EmpPassword,
                              IsAccessDenied = emp.IsAccessDenied,
                              StartDate = emp.StartDate,
                              EndDate = emp.EndDate,
                              PasswordHashed = emp.PasswordHashed,
                              RoleName = role.RoleName
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        /// <summary>
        /// 取得後台用員工資料
        /// </summary>
        public EmployeeForBackend GetEmployeeDataForBackend(string empAccount)
        {
            Logger.Debug("GetEmployeeDataForBackend(empAccount)");

            EmployeeForBackend entity = null;

            try
            {
                Employee employee = cmsCtx.Employee
                    .Include(emp => emp.EmployeeRole)
                    .Include(emp => emp.Department)
                    .Where(emp => emp.EmpAccount == empAccount)
                    .FirstOrDefault();

                entity = new EmployeeForBackend(employee);

                var ownerData = cmsCtx.Employee.Where(emp => emp.EmpAccount == entity.OwnerAccount)
                    .Select(emp => new
                    {
                        DeptId = emp.DeptId ?? 0,
                        OwnerName = emp.EmpName
                    }).FirstOrDefault();

                if (ownerData != null)
                {
                    entity.OwnerDeptId = ownerData.DeptId;
                    entity.OwnerName = ownerData.OwnerName;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        /// <summary>
        /// 取得後台用員工資料
        /// </summary>
        public EmployeeForBackend GetEmployeeDataForBackend(int empId)
        {
            Logger.Debug("GetEmployeeDataForBackend(empId)");

            EmployeeForBackend entity = null;

            try
            {
                Employee employee = cmsCtx.Employee
                    .Include(emp => emp.EmployeeRole)
                    .Include(emp => emp.Department)
                    .Where(emp => emp.EmpId == empId)
                    .FirstOrDefault();

                entity = new EmployeeForBackend(employee);

                var ownerData = cmsCtx.Employee.Where(emp => emp.EmpAccount == entity.OwnerAccount)
                    .Select(emp => new
                    {
                        DeptId = emp.DeptId ?? 0,
                        OwnerName = emp.EmpName
                    }).FirstOrDefault();

                if (ownerData != null)
                {
                    entity.OwnerDeptId = ownerData.DeptId;
                    entity.OwnerName = ownerData.OwnerName;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        /// <summary>
        /// 取得員工代碼的帳號
        /// </summary>
        public string GetEmployeeAccountOfId(int empId)
        {
            Logger.Debug("GetEmployeeAccountOfId(empId)");

            string empAccount = null;

            try
            {
                Employee entity = cmsCtx.Employee.Find(empId);

                if(entity != null)
                {
                    empAccount = entity.EmpAccount;
                }
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return empAccount;
        }

        public List<EmployeeForBackend> GetEmployeeListForBackend(AccountListQueryParamsDA param)
        {
            Logger.Debug("GetEmployeeListForBackend(param)");

            List<EmployeeForBackend> entities = null;

            try
            {
                var tempEntities = from e in cmsCtx.Employee.Include(emp => emp.EmployeeRole).Include(emp => emp.Department)
                                   join oe in cmsCtx.Employee
                                   on e.OwnerAccount equals oe.EmpAccount
                                   into empGroup
                                   from oe in empGroup.DefaultIfEmpty()
                                   select new { e, oe, role = e.EmployeeRole, dept = e.Department };

                // Query conditions

                /*
                and (@CanReadSubItemOfOthers=1
	                or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	                or @CanReadSubItemOfSelf=1 and e.OwnerAccount=@MyAccount
	                or e.EmpAccount=@MyAccount)
                 */
                if (!param.AuthParams.CanReadSubItemOfCrew)
                {
                    tempEntities = tempEntities.Where(obj =>
                        obj.e.DeptId == param.DeptId && param.AuthParams.CanReadSubItemOfCrew
                        || obj.e.OwnerAccount == param.AuthParams.MyAccount && param.AuthParams.CanReadSubItemOfSelf
                        || obj.e.EmpAccount == param.AuthParams.MyAccount);
                }

                if (param.DeptId != 0) // 0:all
                {
                    tempEntities = tempEntities.Where(obj => obj.e.DeptId == param.DeptId);
                }

                if (param.Kw != "")
                {
                    tempEntities = tempEntities.Where(obj =>
                        obj.e.EmpAccount.Contains(param.Kw)
                        || obj.e.EmpName.Contains(param.Kw));
                }

                //清單內容模式(0:all, 1:normal, 2:access is denied)
                switch (param.ListMode)
                {
                    case 1:
                        tempEntities = tempEntities.Where(obj =>
                            !obj.e.IsAccessDenied
                            && (obj.role.RoleName == "admin"
                                || obj.e.StartDate <= DateTime.Now && DateTime.Now < DbFunctions.AddDays(obj.e.EndDate, 1)));
                        break;
                    case 2:
                        tempEntities = tempEntities.Where(obj =>
                            obj.e.IsAccessDenied
                            || !(obj.role.RoleName == "admin"
                                || obj.e.StartDate <= DateTime.Now && DateTime.Now < DbFunctions.AddDays(obj.e.EndDate, 1)));
                        break;
                }

                // sorting
                switch (param.PagedParams.SortField)
                {
                    case "DeptName":
                        if (param.PagedParams.IsSortDesc)
                            tempEntities = tempEntities.OrderByDescending(obj => obj.dept.DeptName);
                        else
                            tempEntities = tempEntities.OrderBy(obj => obj.dept.DeptName);
                        break;
                    case "RoleSortNo":
                        if (param.PagedParams.IsSortDesc)
                            tempEntities = tempEntities.OrderByDescending(obj => obj.role.SortNo);
                        else
                            tempEntities = tempEntities.OrderBy(obj => obj.role.SortNo);
                        break;
                    case "EmpName":
                        if (param.PagedParams.IsSortDesc)
                            tempEntities = tempEntities.OrderByDescending(obj => obj.e.EmpName);
                        else
                            tempEntities = tempEntities.OrderBy(obj => obj.e.EmpName);
                        break;
                    case "EmpAccount":
                        if (param.PagedParams.IsSortDesc)
                            tempEntities = tempEntities.OrderByDescending(obj => obj.e.EmpAccount);
                        else
                            tempEntities = tempEntities.OrderBy(obj => obj.e.EmpAccount);
                        break;
                    case "StartDate":
                        if (param.PagedParams.IsSortDesc)
                            tempEntities = tempEntities.OrderByDescending(obj => obj.e.StartDate);
                        else
                            tempEntities = tempEntities.OrderBy(obj => obj.e.StartDate);
                        break;
                    case "OwnerName":
                        if (param.PagedParams.IsSortDesc)
                            tempEntities = tempEntities.OrderByDescending(obj => obj.oe.EmpName);
                        else
                            tempEntities = tempEntities.OrderBy(obj => obj.oe.EmpName);
                        break;
                    default:
                        tempEntities = tempEntities
                            .OrderBy(obj => obj.e.DeptId)
                            .ThenBy(obj => obj.role.SortNo)
                            .ThenBy(obj => obj.e.EmpName);
                        break;
                }

                // total
                param.PagedParams.RowCount = tempEntities.Count();

                // paging
                int skipCount = param.PagedParams.GetSkipCount();
                int takeCount = param.PagedParams.GetTakeCount();

                if (skipCount > 0)
                {
                    tempEntities = tempEntities.Skip(skipCount);
                }

                if (takeCount >= 0)
                {
                    tempEntities = tempEntities.Take(takeCount);
                }

                // result
                entities = tempEntities.Select(obj => new EmployeeForBackend()
                {
                    EmpId = obj.e.EmpId,
                    EmpAccount = obj.e.EmpAccount,
                    EmpPassword = obj.e.EmpPassword,
                    EmpName = obj.e.EmpName,
                    Email = obj.e.Email,
                    Remarks = obj.e.Remarks,
                    IsAccessDenied = obj.e.IsAccessDenied,
                    PostAccount = obj.e.PostAccount,
                    PostDate = obj.e.PostDate,
                    MdfAccount = obj.e.MdfAccount,
                    MdfDate = obj.e.MdfDate,
                    StartDate = obj.e.StartDate,
                    EndDate = obj.e.EndDate,
                    OwnerAccount = obj.e.OwnerAccount,
                    ThisLoginTime = obj.e.ThisLoginTime,
                    ThisLoginIP = obj.e.ThisLoginIP,
                    LastLoginTime = obj.e.LastLoginTime,
                    LastLoginIP = obj.e.LastLoginIP,
                    PasswordHashed = obj.e.PasswordHashed,
                    DefaultRandomPassword = obj.e.DefaultRandomPassword,
                    DeptId = obj.dept.DeptId,
                    DeptName = obj.dept.DeptName,
                    RoleId = obj.role.RoleId,
                    RoleName = obj.role.RoleName,
                    RoleDisplayName = obj.role.RoleDisplayName,
                    RoleDisplayText = obj.role.RoleDisplayName, //string.Format("{0} ({1})", obj.role.RoleDisplayName, obj.role.RoleName),
                    RoleSortNo = obj.role.SortNo,
                    OwnerDeptId = (obj.oe == null) ? 0 : obj.oe.DeptId ?? 0,
                    OwnerName = (obj.oe == null) ? "" : obj.oe.EmpName
                }).ToList();

                for (int rowIndex = 0; rowIndex < entities.Count; rowIndex++)
                {
                    entities[rowIndex].RowNum = skipCount + rowIndex + 1;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entities;
        }

        #endregion

        #region 網頁後端作業選項相關

        /// <summary>
        /// 取得後台用後端作業選項資料
        /// </summary>
        public OperationForBackend GetOperationDataForBackend(int opId)
        {
            Logger.Debug("GetOperationDataForBackend(opId)");

            OperationForBackend entity = null;

            try
            {
                entity = (from op in cmsCtx.Operations
                          join emp in cmsCtx.Employee.Include(emp => emp.Department)
                          on op.PostAccount equals emp.EmpAccount
                          where op.OpId == opId
                          select new OperationForBackend()
                          {
                              OpId = op.OpId,
                              ParentId = op.ParentId,
                              OpSubject = op.OpSubject,
                              LinkUrl = op.LinkUrl,
                              IsNewWindow = op.IsNewWindow,
                              IconImageFile = op.IconImageFile,
                              SortNo = op.SortNo,
                              IsHideSelf = op.IsHideSelf,
                              CommonClass = op.CommonClass,
                              PostAccount = op.PostAccount,
                              PostDate = op.PostDate,
                              MdfAccount = op.MdfAccount,
                              MdfDate = op.MdfDate,
                              EnglishSubject = op.EnglishSubject,
                              PostName = emp.EmpName,
                              PostDeptName = (emp.Department == null) ? null : emp.Department.DeptName
                          }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        /// <summary>
        /// 用共用元件類別名稱取得後端作業選項資訊
        /// </summary>
        public OperationOpInfo GetOperationOpInfoByCommonClass(string commonClass)
        {
            Logger.Debug("GetOperationOpInfoByCommonClass(commonClass)");

            OperationOpInfo entity = null;

            try
            {
                entity = (from op in cmsCtx.Operations
                          where op.CommonClass == commonClass
                          select new OperationOpInfo
                          {
                              OpId = op.OpId,
                              IsNewWindow = op.IsNewWindow
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        /// <summary>
        /// 用超連結網址取得後端作業選項資訊
        /// </summary>
        public OperationOpInfo GetOperationOpInfoByLinkUrl(string linkUrl)
        {
            Logger.Debug("GetOperationOpInfoByLinkUrl(linkUrl)");

            OperationOpInfo entity = null;

            try
            {
                entity = (from op in cmsCtx.Operations
                          where op.LinkUrl == linkUrl
                          select new OperationOpInfo
                          {
                              OpId = op.OpId,
                              IsNewWindow = op.IsNewWindow
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        #endregion

        #region 部門資料


        #endregion
    }
}
