using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Controllers
{
    public class FuelPricesController : Controller
    {
        WebServices webServices = new WebServices();
        List<FuelPricesViewModel> fuelPricesViewModels = new List<FuelPricesViewModel>();
        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        FuelPricesViewModel FuelPricesViewModel = new FuelPricesViewModel();

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var fuelPriceList = webServices.Post(new ProductViewModel(), "FuelPrices/All");

                if (fuelPriceList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    fuelPricesViewModels = (new JavaScriptSerializer().Deserialize<List<FuelPricesViewModel>>(fuelPriceList.Data.ToString()));
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(fuelPricesViewModels, JsonRequestBehavior.AllowGet);
                }
                return View(fuelPricesViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult ChangeStatus(FuelPricesViewModel fuelPricesViewModel)
        {
            try
            {
                var Result = new ServiceResponseModel();

                fuelPricesViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                Result = webServices.Post(fuelPricesViewModel, "FuelPrices/FuelPriceUpdateStatus");

                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(Result.Data));


                }
                return Json("success", JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Create()
        {
            
            try
            {
                var producUnittList = webServices.Post(new ProductViewModel(), "ProductUnit/All");

                if (producUnittList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productUnitViewModels = (new JavaScriptSerializer().Deserialize<List<ProductUnitViewModel>>(producUnittList.Data.ToString()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
            ViewBag.productUnitViewModels = productUnitViewModels;
            return View(new FuelPricesViewModel());
        }


        [HttpPost]
        public ActionResult Create(FuelPricesViewModel fuelPricesViewModel)
        {
            try
            {
                var priceResult = new ServiceResponseModel();
                if (fuelPricesViewModel.Id < 1)
                {
                    fuelPricesViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    priceResult = webServices.Post(fuelPricesViewModel, "FuelPrices/Add");
                }
                else
                {
                    fuelPricesViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                    priceResult = webServices.Post(fuelPricesViewModel, "FuelPrices/Update");
                }

                if (priceResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(priceResult.Data));
                    return RedirectToAction(nameof(Index));
                }

                return View(fuelPricesViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {
                FuelPricesViewModel.Id = Id;
                var FuelResult = webServices.Post(FuelPricesViewModel, "FuelPrices/Edit");

                if (FuelResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    FuelPricesViewModel = (new JavaScriptSerializer().Deserialize<FuelPricesViewModel>(FuelResult.Data.ToString()));
                }
                var producUnittList = webServices.Post(new ProductViewModel(), "ProductUnit/All");

                if (producUnittList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productUnitViewModels = (new JavaScriptSerializer().Deserialize<List<ProductUnitViewModel>>(producUnittList.Data.ToString()));
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(FuelPricesViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.productUnitViewModels = productUnitViewModels;
                    return View("Create", FuelPricesViewModel);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpGet]
        public ActionResult Details(int Id)
        {
            try
            {
                FuelPricesViewModel.Id = Id;
                var FuelResult = webServices.Post(FuelPricesViewModel, "FuelPrices/Edit");

                if (FuelResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    FuelPricesViewModel = (new JavaScriptSerializer().Deserialize<FuelPricesViewModel>(FuelResult.Data.ToString()));
                }
                var producUnittList = webServices.Post(new ProductViewModel(), "ProductUnit/All");

                if (producUnittList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productUnitViewModels = (new JavaScriptSerializer().Deserialize<List<ProductUnitViewModel>>(producUnittList.Data.ToString()));
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(FuelPricesViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.productUnitViewModels = productUnitViewModels;
                    return View(FuelPricesViewModel);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}