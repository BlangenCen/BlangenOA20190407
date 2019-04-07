using BlangenOA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.DALFactory
{
    public class DbSessionFactory
    {
        /// <summary>
        /// 线程内唯一DbSession
        /// </summary>
        /// <returns>DbSession</returns>
        public static IDbSession CreateDbSession()
        {
            IDbSession dbSession = CallContext.GetData("dbSession") as DbSession;
            if (dbSession == null)
            {
                dbSession = new DbSession();
                CallContext.SetData("dbSession", dbSession);
            }
            return dbSession;
        }
    }
}
