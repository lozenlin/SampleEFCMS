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
using Common.DataAccess.EF.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.DataAccess.EF.EntityRequiredPropValues;

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

        /// <summary>
        /// 取得後台用帳號名單
        /// </summary>
        public List<EmployeeForBackend> GetEmployeeListForBackend(AccountListQueryParamsDA param)
        {
            Logger.Debug("GetEmployeeListForBackend(param)");

            List<EmployeeForBackend> entities = null;

            try
            {
                var tempQuery = from e in cmsCtx.Employee.Include(emp => emp.EmployeeRole).Include(emp => emp.Department)
                                join oe in cmsCtx.Employee
                                on e.OwnerAccount equals oe.EmpAccount
                                into empGroup
                                from oe in empGroup.DefaultIfEmpty()
                                select new EmployeeForBackend()
                                {
                                    EmpId = e.EmpId,
                                    EmpAccount = e.EmpAccount,
                                    EmpPassword = e.EmpPassword,
                                    EmpName = e.EmpName,
                                    Email = e.Email,
                                    Remarks = e.Remarks,
                                    IsAccessDenied = e.IsAccessDenied,
                                    PostAccount = e.PostAccount,
                                    PostDate = e.PostDate,
                                    MdfAccount = e.MdfAccount,
                                    MdfDate = e.MdfDate,
                                    StartDate = e.StartDate,
                                    EndDate = e.EndDate,
                                    OwnerAccount = e.OwnerAccount,
                                    ThisLoginTime = e.ThisLoginTime,
                                    ThisLoginIP = e.ThisLoginIP,
                                    LastLoginTime = e.LastLoginTime,
                                    LastLoginIP = e.LastLoginIP,
                                    PasswordHashed = e.PasswordHashed,
                                    DefaultRandomPassword = e.DefaultRandomPassword,
                                    DeptId = e.DeptId ?? 0,
                                    DeptName = e.Department.DeptName,
                                    RoleId = e.EmployeeRole.RoleId,
                                    RoleName = e.EmployeeRole.RoleName,
                                    RoleDisplayName = e.EmployeeRole.RoleDisplayName,
                                    RoleDisplayText = e.EmployeeRole.RoleDisplayName, //string.Format("{0} ({1})", role.RoleDisplayName, role.RoleName),
                                    RoleSortNo = e.EmployeeRole.SortNo,
                                    OwnerDeptId = oe.DeptId ?? 0,
                                    OwnerName = oe.EmpName ?? ""
                                };

                // Query conditions

                /*
                and (@CanReadSubItemOfOthers=1
	                or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	                or @CanReadSubItemOfSelf=1 and e.OwnerAccount=@MyAccount
	                or e.EmpAccount=@MyAccount)
                 */
                if (!param.AuthParams.CanReadSubItemOfOthers)
                {
                    tempQuery = tempQuery.Where(obj =>
                        param.AuthParams.CanReadSubItemOfCrew && obj.DeptId == param.AuthParams.MyDeptId
                        || param.AuthParams.CanReadSubItemOfSelf && obj.OwnerAccount == param.AuthParams.MyAccount
                        || obj.EmpAccount == param.AuthParams.MyAccount);
                }

                if (param.DeptId != 0) // 0:all
                {
                    tempQuery = tempQuery.Where(obj => obj.DeptId == param.DeptId);
                }

                if (param.Kw != "")
                {
                    tempQuery = tempQuery.Where(obj =>
                        obj.EmpAccount.Contains(param.Kw)
                        || obj.EmpName.Contains(param.Kw));
                }

                //清單內容模式(0:all, 1:normal, 2:access is denied)
                switch (param.ListMode)
                {
                    case 1:
                        tempQuery = tempQuery.Where(obj =>
                            !obj.IsAccessDenied
                            && (obj.RoleName == "admin"
                                || obj.StartDate <= DateTime.Now && DateTime.Now < DbFunctions.AddDays(obj.EndDate, 1)));
                        break;
                    case 2:
                        tempQuery = tempQuery.Where(obj =>
                            obj.IsAccessDenied
                            || !(obj.RoleName == "admin"
                                || obj.StartDate <= DateTime.Now && DateTime.Now < DbFunctions.AddDays(obj.EndDate, 1)));
                        break;
                }

                // sorting
                if (param.PagedParams.SortField != "")
                {
                    tempQuery = tempQuery.OrderBy(param.PagedParams.SortField, param.PagedParams.IsSortDesc);
                }
                else
                {
                    // default
                    tempQuery = tempQuery
                        .OrderBy(obj => obj.DeptId)
                        .ThenBy(obj => obj.RoleSortNo)
                        .ThenBy(obj => obj.EmpName);
                }

                // total
                param.PagedParams.RowCount = tempQuery.Count();

                // paging
                int skipCount = param.PagedParams.GetSkipCount();
                int takeCount = param.PagedParams.GetTakeCount();

                if (skipCount > 0)
                {
                    tempQuery = tempQuery.Skip(skipCount);
                }

                if (takeCount >= 0)
                {
                    tempQuery = tempQuery.Take(takeCount);
                }

                // result
                entities = tempQuery.ToList();

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

        #region 員工身分後端作業授權相關

        /// <summary>
        /// 取得指定作業代碼的後端身分可使用權限
        /// </summary>
        public EmployeeRoleOperationsDesc GetEmployeeRoleOperationsDescDataOfOp(string roleName, int opId)
        {
            Logger.Debug("GetEmployeeRoleOperationsDescDataOfOp(roleName, opId)");

            EmployeeRoleOperationsDesc entity = null;

            try
            {
                entity = cmsCtx.EmployeeRoleOperationsDesc.Where(ro => ro.RoleName == roleName && ro.OpId == opId)
                    .FirstOrDefault();
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        #endregion

        #region 員工身分

        /// <summary>
        /// 取得後台用員工身分資料
        /// </summary>
        public EmployeeRoleForBackend GetEmployeeRoleDataForBackend(int roleId)
        {
            Logger.Debug("GetEmployeeRoleDataForBackend(roleId)");

            EmployeeRoleForBackend entity = null;

            try
            {
                entity = (from r in cmsCtx.EmployeeRole
                          join e in cmsCtx.Employee
                          on r.PostAccount equals e.EmpAccount
                          into roleGroup
                          from e in roleGroup.DefaultIfEmpty()
                          where r.RoleId == roleId
                          select new EmployeeRoleForBackend()
                          {
                              RoleId = r.RoleId,
                              RoleName = r.RoleName,
                              RoleDisplayName = r.RoleDisplayName,
                              SortNo = r.SortNo,
                              PostAccount = r.PostAccount ?? "",
                              PostDate = r.PostDate,
                              MdfAccount = r.MdfAccount,
                              MdfDate = r.MdfDate,
                              PostDeptId = e.DeptId ?? 0
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
        /// 取得後台用員工身分清單
        /// </summary>
        public List<EmployeeRoleForBackend> GetEmployeeRoleListForBackend(RoleListQueryParamsDA param)
        {
            Logger.Debug("GetEmployeeRoleListForBackend(param)");

            List<EmployeeRoleForBackend> entities = null;

            try
            {
                var tempQuery = from r in cmsCtx.EmployeeRole
                                join e in cmsCtx.Employee
                                on r.PostAccount equals e.EmpAccount
                                into roleGroup
                                from e in roleGroup.DefaultIfEmpty()
                                select new EmployeeRoleForBackend()
                                {
                                    RoleId = r.RoleId,
                                    RoleName = r.RoleName,
                                    RoleDisplayName = r.RoleDisplayName,
                                    SortNo = r.SortNo,
                                    PostAccount = r.PostAccount ?? "",
                                    PostDate = r.PostDate,
                                    MdfAccount = r.MdfAccount,
                                    MdfDate = r.MdfDate,
                                    PostDeptId = e.DeptId ?? 0,
                                    EmpTotal = cmsCtx.Employee.Where(emp => emp.RoleId == r.RoleId).Count()
                                };

                // Qeury conditions

                if (!param.AuthParams.CanReadSubItemOfOthers)
                {
                    tempQuery = tempQuery.Where(obj =>
                        param.AuthParams.CanReadSubItemOfCrew && obj.PostDeptId == param.AuthParams.MyDeptId
                        || param.AuthParams.CanReadSubItemOfSelf && obj.PostAccount == param.AuthParams.MyAccount);
                }

                if (param.Kw != "")
                {
                    tempQuery = tempQuery.Where(obj =>
                        obj.RoleName.Contains(param.Kw)
                        || obj.RoleDisplayName.Contains(param.Kw));
                }

                // sorting
                if (param.PagedParams.SortField != "")
                {
                    tempQuery = tempQuery.OrderBy(param.PagedParams.SortField, param.PagedParams.IsSortDesc);
                }
                else
                {
                    // default
                    tempQuery = tempQuery.OrderBy(obj => obj.SortNo);
                }

                // total
                param.PagedParams.RowCount = tempQuery.Count();

                // paging
                int skipCount = param.PagedParams.GetSkipCount();
                int takeCount = param.PagedParams.GetTakeCount();

                if (skipCount > 0)
                {
                    tempQuery = tempQuery.Skip(skipCount);
                }

                if (takeCount >= 0)
                {
                    tempQuery = tempQuery.Take(takeCount);
                }

                // result
                entities = tempQuery.ToList();

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

        /// <summary>
        /// 刪除員工身分
        /// </summary>
        public bool DeleteEmployeeRoleData(int roleId)
        {
            Logger.Debug("DeleteEmployeeRoleData(roleId)");

            bool result = false;
            DbContextTransaction tran = null;

            try
            {
                // 檢查關聯帳號
                if (cmsCtx.Employee.Any(emp => emp.RoleId == roleId))
                {
                    sqlErrNumber = 50000;
                    sqlErrState = 2;
                    return false;
                }

                tran = cmsCtx.Database.BeginTransaction();

                // 先刪除授權設定
                string roleName = cmsCtx.EmployeeRole.Where(role => role.RoleId == roleId)
                    .Select(role => role.RoleName)
                    .FirstOrDefault();

                cmsCtx.Database.ExecuteSqlCommand("delete from dbo.EmployeeRoleOperationsDesc where RoleName=@p0", roleName);

                // main data
                EmployeeRole entity = new EmployeeRole()
                {
                    RoleId = roleId
                };

                cmsCtx.Entry<EmployeeRole>(entity).State = EntityState.Deleted;
                cmsCtx.SaveChanges();

                tran.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;

                if (tran != null)
                    tran.Rollback();

                return false;
            }
            finally
            {
                if (tran != null)
                    tran.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 取得員工身分最大排序編號
        /// </summary>
        public int GetEmployeeRoleMaxSortNo()
        {
            Logger.Debug("GetEmployeeRoleMaxSortNo()");

            int result = 0;

            try
            {
                result = cmsCtx.EmployeeRole.Max(emp => emp.SortNo) ?? 0;
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return -1;
            }

            return result;
        }

        /// <summary>
        /// 新增員工身分資料
        /// </summary>
        public InsertResult InsertEmployeeRoleData(EmployeeRole entity, string copyPrivilegeFromRoleName)
        {
            Logger.Debug("InsertEmployeeRoleData(entity, copyPrivilegeFromRoleName)");

            InsertResult insResult = new InsertResult() { IsSuccess = false };
            DbContextTransaction tran = null;

            try
            {
                // check role name
                if(cmsCtx.EmployeeRole.Any(role=> role.RoleName == entity.RoleName))
                {
                    sqlErrNumber = 50000;
                    sqlErrState = 2;
                    return insResult;
                }

                tran = cmsCtx.Database.BeginTransaction();

                cmsCtx.EmployeeRole.Add(entity);
                cmsCtx.SaveChanges();
                insResult.NewId = entity.GetIdentityColValue();

                // copy privilege
                var empRoleOps = cmsCtx.EmployeeRoleOperationsDesc.Where(ro => ro.RoleName == copyPrivilegeFromRoleName)
                    .AsEnumerable()
                    .Select(ro => new EmployeeRoleOperationsDesc()
                    {
                        RoleName = entity.RoleName,
                        OpId = ro.OpId,
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
                        PostAccount = entity.PostAccount,
                        PostDate = DateTime.Now
                    });

                cmsCtx.EmployeeRoleOperationsDesc.AddRange(empRoleOps);
                cmsCtx.SaveChanges();

                tran.Commit();
                insResult.IsSuccess = true;
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;

                if (tran != null)
                    tran.Rollback();

                return insResult;
            }
            finally
            {
                if (tran != null)
                    tran.Dispose();
            }

            return insResult;
        }

        /// <summary>
        /// 儲存員工身分後端作業授權清單
        /// </summary>
        public bool SaveListOfEmployeeRolePrivileges(List<EmployeeRoleOperationsDesc> empRoleOps)
        {
            Logger.Debug("SaveListOfEmployeeRolePrivileges(empRoleOps)");

            bool result = false;

            try
            {
                // divide into insert or update
                empRoleOps.ForEach(ro =>
                {
                    if (cmsCtx.EmployeeRoleOperationsDesc.Any(obj => obj.RoleName == ro.RoleName && obj.OpId == ro.OpId))
                    {
                        // update
                        var entry = cmsCtx.Entry<EmployeeRoleOperationsDesc>(ro);
                        entry.State = EntityState.Unchanged;

                        entry.Property(obj => obj.CanRead).IsModified = true;
                        entry.Property(obj => obj.CanEdit).IsModified = true;
                        entry.Property(obj => obj.CanReadSubItemOfSelf).IsModified = true;
                        entry.Property(obj => obj.CanEditSubItemOfSelf).IsModified = true;
                        entry.Property(obj => obj.CanAddSubItemOfSelf).IsModified = true;
                        entry.Property(obj => obj.CanDelSubItemOfSelf).IsModified = true;
                        entry.Property(obj => obj.CanReadSubItemOfCrew).IsModified = true;
                        entry.Property(obj => obj.CanEditSubItemOfCrew).IsModified = true;
                        entry.Property(obj => obj.CanDelSubItemOfCrew).IsModified = true;
                        entry.Property(obj => obj.CanReadSubItemOfOthers).IsModified = true;
                        entry.Property(obj => obj.CanEditSubItemOfOthers).IsModified = true;
                        entry.Property(obj => obj.CanDelSubItemOfOthers).IsModified = true;
                        ro.MdfAccount = ro.PostAccount;
                        ro.MdfDate = ro.PostDate;
                        entry.Property(obj => obj.MdfAccount).IsModified = true;
                        entry.Property(obj => obj.MdfDate).IsModified = true;
                    }
                    else
                    {
                        // insert
                        ro.MdfAccount = null;
                        ro.MdfDate = null;
                        cmsCtx.EmployeeRoleOperationsDesc.Add(ro);
                    }
                });

                cmsCtx.SaveChanges();
                result = true;
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;

                return false;
            }

            return result;
        }

        #endregion
    }
}
