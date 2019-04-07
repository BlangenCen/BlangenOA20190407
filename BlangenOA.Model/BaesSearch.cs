using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlangenOA.Model
{
    /// <summary>
    /// 基本的搜索条件
    /// </summary>
    public class BaesSearch
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
