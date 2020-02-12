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
    public class DesignationController : Controller
    {
        WebServices webServices = new WebServices();
        List<DesignationViewModel> designationViewModels = new List<DesignationViewModel>();
        DesignationViewModel DesignationViewModel = new DesignationViewModel();

       [HttpGet]
        public ActionResult Index()
        {
            try
            {
                if (HttpContext.Cache["DesignationData"] != null)
                {
                    designationViewModels = HttpContext.Cache["DesignationData"] as List<DesignationViewModel>;
                }
                else
                {

                    var result = webServices.Post(new DesignationViewModel(), "Designation/All");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        designationViewModels = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(result.Data.ToString());

                        HttpContext.Cache["DesignationData"] = designationViewModels;
                    }
                }

                return View(designationViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new DesignationViewModel());
        }
        
        [HttpPost]
        public ActionResult Create(DesignationViewModel designationViewModel)
        {
            try
            {
                var result = webServices.Post(designationViewModel, "Designation/Add");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("DesignationData");

                    int k = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
                
        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            try
            {
                var result = webServices.Post(new DesignationViewModel(), "Designation/Edit/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        DesignationViewModel = (new JavaScriptSerializer()).Deserialize<List<DesignationViewModel>>(result.Data.ToString()).FirstOrDefault();
                    }
                    return View(DesignationViewModel);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
               
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var result = webServices.Post("", "Designation/Delete/" + Id);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        if (Res > 0)
                        {
                            HttpContext.Cache.Remove("DesignationData");

                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                return Json("Failed", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(DesignationViewModel designationViewModel)
        {
            try
            {
                designationViewModel.CreatedBy = 1;

                var result = webServices.Post(designationViewModel, "Designation/Update");
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    HttpContext.Cache.Remove("DesignationData");

                    int k = (new JavaScriptSerializer()).Deserialize<int>(result.Data);
                    return View(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public List<DesignationViewModel> Designations()
        {
            try
            {
                var DesignationList = webServices.Post(new DesignationViewModel(), "Designation/All");

                if (DesignationList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    designationViewModels = (new JavaScriptSerializer().Deserialize<List<DesignationViewModel>>(DesignationList.Data.ToString()));
                  
                }
                return designationViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}