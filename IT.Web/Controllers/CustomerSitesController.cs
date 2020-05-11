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
    public class CustomerSitesController : Controller
    {
        WebServices webServices = new WebServices();
        List<SiteViewModel> siteViewModels = new List<SiteViewModel>();
        SiteViewModel siteViewModel = new SiteViewModel();
        int CompanyId;

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
                //    PageSize = 100
                //};

                //var SiteList = webServices.Post(pagingParameterModel, "CustomerSites/SiteAllCustomer");

                //if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                //{
                //    siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SiteList.Data.ToString()));
                //}
                //return View(siteViewModels);

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

                if (Convert.ToInt32(start) == 0)
                {
                    pagingParameterModel.pageNumber = 1;
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
                    pagingParameterModel.CompanyId = CompanyId;
                }
                else
                {
                    pagingParameterModel.pageNumber = Convert.ToInt32(draw);
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.CompanyId = CompanyId;
                }

                var SitesList = webServices.Post(pagingParameterModel, "CustomerSites/SiteAllCustomer");

                if (SitesList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (SitesList.Data != "[]" && SitesList.Data != null)
                    {
                        siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SitesList.Data.ToString()));

                        TotalRow = siteViewModels.Count;

                        return Json(new { draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = siteViewModels }, JsonRequestBehavior.AllowGet);
                        //compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                    }
                }
                return Json(new { draw, recordsFiltered = 0, recordsTotal = 0, data = siteViewModels }, JsonRequestBehavior.AllowGet);

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
                Result = webServices.Post(siteViewModel, "CustomerSites/ChangeStatus");

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
                if (!ModelState.IsValid)
                {
                    return View(siteViewModel);
                }
                else
                {
                    var SiteResult = new ServiceResponseModel();
                    if (siteViewModel.Id < 1)
                    {
                        siteViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                        siteViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                        SiteResult = webServices.Post(siteViewModel, "CustomerSites/Add");
                    }
                    else
                    {
                        siteViewModel.UpdateBy = Convert.ToInt32(Session["UserId"]);
                        siteViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                        SiteResult = webServices.Post(siteViewModel, "CustomerSites/Update");
                    }
                    if (SiteResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var reuslt = (new JavaScriptSerializer().Deserialize<int>(SiteResult.Data));
                        return RedirectToAction(nameof(Index));
                    }

                    return View(siteViewModels);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public List<SiteViewModel> SitesAll(int CompId)
        {
            try
            {
                List<SiteViewModel> siteViewModels = new List<SiteViewModel>();
                PagingParameterModel pagingParameterModel = new PagingParameterModel
                { 
                    pageNumber = 1,
                    _pageSize = 1,
                    CompanyId = CompId,
                    PageSize = 100,
                };
                var SiteList = webServices.Post(pagingParameterModel, "CustomerSites/SiteAllCustomer");
                if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (SiteList.Data != "[]")
                    {
                        siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SiteList.Data.ToString()));
                    }
                }
                siteViewModels.Insert(0, new SiteViewModel() { Id = 0, SiteName = "Select Delivery Sites" });
                return siteViewModels;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {
                var result = webServices.Post(new SiteViewModel(), "/CustomerSites/CustomerSiteById/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        siteViewModel = (new JavaScriptSerializer().Deserialize<SiteViewModel>(result.Data.ToString()));
                    }
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(siteViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View("Create", siteViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            try
            {
                var result = webServices.Post(new SiteViewModel(), "/CustomerSites/CustomerSiteById/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        siteViewModel = (new JavaScriptSerializer().Deserialize<SiteViewModel>(result.Data.ToString()));
                    }
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(siteViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View(siteViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}