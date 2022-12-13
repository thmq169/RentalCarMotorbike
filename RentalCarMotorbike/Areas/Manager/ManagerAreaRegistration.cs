using System.Web.Mvc;

namespace RentalCarMotorbike.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Manager_default",
                
                "Manager/{controller}/{action}/{id}",
                defaults: new { action = "Index", controller= "Vehicle", id = UrlParameter.Optional }
            );
        }
    }
}