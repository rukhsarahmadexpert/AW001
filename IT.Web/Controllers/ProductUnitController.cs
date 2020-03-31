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
    [Autintication]
    public class ProductUnitController : Controller
    {
        WebServices webServices = new WebServices();
        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        ProductUnitViewModel ProductUnitViewModel = new ProductUnitViewModel();

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var productList = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");

                if (productList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productUnitViewModels = (new JavaScriptSerializer().Deserialize<List<ProductUnitViewModel>>(productList.Data.ToString()));
                }
                return View(productUnitViewModels);
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
                var productResult = webServices.Post(new ProductUnitViewModel(), "ProductUnit/Edit/" + Id);

                if (productResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    ProductUnitViewModel = (new JavaScriptSerializer().Deserialize<ProductUnitViewModel>(productResult.Data.ToString()));
                }

                return View(ProductUnitViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductUnitViewModel());
        }
                
        [HttpPost]
        public ActionResult Create(ProductUnitViewModel productUnitViewModel)
        {
            try
            {
                var productResult = new ServiceResponseModel();
                if (productUnitViewModel.Id < 1)
                {
                    productUnitViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    productResult = webServices.Post(productUnitViewModel, "ProductUnit/Add");
                }
                else
                {
                    productUnitViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                    productResult = webServices.Post(productUnitViewModel, "ProductUnit/Update");
                }

                if (productResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(productResult.Data));

                    return RedirectToAction(nameof(Index));
                }

                return View(productUnitViewModel);
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
                var productResult = webServices.Post(new ProductUnitViewModel(), "ProductUnit/Edit/" + Id);

                if (productResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    ProductUnitViewModel = (new JavaScriptSerializer().Deserialize<ProductUnitViewModel>(productResult.Data.ToString()));
                }

                return View("Create", ProductUnitViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [NonAction]
        public List<ProductUnitViewModel> ProductUnits()
        {
      
            try
            {

                var ProductUnitList = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");
                if (ProductUnitList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    productUnitViewModels = (new JavaScriptSerializer().Deserialize<List<ProductUnitViewModel>>(ProductUnitList.Data.ToString()));
                    productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Please select Unit" });
                }
                return productUnitViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}