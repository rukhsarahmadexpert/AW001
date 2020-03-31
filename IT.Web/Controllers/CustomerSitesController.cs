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
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    _pageSize = 1,
                    CompanyId = CompanyId,
                    PageSize = 100
                };

                var SiteList = webServices.Post(pagingParameterModel, "CustomerSites/SiteAllCustomer");

                if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    siteViewModels = (new JavaScriptSerializer().Deserialize<List<SiteViewModel>>(SiteList.Data.ToString()));
                }
                return View(siteViewModels);

            }
            catch (Exception ex)
            {
                throw ex;
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