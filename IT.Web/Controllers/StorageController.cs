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
    public class StorageController : Controller
    {
        WebServices webServices = new WebServices();
        List<StorageViewModel> storageViewModels = new List<StorageViewModel>();
        StorageViewModel StorageViewModel = new StorageViewModel();

        public ActionResult Index()
        {
            try
            {
                List<StorageViewModel> storageViewModels2 = new List<StorageViewModel>();

                PagingParameterModel pagingParameterModel = new PagingParameterModel();
                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.Id = 0;
                pagingParameterModel.PageSize = 100;

                var StorageList = webServices.Post(pagingParameterModel, "Storage/All");

                if (StorageList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    storageViewModels = (new JavaScriptSerializer().Deserialize<List<StorageViewModel>>(StorageList.Data.ToString()));

                    if (storageViewModels.Count > 0)
                    {
                        StorageViewModel storageViewModelObj = new StorageViewModel();

                        foreach (var item in storageViewModels)
                        {
                            if (item.Action == true)
                            {
                                storageViewModelObj.Id = item.Id;
                                storageViewModelObj.StockIn = item.StockIn;
                                storageViewModelObj.From = item.Source.ToLower() == "site" ? item.SiteName : item.TrafficPlateNumber;
                                storageViewModelObj.Source = item.Source;
                                storageViewModelObj.UserName = item.UserName;

                            }
                            else
                            {
                                storageViewModelObj.StockOut = item.StockOut;
                                storageViewModelObj.To = item.Source.ToLower() == "site" ? item.SiteName : item.TrafficPlateNumber;
                                storageViewModelObj.ToSource = item.Source;

                                storageViewModels2.Add(storageViewModelObj);
                                storageViewModelObj = new StorageViewModel();
                            }
                        }
                    }

                    return View(storageViewModels2);
                }
                return View(storageViewModels);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StorageViewListModel storageViewListModel)
        {
            try
            {
                var value = DateTime.Now.ToFileTime().ToString();

                List<StorageViewModel> storageViewModels1 = new List<StorageViewModel>();
                storageViewModels1 = storageViewListModel.storageViewModels;

                storageViewModels1[0].CreatedBy = Convert.ToInt32(Session["UserId"]);
                storageViewModels1[1].CreatedBy = Convert.ToInt32(Session["UserId"]);
                storageViewModels1[0].uniques = value;
                storageViewModels1[1].uniques = value;
                var result = webServices.Post(storageViewModels1, "Storage/StorageAdd");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int k = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                StorageViewModel.Id = Id;
                var result = webServices.Post(StorageViewModel, "Storage/Edit");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        storageViewModels = (new JavaScriptSerializer().Deserialize<List<StorageViewModel>>(result.Data.ToString()));
                    }
                }
                ProductController productController = new ProductController();
                SiteController siteController = new SiteController();
                VehicleController vehicleController = new VehicleController();
                AwfVehicleController awfVehicleController = new AwfVehicleController();
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);


                ViewBag.AdminVehicles = awfVehicleController.AdminVehicles();
                ViewBag.Vehicles = vehicleController.Vehicles();
                ViewBag.Sites = siteController.SitesList(CompanyId);
                ViewBag.Products = productController.Products();
                return View(storageViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(StorageViewListModel storageViewListModel)
        {
            try
            {
                // var value = DateTime.Now.ToFileTime().ToString();

                List<StorageViewModel> storageViewModels1 = new List<StorageViewModel>();
                storageViewModels1 = storageViewListModel.storageViewModels;

                storageViewModels1[0].UpdateBy = Convert.ToInt32(Session["UserId"]);
                storageViewModels1[1].UpdateBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(storageViewModels1, "Storage/StorageUpdate");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int k = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }
    }
}