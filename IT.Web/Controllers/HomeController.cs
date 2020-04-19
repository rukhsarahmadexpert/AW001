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
    [TlsAttribute]
    public class HomeController : Controller
    {
        List<CustomerNotificationViewModel> customerNotificationViewModels = new List<CustomerNotificationViewModel>();
        CustomerOrderStatistics customerOrderStatistics = new CustomerOrderStatistics();
        AboutViewModel aboutViewModel = new AboutViewModel();
        ServiceViewModel serviceViewModel = new ServiceViewModel();
        PrivatePolicyViewModel privatePolicyViewModel = new PrivatePolicyViewModel();
        
        WebServices webServices = new WebServices();
                
        public ActionResult Details()
        {
            var result = webServices.Post(new ServiceViewModel(), "OurServices/All");

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (result.Data != null)
                {
                    serviceViewModel = (new JavaScriptSerializer().Deserialize<ServiceViewModel>(result.Data.ToString()));
                }
                ViewBag.serviceViewModel = serviceViewModel;
            }
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
                    ViewBag.fuelPricesViewModels = fuelPricesViewModels;

                    var RequestedData = customerOrderStatistics.RequestedBySevenDayed;
                    var userCompanyViewModel = new UserCompanyViewModel();
                    Session["RequestedData"] = RequestedData;
                    userCompanyViewModel = Session["userCompanyViewModel"] as UserCompanyViewModel;

                    if (userCompanyViewModel != null)
                    {
                        TempData["Title"] = userCompanyViewModel.CompanyName ?? "Unknown";
                    }

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

            var result = webServices.Post(new AboutViewModel(), "AboutUs/Index");

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (result.Data != null)
                {
                    aboutViewModel = (new JavaScriptSerializer().Deserialize<AboutViewModel>(result.Data.ToString()));
                }
                return View(aboutViewModel);
            }
            return View();
        }

        [Autintication]
        public ActionResult BulkStorage()
        {
            var result = webServices.Post(new ServiceViewModel(), "OurServices/All");

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (result.Data != null)
                {
                    serviceViewModel = (new JavaScriptSerializer().Deserialize<ServiceViewModel>(result.Data.ToString()));
                }
                ViewBag.serviceViewModel = serviceViewModel;
            }
            return View();
        }
              
        public ActionResult FuelTransportation()
        {
            var result = webServices.Post(new ServiceViewModel(), "OurServices/All");

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (result.Data != null)
                {
                    serviceViewModel = (new JavaScriptSerializer().Deserialize<ServiceViewModel>(result.Data.ToString()));
                }
                ViewBag.serviceViewModel = serviceViewModel;
            }
            return View();
        }
               
        public ActionResult Products()
        {
            var result = webServices.Post(new ServiceViewModel(), "OurServices/All");

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (result.Data != null)
                {
                    serviceViewModel = (new JavaScriptSerializer().Deserialize<ServiceViewModel>(result.Data.ToString()));
                }
                ViewBag.serviceViewModel = serviceViewModel;
            }
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
                ViewBag.fuelPricesViewModels = fuelPricesViewModels;
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Policy()
        {

            var result = webServices.Post(new PrivatePolicyViewModel(), "PrivacyPolicy/Index");

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (result.Data != null)
                {
                    privatePolicyViewModel = (new JavaScriptSerializer().Deserialize<PrivatePolicyViewModel>(result.Data.ToString()));
                }
                ViewBag.privatePolicyViewModel = privatePolicyViewModel;
            }
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
                //loginViewModel.DeviceId = System.Web.HttpContext.Current.Server.MachineName;
                //loginViewModel.DeviceId = loginViewModel.DeviceId = Request.UserHostAddress;
                //loginViewModel.DeviceId = loginViewModel.DeviceId = Request.UserHostAddress;
                loginViewModel.DeviceId = loginViewModel.Token.Substring(0, 10);
                loginViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                loginViewModel.Authority = userCompanyViewModel.Authority;
                loginViewModel.UserName = userCompanyViewModel.UserName;

                Session["Token"] = loginViewModel.Token.ToString();
                
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