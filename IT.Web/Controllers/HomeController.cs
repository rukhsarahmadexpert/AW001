using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    [ExceptionLog]
   
    public class HomeController : Controller
    {
        List<CustomerNotificationViewModel> customerNotificationViewModels = new List<CustomerNotificationViewModel>();
        CustomerOrderStatistics customerOrderStatistics = new CustomerOrderStatistics();

        WebServices webServices = new WebServices();

        public ActionResult Details()
        {
            return View();
        }

        [Autintication]
        public ActionResult Index()
        {
            var fuelPricesViewModels = new List<FuelPricesViewModel>();
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (CompanyId < 1)
                {
                    return RedirectToAction("Create", "Company");
                }
                else
                {

                    if (HttpContext.Cache["customerNotificationViewModels"] == null)
                    {
                        var result = webServices.Post(new CustomerNotificationViewModel(), "Advertisement/All");

                        if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            if (result.Data != null)
                            {
                                customerNotificationViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerNotificationViewModel>>(result.Data.ToString()));
                                HttpContext.Cache["customerNotificationViewModels"] = customerNotificationViewModels;
                            }
                        }
                    }
                    else
                    {
                        customerNotificationViewModels = HttpContext.Cache["customerNotificationViewModels"] as List<CustomerNotificationViewModel>;
                    }
                    ViewBag.customerNotificationViewModels = customerNotificationViewModels;

                    SearchViewModel searchViewModel = new SearchViewModel
                    { 
                        CompanyId = Convert.ToInt32(Session["CompanyId"])
                    };
                    var resultCustomerStatistics = webServices.Post(searchViewModel, "CustomerOrder/CustomerStatistics");
                    if (resultCustomerStatistics.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        customerOrderStatistics = (new JavaScriptSerializer().Deserialize<CustomerOrderStatistics>(resultCustomerStatistics.Data.ToString()));
                    }
                    ViewBag.customerOrderStatistics = customerOrderStatistics;

                    FuelPricesViewModel fuelPricesViewModel = new FuelPricesViewModel();

                    var resultFuel = webServices.Post(fuelPricesViewModel, "FuelPrices/FuelPricesTopOne");
                    if (resultFuel.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        fuelPricesViewModels = (new JavaScriptSerializer().Deserialize<List<FuelPricesViewModel>>(resultFuel.Data.ToString()));
                    }
                    ViewBag.fuelPricesViewModel = fuelPricesViewModels[0];

                    var RequestedData = customerOrderStatistics.RequestedBySevenDayed;
                    var userCompanyViewModel = new UserCompanyViewModel();
                    Session["RequestedData"] = RequestedData;
                    userCompanyViewModel = Session["userCompanyViewModel"] as UserCompanyViewModel;
                    TempData["Title"] = userCompanyViewModel.CompanyName;

                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult BulkStorage()
        {
            return View();
        }

        public ActionResult FuelTransportation()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Training()
        {
            return View();
        }

        public ActionResult Contact()
        {
           
            return View();
        }

        [Autintication]
        public ActionResult CustomerHomes()
        {
            return View();
        }


        [Autintication]
        public ActionResult AdminHome()
        {
            try
            {

                if (HttpContext.Cache["customerNotificationViewModels"] == null)
                {
                    var result = webServices.Post(new CustomerNotificationViewModel(), "Advertisement/All");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != null)
                        {
                            customerNotificationViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerNotificationViewModel>>(result.Data.ToString()));
                            HttpContext.Cache["customerNotificationViewModels"] = customerNotificationViewModels;
                        }
                    }
                }
                else
                {
                    customerNotificationViewModels = HttpContext.Cache["customerNotificationViewModels"] as List<CustomerNotificationViewModel>;
                }
                ViewBag.customerNotificationViewModels = customerNotificationViewModels;

                SearchViewModel searchViewModel = new SearchViewModel
                {
                    CompanyId = Convert.ToInt32(Session["CompanyId"])
                };
                var resultCustomerStatistics = webServices.Post(searchViewModel, "CustomerOrder/AdminStatistics");
                if (resultCustomerStatistics.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerOrderStatistics = (new JavaScriptSerializer().Deserialize<CustomerOrderStatistics>(resultCustomerStatistics.Data.ToString()));
                }
                ViewBag.customerOrderStatistics = customerOrderStatistics;

                List<FuelPricesViewModel> fuelPricesViewModels = new List<FuelPricesViewModel>();

                var resultFuel = webServices.Post(new FuelPricesViewModel(), "FuelPrices/FuelPricesTopOne");
                if (resultFuel.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                     fuelPricesViewModels = (new JavaScriptSerializer().Deserialize<List<FuelPricesViewModel>>(resultFuel.Data.ToString()));
                }
                ViewBag.fuelPricesViewModel = fuelPricesViewModels[0];
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Policy()
        {
            return View();
        }

        public ActionResult StorgeGraphPartialView()
        {
            return PartialView("~/Views/Shared/PartialView/StorageGraph/StorgeGraphPartialView.cshtml");
        }

        [Autintication]
        [HttpPost]
        public ActionResult UpdateToken(LoginViewModel loginViewModel)
        {
            try
            {
                UserCompanyViewModel userCompanyViewModel = new UserCompanyViewModel();
                userCompanyViewModel = Session["userCompanyViewModel"] as UserCompanyViewModel;

                loginViewModel.Token = loginViewModel.Token ?? "token not availibe";
                loginViewModel.Device = "web";
               // loginViewModel.DeviceId = System.Net.Dns.GetHostName().ToString();
                loginViewModel.DeviceId = System.Environment.GetEnvironmentVariable("COMPUTERNAME").ToString(); 
                loginViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                loginViewModel.Authority = userCompanyViewModel.Authority;
                loginViewModel.UserName = userCompanyViewModel.UserName;

                var result = webServices.Post(loginViewModel, "User/UpdateToken", false);
                return Json("Success",JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}