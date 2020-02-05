using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    public class VehicleTypeController : Controller
    {
        WebServices webServices = new WebServices();
        List<VehicleTypeViewModel> vehicleTypeViewModels = new List<VehicleTypeViewModel>();

        public ActionResult Index()
        {
            return View();
        }

        [NonAction]
        public List<VehicleTypeViewModel> VehicleTypes()
        {
            try
            {
                var TypeList = webServices.Post(new ProductViewModel(), "VehicleType/GetAll");

                if (TypeList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    vehicleTypeViewModels = (new JavaScriptSerializer().Deserialize<List<VehicleTypeViewModel>>(TypeList.Data.ToString()));
                }

                return vehicleTypeViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}