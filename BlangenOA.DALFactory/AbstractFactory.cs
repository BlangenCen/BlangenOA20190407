using BlangenOA.DAL;
using BlangenOA.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.DALFactory
{
    public partial class AbstractFactory
    {
        //程序集名称
        public readonly static string AssemblyPath = ConfigurationManager.AppSettings["AssemblyPath"];
        //命名空间名称
        public static readonly string Namespace = ConfigurationManager.AppSettings["Namespace"];
        /// <summary>
        /// 反射
        /// </summary>
        /// <param name="fullClassName">类全称</param>
        /// <returns>类实例</returns>
        public static object CreateInstance(string fullClassName)
        {
            Assembly a = Assembly.Load(AssemblyPath);
            return a.CreateInstance(fullClassName);
        }
        /// <summary>
        /// 创建UserInfoDal实例
        /// </summary>
        /// <returns>UserInfoDal实例</returns>
        //public static IUserInfoDal CreateUserInfoDal()
        //{
        //    string fullClassName = Namespace + ".UserInfoDal";
        //    return CreateInstance(fullClassName) as UserInfoDal;
        //}
        
    }
}
