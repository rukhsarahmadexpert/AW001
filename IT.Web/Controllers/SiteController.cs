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
    public class SiteController : Controller
    {
        WebServices webServices = new WebServices();
        List<DriverViewModel> driverViewModels = new List<DriverViewModel>();
        List<SiteViewModel> siteViewModels = new List<SiteViewModel>();
        DriverViewModel driverViewModel = new DriverViewModel();
        int CompanyId;

        public SiteViewModel SiteViewModel { get; private set; }

        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = CompanyId;
                pagingParameterModel.PageSize = 100;

                var SiteList = webServices.Post(pagingParameterModel, "Site/All");

                if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SiteList.Data.ToString()));
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(siteViewModels, JsonRequestBehavior.AllowGet);
                }
                return View(siteViewModels);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(SiteResult.Data));

                    return RedirectToAction(nameof(Index));
                }

                return View(siteViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                var SiteResult = webServices.Post(new SiteViewModel(), "Site/Edit/" + Id);

                if (SiteResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    SiteViewModel = (new JavaScriptSerializer().Deserialize<SiteViewModel>(SiteResult.Data.ToString()));
                }
                return View("Create", SiteViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

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
                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = Id;
                pagingParameterModel.PageSize = 100;

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