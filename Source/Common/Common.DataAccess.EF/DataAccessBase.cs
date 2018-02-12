using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.DataAccess.EF.Model;
using log4net;
using System.Linq.Expressions;

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

        public TEntity Get<TEntity>(params object[] keyValues) where TEntity :class
        {
            return cmsCtx.Set<TEntity>().Find(keyValues);
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return cmsCtx.Set<TEntity>().FirstOrDefault(predicate);
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
