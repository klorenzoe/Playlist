using System.Web;
using System.Web.Mvc;

namespace Laboratorio0_1016816
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
