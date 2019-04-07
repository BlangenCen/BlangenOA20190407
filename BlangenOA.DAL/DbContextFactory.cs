using BlangenOA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.DAL
{
    public class DbContextFactory
    {
        /// <summary>
        /// 线程内唯一EF实例
        /// </summary>
        /// <returns></returns>
        public static DbContext CreateDbContext()
        {
            DbContext db = CallContext.GetData("db") as DbContext;
            if (db == null)
            {
                db = new ItcastCmsEntities();
                CallContext.SetData("db", db);
            }
            return db;
        }
    }
}
