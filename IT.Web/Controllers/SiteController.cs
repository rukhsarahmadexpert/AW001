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
    public class SiteController : Controller
    {
        WebServices webServices = new WebServices();
        readonly List<DriverViewModel> driverViewModels = new List<DriverViewModel>();
        List<SiteViewModel> siteViewModels = new List<SiteViewModel>();
        readonly DriverViewModel driverViewModel = new DriverViewModel();
        int CompanyId;

        public SiteViewModel SiteViewModel { get; private set; }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View();
                //CompanyId = Convert.ToInt32(Session["CompanyId"]);

                //PagingParameterModel pagingParameterModel = new PagingParameterModel
                //{
                //    pageNumber = 1,
                //    _pageSize = 1,
                //    CompanyId = CompanyId,
                //    PageSize = 100,
                //};
                //var SiteList = webServices.Post(pagingParameterModel, "Site/All");
                //if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                //{
                //    siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SiteList.Data.ToString()));
                //}
                //if (Request.IsAjaxRequest())
                //{

                //    if (siteViewModels == null || siteViewModels.Count < 1)
                //    {
                //        SiteViewModel siteViewModel = new SiteViewModel
                //        {
                //            Id = 0,
                //            SiteName= "Select Site"

                //        };

                //        siteViewModels.Add(siteViewModel);
                //    }
                //    else
                //    {
                //        siteViewModels.Insert(0, new SiteViewModel() { Id = 0, SiteName = "Select Site" });
                //    }
                //    return Json(siteViewModels, JsonRequestBehavior.AllowGet);

                   
                //}
                //return View(siteViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult SiteSelectList()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    _pageSize = 1,
                    CompanyId = CompanyId,
                    PageSize = 100,
                };
                var SiteList = webServices.Post(pagingParameterModel, "Site/All");
                if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SiteList.Data.ToString()));
                }
                if (Request.IsAjaxRequest())
                {

                    if (siteViewModels == null || siteViewModels.Count < 1)
                    {
                        SiteViewModel siteViewModel = new SiteViewModel
                        {
                            Id = 0,
                            SiteName = "Select Site"

                        };

                        siteViewModels.Add(siteViewModel);
                    }
                    else
                    {
                        siteViewModels.Insert(0, new SiteViewModel() { Id = 0, SiteName = "Select Site" });
                    }
                    return Json(siteViewModels, JsonRequestBehavior.AllowGet);


                }
                return View(siteViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult All()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" +
                Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                string search = Request.Form.GetValues("search[value]")[0];
                //int skip = start != null ? Convert.ToInt32(start) : 0;

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                int pageNumer = (Convert.ToInt32(start) / Convert.ToInt32(length)) + 1;
                pagingParameterModel.pageNumber = pageNumer;
                pagingParameterModel._pageSize = pageSize;
                pagingParameterModel.PageSize = pageSize;
                pagingParameterModel.CompanyId = CompanyId;

                var VehicleList = webServices.Post(pagingParameterModel, "Site/All");

                if (VehicleList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (VehicleList.Data != "[]" && VehicleList.Data != null)
                    {
                        siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(VehicleList.Data.ToString()));

                        TotalRow = siteViewModels[0].TotalRows;

                        return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = siteViewModels }, JsonRequestBehavior.AllowGet);
                        //compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                    }
                }
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = siteViewModels }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ChangeStatus(SiteViewModel siteViewModel)
        {
            try
            {
                var Result = new ServiceResponseModel();

                siteViewModel.UpdateBy = Convert.ToInt32(Session["UserId"]);
                Result = webServices.Post(siteViewModel, "Site/ChangeStatus");

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
            return View(new SiteViewModel());
        }

        [HttpPost]
        public ActionResult Create(SiteViewModel siteViewModel)
        {
            try
            {
                var SiteResult = new ServiceResponseModel();
                if (siteViewModel.Id < 1)
                {
                    siteViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    siteViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    SiteResult = webServices.Post(siteViewModel, "Site/Add");
                }
                else
                {
                    siteViewModel.UpdateBy = Convert.ToInt32(Session["UserId"]);
                    siteViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    SiteResult = webServices.Post(siteViewModel, "Site/Update");
                }

                if (SiteResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<SiteViewModel>(SiteResult.Data));

                    if(reuslt.SiteName == "Already site exist")
                    {
                        ModelState.AddModelError("SiteName","Site Name already exists");
                        return View(siteViewModel);
                    }

                    return RedirectToAction(nameof(Index));
                }

                return View(siteViewModels);
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
                var SiteResult = webServices.Post(new SiteViewModel(), "Site/Edit/" + Id);

                if (SiteResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    SiteViewModel = (new JavaScriptSerializer().Deserialize<SiteViewModel>(SiteResult.Data.ToString()));
                }

                if (Request.IsAjaxRequest()){

                    return Json(SiteViewModel, JsonRequestBehavior.AllowGet);
                }                
                return View("Create", SiteViewModel);
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
                var SiteResult = webServices.Post(new SiteViewModel(), "Site/Edit/" + Id);

                if (SiteResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    SiteViewModel = (new JavaScriptSerializer().Deserialize<SiteViewModel>(SiteResult.Data.ToString()));
                }
                return View(SiteViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [NonAction]
        public List<SiteViewModel> SitesList(int Id)
        {
            try
            {
                List<SiteViewModel> siteViewModels1 = new List<SiteViewModel>();
                PagingParameterModel pagingParameterModel = new PagingParameterModel
                { 
                    pageNumber = 1,
                    _pageSize = 1,
                    CompanyId = Id,
                    PageSize = 100,
                };
                var SiteList = webServices.Post(pagingParameterModel, "Site/All");
                if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    siteViewModels1 = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SiteList.Data.ToString()));
                }
                return siteViewModels1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}