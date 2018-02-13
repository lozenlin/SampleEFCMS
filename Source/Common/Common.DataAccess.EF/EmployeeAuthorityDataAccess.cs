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
            EmployeeToLogin result = null;

            try
            {
                result = (from emp in cmsCtx.Employee
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

            return result;
        }

        #endregion
    }
}
