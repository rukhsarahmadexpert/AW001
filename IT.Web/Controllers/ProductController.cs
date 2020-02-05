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
    public class ProductController : Controller
    {
        WebServices webServices = new WebServices();
        List<ProductViewModel> productViewModels = new List<ProductViewModel>();
        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        ProductViewModel ProductViewModel = new ProductViewModel();

        //int CompanyId;

        public ActionResult Index()
        {
            try
            {
                var productList = webServices.Post(new ProductViewModel(), "Product/All");

                if (productList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productViewModels = (new JavaScriptSerializer().Deserialize<List<ProductViewModel>>(productList.Data.ToString()));
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(productViewModels, JsonRequestBehavior.AllowGet);
                }
                return View(productViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
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
            productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Product Unit" });
            ViewBag.productUnitViewModels = productUnitViewModels;

            return View(new ProductViewModel());
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            try
            {
                var productResult = new ServiceResponseModel();
                if (productViewModel.Id < 1)
                {
                    productViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    productResult = webServices.Post(productViewModel, "Product/Add");
                }
                else
                {
                    productViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                    productResult = webServices.Post(productViewModel, "Product/Update");
                }

                if (productResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(productResult.Data));
                    return RedirectToAction(nameof(Index));
                }

                return View(productViewModels);
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
                var productResult = webServices.Post(new ProductViewModel(), "Product/Edit/" + Id);

                if (productResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    ProductViewModel = (new JavaScriptSerializer().Deserialize<ProductViewModel>(productResult.Data.ToString()));
                }
                var producUnittList = webServices.Post(new ProductViewModel(), "ProductUnit/All");

                if (producUnittList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productUnitViewModels = (new JavaScriptSerializer().Deserialize<List<ProductUnitViewModel>>(producUnittList.Data.ToString()));
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(ProductViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.productUnitViewModels = productUnitViewModels;
                    return View("Create", ProductViewModel);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [NonAction]
        public List<ProductViewModel> Products()
        {
            try
            {
                var productList = webServices.Post(new ProductViewModel(), "Product/All");

                if (productList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productViewModels = (new JavaScriptSerializer().Deserialize<List<ProductViewModel>>(productList.Data.ToString()));
                    productViewModels.Insert(0, new ProductViewModel() { Id = 0, Name = "select Product" });
                }
                return productViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}