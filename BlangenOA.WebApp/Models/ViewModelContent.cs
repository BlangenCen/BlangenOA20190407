using BlangenOA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlangenOA.WebApp.Models
{
    public class ViewModelContent
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public LuceneEnumType LuceneEnumType { get; set; }
    }
}