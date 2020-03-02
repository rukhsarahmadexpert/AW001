using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web_New.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Controllers
{
    public class CustomerBookingController : Controller
    {
        WebServices webServices = new WebServices();
        List<CustomerBookingViewModel> customerBookingViewModels = new List<CustomerBookingViewModel>();
        CustomerBookingViewModel customerBookingViewModel = new CustomerBookingViewModel();
        int CompanyId;
        // GET: CustomerBooking
        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    CompanyId = 0,
                    PageSize = 10
                };

                var CustomerBookingList = webServices.Post(pagingParameterModel, "CustomerBooking/All");

                if (CustomerBookingList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerBookingViewModel>>(CustomerBookingList.Data.ToString()));
                }

                return View(customerBookingViewModels);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerBookingAdd(CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                var CustomerResult = new ServiceResponseModel();
                customerBookingViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                customerBookingViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                CustomerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingAdd");
              
                if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(CustomerResult.Data));

                    return RedirectToAction(nameof(Index));
                }

                return View(customerBookingViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AdminCreate()
        {
            CompanyController companyController = new CompanyController();
            ProductController productController = new ProductController();
            ProductUnitController productUnitController = new ProductUnitController();

            ViewBag.Companies = companyController.Companies();
            ViewBag.Products = productController.Products();
            ViewBag.ProductUnits = productUnitController.ProductUnits();

            return View();
        }


        [HttpGet]
        public ActionResult CustomerBookingGetById(int Id)
        {
            try
            {
                var CustomerResult = webServices.Post(new CustomerBookingViewModel(), "CustomerBooking/CustomerBookingGetById/" + Id);

                if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModel = (new JavaScriptSerializer().Deserialize<CustomerBookingViewModel>(CustomerResult.Data.ToString()));
                }

                CompanyController companyController = new CompanyController();
                ProductController productController = new ProductController();
                ProductUnitController productUnitController = new ProductUnitController();

                ViewBag.Companies = companyController.Companies();
                ViewBag.Products = productController.Products();
                ViewBag.ProductUnits = productUnitController.ProductUnits();

                return View(customerBookingViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult Customer()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    CompanyId = CompanyId,
                    PageSize = 10
                };

                var CustomerBookingList = webServices.Post(pagingParameterModel, "CustomerBooking/All");

                if (CustomerBookingList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerBookingViewModel>>(CustomerBookingList.Data.ToString()));
                }
                return View(customerBookingViewModels);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}