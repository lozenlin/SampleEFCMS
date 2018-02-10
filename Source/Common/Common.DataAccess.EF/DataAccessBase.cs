using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.DataAccess.EF.Model;
using log4net;

namespace Common.DataAccess.EF
{
    public abstract class DataAccessBase : IDisposable
    {
        protected CmsContext cmsCtx;
        protected ILog logger;

        private bool disposed = false;

        public DataAccessBase()
        {
            cmsCtx = new CmsContext();
            logger = LogManager.GetLogger(this.GetType());
            cmsCtx.Database.Log = (msg) => logger.Debug(msg);
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

    }
}
