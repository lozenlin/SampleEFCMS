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
                entity.OwnerDeptId = cmsCtx.Employee.Where(emp => emp.EmpAccount == entity.OwnerAccount)
                    .Select(emp => emp.DeptId)
                    .FirstOrDefault() ?? 0;
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
                entity.OwnerDeptId = cmsCtx.Employee.Where(emp => emp.EmpAccount == entity.OwnerAccount)
                    .Select(emp => emp.DeptId)
                    .FirstOrDefault() ?? 0;
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
                              OpId = op.OpId
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
        /// 取得後端作業選項第一層清單和身分授權
        /// </summary>
        public List<OperationWithRoleAuth> GetOperationsTopListWithRoleAuth(string roleName)
        {
            Logger.Debug("GetOperationsTopListWithRoleAuth(roleName)");

            List<OperationWithRoleAuth> entities = null;

            try
            {
                //todo by lozen
                var entities2 = (from op in cmsCtx.Operations
                            join ro in cmsCtx.EmployeeRoleOperationsDesc
                            on new
                            {
                                op.OpId,
                                roleName = roleName
                            } equals new
                            {
                                ro.OpId,
                                roleName = ro.RoleName
                            }
                            into opGroup
                            from ro in opGroup.DefaultIfEmpty()
                            where op.ParentId == null
                               && !op.IsHideSelf
                            orderby op.SortNo
                            select opGroup /*new OperationWithRoleAuth()
                            {
                                OpId = op.OpId,
                                OpSubject = op.OpSubject,
                                LinkUrl = op.LinkUrl,
                                IsNewWindow = op.IsNewWindow,
                                IconImageFile = op.IconImageFile,
                                EnglishSubject = op.EnglishSubject,
                                CanRead = ro.CanRead,
                                CanEdit = ro.CanEdit,
                                CanReadSubItemOfSelf = ro.CanReadSubItemOfSelf,
                                CanEditSubItemOfSelf = ro.CanEditSubItemOfSelf,
                                CanAddSubItemOfSelf = ro.CanAddSubItemOfSelf,
                                CanDelSubItemOfSelf = ro.CanDelSubItemOfSelf,
                                CanReadSubItemOfCrew = ro.CanReadSubItemOfCrew,
                                CanEditSubItemOfCrew = ro.CanEditSubItemOfCrew,
                                CanDelSubItemOfCrew = ro.CanDelSubItemOfCrew,
                                CanReadSubItemOfOthers = ro.CanReadSubItemOfOthers,
                                CanEditSubItemOfOthers = ro.CanEditSubItemOfOthers,
                                CanDelSubItemOfOthers = ro.CanDelSubItemOfOthers,
                                PostAccount = ro.PostAccount,
                                PostDate = ro.PostDate,
                                MdfAccount = ro.MdfAccount,
                                MdfDate = ro.MdfDate
                            }*/).ToList();
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entities;
        }

        /// <summary>
        /// 取得後端作業選項子清單和身分授權
        /// </summary>
        public List<OperationWithRoleAuth> GetOperationsSubListWithRoleAuth(string roleName)
        {
            Logger.Debug("GetOperationsSubListWithRoleAuth(roleName)");

            List<OperationWithRoleAuth> entities = null;

            try
            {
                //todo by lozen
                entities = (from op in cmsCtx.Operations
                            join ro in cmsCtx.EmployeeRoleOperationsDesc
                            on new
                            {
                                op.OpId,
                                roleName = roleName
                            } equals new
                            {
                                ro.OpId,
                                roleName = ro.RoleName
                            }
                            into opGroup
                            from ro in opGroup.DefaultIfEmpty()
                            where op.ParentId != null
                                && !op.IsHideSelf
                            orderby op.SortNo
                            select new OperationWithRoleAuth()
                            {
                                OpId = op.OpId,
                                ParentId = op.ParentId,
                                OpSubject = op.OpSubject,
                                LinkUrl = op.LinkUrl,
                                IsNewWindow = op.IsNewWindow,
                                IconImageFile = op.IconImageFile,
                                EnglishSubject = op.EnglishSubject,
                                CanRead = ro.CanRead,
                                CanEdit = ro.CanEdit,
                                CanReadSubItemOfSelf = ro.CanReadSubItemOfSelf,
                                CanEditSubItemOfSelf = ro.CanEditSubItemOfSelf,
                                CanAddSubItemOfSelf = ro.CanAddSubItemOfSelf,
                                CanDelSubItemOfSelf = ro.CanDelSubItemOfSelf,
                                CanReadSubItemOfCrew = ro.CanReadSubItemOfCrew,
                                CanEditSubItemOfCrew = ro.CanEditSubItemOfCrew,
                                CanDelSubItemOfCrew = ro.CanDelSubItemOfCrew,
                                CanReadSubItemOfOthers = ro.CanReadSubItemOfOthers,
                                CanEditSubItemOfOthers = ro.CanEditSubItemOfOthers,
                                CanDelSubItemOfOthers = ro.CanDelSubItemOfOthers,
                                PostAccount = ro.PostAccount,
                                PostDate = ro.PostDate,
                                MdfAccount = ro.MdfAccount,
                                MdfDate = ro.MdfDate
                            }).ToList();
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
    }
}
