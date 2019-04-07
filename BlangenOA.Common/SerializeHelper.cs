using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.Common
{
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeToString(object o)
        {
            return JsonConvert.SerializeObject(o);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T DeSerializeToT<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
