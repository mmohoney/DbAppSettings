using System.Web.Mvc;

namespace WebDbAppSettingsMaintenance.Areas.DbAppSettings
{
    public class MaintenanceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DbAppSettings";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DbAppSettings_default",
                "DbAppSettings/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}