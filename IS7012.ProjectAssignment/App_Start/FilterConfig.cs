﻿using System.Web;
using System.Web.Mvc;

namespace IS7012.ProjectAssignment
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
