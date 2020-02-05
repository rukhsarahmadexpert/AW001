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
    public class VenderController : Controller
    {
        WebServices webServices = new WebServices();
        VenderViewModel venderViewModel = new VenderViewModel();
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        int CompanyId;
        // GET: Vender
        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    CompanyId = CompanyId,
                    PageSize = 100,
                };

                var DriverList = webServices.Post(pagingParameterModel, "Vender/All");
                if (DriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    venderViewModels = (new JavaScriptSerializer().Deserialize<List<VenderViewModel>>(DriverList.Data.ToString()));
                }

                return View(venderViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Create()
        {

            CountryController countryController = new CountryController();

            ViewBag.Countries = countryController.Countries();
            return View(new VenderViewModel());
        }

        [HttpPost]
        public ActionResult Create(VenderViewModel venderViewModel)
        {
            try
            {
                var venderResult = new ServiceResponseModel();
                if (venderViewModel.Id < 1)
                {
                    venderViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    venderResult = webServices.Post(venderViewModel, "Vender/Add");
                }
                else
                {
                    venderViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                    venderResult = webServices.Post(venderViewModel, "Vender/Update");
                }
                if (venderResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(venderResult.Data));
                    return RedirectToAction(nameof(Index));
                }

                return View(venderViewModel);
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
                var venderResult = webServices.Post(new VenderViewModel(), "Vender/Edit/" + Id);

                if (venderResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    venderViewModel = (new JavaScriptSerializer().Deserialize<VenderViewModel>(venderResult.Data.ToString()));
                }


                if (Request.IsAjaxRequest())
                {
                    return Json(venderViewModel, JsonRequestBehavior.AllowGet);
                }

                CountryController countryController = new CountryController();

                ViewBag.Countries = countryController.Countries();

                return View("Create", venderViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}