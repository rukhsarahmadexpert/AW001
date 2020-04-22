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
    public class VenderController : Controller
    {
        WebServices webServices = new WebServices();
        VenderViewModel venderViewModel = new VenderViewModel();
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        int CompanyId;

        [HttpGet]
        public ActionResult Index()
        {
            return View();            
        }

        [HttpPost]
        public ActionResult All()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" +
                Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                string search = Request.Form.GetValues("search[value]")[0]; 

                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                PagingParameterModel pagingParameterModel = new PagingParameterModel();
                pagingParameterModel.CompanyId = CompanyId;

                if (Convert.ToInt32(start) == 0)
                {
                    pagingParameterModel.pageNumber = 1;
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;                   
                }
                else
                {
                    pagingParameterModel.pageNumber = Convert.ToInt32(draw);
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
                }

                var DriverList = webServices.Post(pagingParameterModel, "Vender/All");
                int TotalRow = 0;
                if (DriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (DriverList.Data != "[]")
                    {
                        venderViewModels = (new JavaScriptSerializer().Deserialize<List<VenderViewModel>>(DriverList.Data.ToString()));
                        TotalRow = venderViewModels.Count;
                    }
                    return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = venderViewModels }, JsonRequestBehavior.AllowGet);
                }                
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = venderViewModels }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult ChangeStatus(VenderViewModel venderViewModel)
        {
            try
            {
                var Result = new ServiceResponseModel();

                venderViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                Result = webServices.Post(venderViewModel, "Vender/ChangeStatus");

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

        [HttpGet]
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
                if (!ModelState.IsValid)
                {
                    CountryController countryController = new CountryController();
                    ViewBag.Countries = countryController.Countries();

                    return View(venderViewModel);
                }
                else
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
                    if (Request.IsAjaxRequest())
                    {
                        return Json(venderViewModels, JsonRequestBehavior.AllowGet);
                    }

                    return View(venderViewModel);
                }
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

        [HttpGet]
        public ActionResult Details(int Id)
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

                return View(venderViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [NonAction]
        public List<VenderViewModel> Venders()
        {
            var venderViewModels1 = new List<VenderViewModel>();

            var Res = webServices.Post(new DriverViewModel(), "Vender/All");
            if (Res.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (Res.Data != "[]")
                {
                    venderViewModels1 = (new JavaScriptSerializer()).Deserialize<List<VenderViewModel>>(Res.Data.ToString());
                    venderViewModels1.Insert(0, new VenderViewModel() { Id = 0, Name = "Select Vender" });
                }
            }
            return venderViewModels1;
        }


        [HttpPost]
        public ActionResult VenderList()
        {
            try
            {
                var vendersResult = Venders();
                return Json(vendersResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Falied", JsonRequestBehavior.AllowGet);
            }
        }
    }
}