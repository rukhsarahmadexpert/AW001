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
    [Autintication]
    public class HomeController : Controller
    {
        List<CustomerNotificationViewModel> customerNotificationViewModels = new List<CustomerNotificationViewModel>();
        CustomerOrderStatistics customerOrderStatistics = new CustomerOrderStatistics();

        WebServices webServices = new WebServices();

        public ActionResult Index()
        {
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

                    SearchViewModel searchViewModel = new SearchViewModel();
                    searchViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);

                    var resultCustomerStatistics = webServices.Post(searchViewModel, "CustomerOrder/CustomerStatistics");
                    if (resultCustomerStatistics.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        customerOrderStatistics = (new JavaScriptSerializer().Deserialize<CustomerOrderStatistics>(resultCustomerStatistics.Data.ToString()));
                    }
                    ViewBag.customerOrderStatistics = customerOrderStatistics;

                    var RequestedData = customerOrderStatistics.RequestedBySevenDayed;

                    Session["RequestedData"] = RequestedData;

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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CustomerHomes()
        {
            return View();
        }

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

                SearchViewModel searchViewModel = new SearchViewModel();
                searchViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var resultCustomerStatistics = webServices.Post(searchViewModel, "CustomerOrder/AdminStatistics");
                if (resultCustomerStatistics.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerOrderStatistics = (new JavaScriptSerializer().Deserialize<CustomerOrderStatistics>(resultCustomerStatistics.Data.ToString()));
                }
                ViewBag.customerOrderStatistics = customerOrderStatistics;

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
    }
}