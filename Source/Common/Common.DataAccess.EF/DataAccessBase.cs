// ===============================================================================
// DataAccessBase of SampleEFCMS
// https://github.com/lozenlin/SampleEFCMS
//
// DataAccessBase.cs
//
// ===============================================================================
// Copyright (c) 2018 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.DataAccess.EF.Model;
using log4net;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.DataAccess.EF
{
    public abstract class DataAccessBase : IDisposable
    {
        protected ILog Logger
        {
            get;
            set;
        }
        protected CmsContext cmsCtx;
        protected string errMsg = "";
        protected int sqlErrNumber = 0;
        protected int sqlErrState = 0;

        private bool disposed = false;

        public DataAccessBase()
        {
            cmsCtx = new CmsContext();
            Logger = LogManager.GetLogger(this.GetType());
            cmsCtx.Database.Log = (msg) => Logger.Debug(msg);
        }

        /// <summary>
        /// 執行後的錯誤訊息
        /// </summary>
        public string GetErrMsg()
        {
            return errMsg;
        }

        public int GetSqlErrNumber()
        {
            return sqlErrNumber;
        }

        public int GetSqlErrState()
        {
            return sqlErrState;
        }

        #region Finalize and Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (cmsCtx != null)
                {
                    cmsCtx.Dispose();
                }

            }

            disposed = true;
        }

        ~DataAccessBase()
        {
            Dispose(false);
        }

        #endregion

        public InsertResult Insert<TEntity>(TEntity entity) where TEntity : class
        {
            InsertResult insResult = new InsertResult() { IsSuccess = false };

            try
            {
                cmsCtx.Set<TEntity>().Add(entity);
                cmsCtx.SaveChanges();

                if (entity is IHasIdentityCol<int>)
                {
                    insResult.NewId = ((IHasIdentityCol<int>)entity).GetIdentityColValue();
                }

                insResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                System.Data.SqlClient.SqlException sqlex = ex.GetBaseException() as System.Data.SqlClient.SqlException;

                if (sqlex != null)
                {
                    errMsg = sqlex.Message;
                    sqlErrNumber = sqlex.Number;
                    sqlErrState = sqlex.State;
                }
            }

            return insResult;
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                cmsCtx.Entry<TEntity>(entity).State = EntityState.Deleted;
                cmsCtx.SaveChanges();
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// generate an entity with required property values that state is unchanged
        /// </summary>
        public TEntity GetEmptyEntity<TEntity>(object requiredPropValues) where TEntity : class
        {
            TEntity entity = Activator.CreateInstance<TEntity>();

            if (requiredPropValues != null)
            {
                //設定必要屬性
                Dictionary<string, PropertyInfo> dicEntityProps = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] entityProps = entity.GetType().GetProperties();
                
                foreach (PropertyInfo entityProp in entityProps)
                {
                    dicEntityProps.Add(entityProp.Name, entityProp);
                }

                PropertyInfo[] initProps = requiredPropValues.GetType().GetProperties();

                foreach (PropertyInfo initProp in initProps)
                {
                    string propertyName = initProp.Name;
                    object propertyValue = initProp.GetValue(requiredPropValues);
                    
                    dicEntityProps[propertyName].SetValue(entity, propertyValue);
                }
            }

            cmsCtx.Entry<TEntity>(entity).State = EntityState.Unchanged;

            return entity;
        }

        public TEntity Get<TEntity>(params object[] pkValues) where TEntity :class
        {
            TEntity entity = null;

            try
            {
                entity = cmsCtx.Set<TEntity>().Find(pkValues);
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            TEntity entity = null;

            try
            {
                entity = cmsCtx.Set<TEntity>().FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entity;
        }

        public List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            List<TEntity> entities = new List<TEntity>();

            try
            {
                entities = cmsCtx.Set<TEntity>().Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                return null;
            }

            return entities;
        }

        public int GetCount<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return cmsCtx.Set<TEntity>().Count(predicate);
        }

        public bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return cmsCtx.Set<TEntity>().Any(predicate);
        }

        public bool UpdateAllCols<TEntity>(TEntity entity) where TEntity :class
        {
            try
            {
                cmsCtx.Entry<TEntity>(entity).State = EntityState.Modified;
                cmsCtx.SaveChanges();
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                System.Data.SqlClient.SqlException sqlex = ex.GetBaseException() as System.Data.SqlClient.SqlException;

                if (sqlex != null)
                {
                    errMsg = sqlex.Message;
                    sqlErrNumber = sqlex.Number;
                    sqlErrState = sqlex.State;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// context.SaveChanges()
        /// </summary>
        public bool Update()
        {
            try
            {
                cmsCtx.SaveChanges();
            }
            catch(Exception ex)
            {
                Logger.Error("", ex);
                errMsg = ex.Message;
                System.Data.SqlClient.SqlException sqlex = ex.GetBaseException() as System.Data.SqlClient.SqlException;

                if (sqlex != null)
                {
                    errMsg = sqlex.Message;
                    sqlErrNumber = sqlex.Number;
                    sqlErrState = sqlex.State;
                }

                return false;
            }

            return true;
        }
    }
}
