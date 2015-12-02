using System.Web;
using System.Web.Mvc;

namespace InmueblesCarso_InCarso
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
