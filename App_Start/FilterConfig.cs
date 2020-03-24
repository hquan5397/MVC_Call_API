using System.Web;
using System.Web.Mvc;

namespace Token_Based_Authentication_17_3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
