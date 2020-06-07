using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
   
    [ExceptionLog]
    [Autintication]
    public class VehicleController : Controller
    {
        WebServices webServices = new WebServices();
        List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
        VehicleViewModel vehicleViewModel = new VehicleViewModel();
        readonly List<VehicleTypeViewModel> vehicleTypeViewModels = new List<VehicleTypeViewModel>();
        DriverVehicelViewModel driverVehicelViewModel = new DriverVehicelViewModel();

        public List<DriverViewModel> VehicleViewModel { get; private set; }
        public List<VehicleViewModel> VehicleViewModels { get; private set; }
        int CompanyId = 0;

        [HttpGet]
        public ActionResult Index(int CompId = 0)
        {
            ViewBag.CompanyId = CompId;
            return View();
        }

        [HttpGet]
        public ActionResult VehicleIndexToSelect(int CompId = 0)
        {
            if (CompId == 0)
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
            }
            else
            {
                CompanyId = CompId;
                ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
            }
            if (Request.IsAjaxRequest())
            {
                VehicleViewModels = new List<VehicleViewModel>();

                try
                {
                    PagingParameterModel pagingParameterModel = new PagingParameterModel
                    {
                        CompanyId = CompanyId,
                    };

                    var VehicleList = webServices.Post(pagingParameterModel, "Vehicle/All");

                    if (VehicleList.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (VehicleList.Data != "[]" && VehicleList.Data != null)
                        {
                            VehicleViewModels = (new JavaScriptSerializer().Deserialize<List<VehicleViewModel>>(VehicleList.Data.ToString()));
                        }
                    }

                    if (VehicleViewModels == null || VehicleViewModels.Count < 1)
                    {
                        VehicleViewModel vehicleViewModel = new VehicleViewModel
                        {
                            Id = 0,
                            TraficPlateNumber = "Select Vehicle"
                        };
                        VehicleViewModels.Add(vehicleViewModel);
                    }
                    else
                    {
                        VehicleViewModels.Insert(0, new VehicleViewModel() { Id = 0, TraficPlateNumber = "Select Vehicle" });
                    }
                    return Json(VehicleViewModels, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult All(int CompId = 0)
        {
            VehicleViewModels = new List<VehicleViewModel>();
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]); ;
                if (CompId > 0)
                {
                    CompanyId = CompId;
                }
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

                var VehicleList = webServices.Post(pagingParameterModel, "Vehicle/All");

                if (VehicleList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (VehicleList.Data != "[]" && VehicleList.Data != null)
                    {
                        VehicleViewModels = (new JavaScriptSerializer().Deserialize<List<VehicleViewModel>>(VehicleList.Data.ToString()));
                        TotalRow = VehicleViewModels[0].TotalRows;
                        return Json(new { draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = VehicleViewModels }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { draw, recordsFiltered = 0, recordsTotal = 0, data = VehicleViewModels }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public ActionResult ChangeStatus(VehicleViewModel vehicleViewModel)
        {
            try
            {
                var Result = new ServiceResponseModel();

                vehicleViewModel.UpdateBy = Convert.ToInt32(Session["UserId"]);
                Result = webServices.Post(vehicleViewModel, "Vehicle/ChangeStatus");

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
        public JsonResult GetAllVehicle()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var result = webServices.Post(new VehicleViewModel(), "Vehicle/All/" + CompanyId);
                if (result.Data != null)
                {
                    vehicleViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString());
                }

                return Json(vehicleViewModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Details(int id, int CompId = 0)
        {
            CompanyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
                if (CompId < 1) {

                    CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    ViewBag.LayoutName = "~/Views/Shared/_layout.cshtml";
                    
                }
                else {
                    //VehicleViewModel vehicleViewModel = new VehicleViewModel();

                    CompanyId = CompId;
                    ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
                }
                vehicleViewModel.Id = id;
                vehicleViewModel.CompanyId = CompanyId;

                var result = webServices.Post(vehicleViewModel, "Vehicle/Edit", true);
                if (result.Data != null)
                {
                    vehicleViewModel = (new JavaScriptSerializer()).Deserialize<List<VehicleViewModel>>(result.Data.ToString()).FirstOrDefault();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return View(vehicleViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {

            VehicleTypeController vehicleTypeController = new VehicleTypeController();
            ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

            VehicleViewModel vehicle = new VehicleViewModel { 
                InsuranceExpiryDate = System.DateTime.Now,
                MulkiaExpiryDate = System.DateTime.Now
            };
            return View(vehicle);
            //try
            //{
            //    var result = webServices.Post(new VehicleViewModel(), "VehicleType/GetAll");
            //    if (result.Data != null)
            //    {
            //        vehicleTypeViewModels = (new JavaScriptSerializer()).Deserialize<List<VehicleTypeViewModel>>(result.Data.ToString());
            //    }

            //    ViewBag.vehicleTypeViewModels = vehicleTypeViewModels;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}


            //return View();
        }
               
        [HttpPost]
        public ActionResult Create(VehicleViewModel vehicleViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    VehicleTypeController vehicleTypeController = new VehicleTypeController();
                    ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();
                    return View(vehicleViewModel);
                }
                else
                {

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase[] httpPostedFileBase = new HttpPostedFileBase[4];
                        if (vehicleViewModel.MulkiaFront1File != null)
                        {
                            httpPostedFileBase[0] = vehicleViewModel.MulkiaFront1File;
                        }
                        if (vehicleViewModel.MulkiaBack1File != null)
                        {
                            httpPostedFileBase[1] = vehicleViewModel.MulkiaBack1File;
                        }
                        if (vehicleViewModel.MulkiaFront2File != null)
                        {
                            httpPostedFileBase[2] = vehicleViewModel.MulkiaFront2File;
                        }
                        if (vehicleViewModel.MulkiaBack2File != null)
                        {
                            httpPostedFileBase[3] = vehicleViewModel.MulkiaBack2File;
                        }

                        var file = vehicleViewModel.MulkiaFront1File;

                        using (HttpClient client = new HttpClient())
                        {
                            using (var content = new MultipartFormDataContent())
                            {
                                if (httpPostedFileBase.ToList().Count > 0)
                                {
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (httpPostedFileBase[i] != null)
                                        {
                                            file = httpPostedFileBase[i];

                                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                            var fileContent = new ByteArrayContent(fileBytes);

                                            if (i == 0)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaFront1") { FileName = file.FileName };
                                            }
                                            else if (i == 1)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaBack1") { FileName = file.FileName };
                                            }
                                            else if (i == 2)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaFront2") { FileName = file.FileName };
                                            }
                                            else if (i == 3)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaBack2") { FileName = file.FileName };
                                            }
                                            content.Add(fileContent);
                                        }
                                    }
                                }

                                string UserId = Session["UserId"].ToString();
                                content.Add(new StringContent(UserId), "CreatedBy");
                                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                                content.Add(new StringContent(CompanyId.ToString()), "CompanyId");
                                content.Add(new StringContent("ClientDocs"), "ClientDocs");
                                content.Add(new StringContent(vehicleViewModel.VehicleType.ToString()), "VehicleType");
                                content.Add(new StringContent(vehicleViewModel.TraficPlateNumber ?? ""), "TraficPlateNumber");
                                content.Add(new StringContent(vehicleViewModel.TCNumber ?? "" ), "TCNumber");
                                content.Add(new StringContent(vehicleViewModel.Model ?? "" ), "Model");
                                content.Add(new StringContent(vehicleViewModel.Brand ?? "" ), "Brand");
                                content.Add(new StringContent(vehicleViewModel.Color ?? ""  ), "Color");
                                content.Add(new StringContent(vehicleViewModel.MulkiaExpiry ?? ""  ), "MulkiaExpiry");
                                content.Add(new StringContent(vehicleViewModel.InsuranceExpiry ?? ""  ), "InsuranceExpiry");
                                content.Add(new StringContent(vehicleViewModel.RegisteredRegion ?? "" ), "RegisteredRegion");
                                content.Add(new StringContent(vehicleViewModel.Comments ?? "" ), "Comments");

                                var result = webServices.PostMultiPart(content, "Vehicle/Add", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    if (result.Message != "\"TraficPlateNumber Already Availible\"")
                                    {
                                        return RedirectToAction(nameof(Details), new { vehicleViewModel.Id });
                                    }
                                    else
                                    {
                                        VehicleTypeController vehicleTypeController = new VehicleTypeController();
                                        ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

                                        ModelState.AddModelError("TraficPlateNumber", "Plate Number already exist chhose another");
                                        return View(vehicleViewModel);
                                    }
                                }
                                else
                                {
                                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                    {
                                        return RedirectToAction(nameof(Index));
                                    }
                                    else
                                    {
                                        VehicleTypeController vehicleTypeController = new VehicleTypeController();
                                        ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

                                        return View(vehicleViewModel);
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            using (var content = new MultipartFormDataContent())
                            {

                                string UserId = Session["UserId"].ToString();
                                content.Add(new StringContent(UserId), "CreatedBy");
                                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                                content.Add(new StringContent(CompanyId.ToString()), "CompanyId");
                                content.Add(new StringContent("ClientDocs"), "ClientDocs");
                                content.Add(new StringContent(vehicleViewModel.VehicleType.ToString()), "VehicleType");
                                content.Add(new StringContent(vehicleViewModel.TraficPlateNumber ?? "" ), "TraficPlateNumber");
                                content.Add(new StringContent(vehicleViewModel.TCNumber ?? ""), "TCNumber");
                                content.Add(new StringContent(vehicleViewModel.Model ?? ""), "Model");
                                content.Add(new StringContent(vehicleViewModel.Brand ?? ""), "Brand");
                                content.Add(new StringContent(vehicleViewModel.Color ?? ""), "Color");
                                content.Add(new StringContent(vehicleViewModel.MulkiaExpiry ?? ""  ), "MulkiaExpiry");
                                content.Add(new StringContent(vehicleViewModel.InsuranceExpiry ?? "" ), "InsuranceExpiry");
                                content.Add(new StringContent(vehicleViewModel.RegisteredRegion ?? "" ), "RegisteredRegion");
                                content.Add(new StringContent(vehicleViewModel.Comments ?? "" ), "Comments");

                                var result = webServices.PostMultiPart(content, "Vehicle/Add", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    if (result.Message != "\"TraficPlateNumber Already Availible\"")
                                    {
                                        return RedirectToAction(nameof(Details), new { vehicleViewModel.Id });
                                    }
                                    else
                                    {
                                        VehicleTypeController vehicleTypeController = new VehicleTypeController();
                                        ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

                                        ModelState.AddModelError("TraficPlateNumber", "Plate Number already exist chhose another");
                                        return View(vehicleViewModel);
                                    }
                                }
                                else
                                {
                                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                    {
                                        return RedirectToAction(nameof(Index));
                                    }
                                    else
                                    {
                                        VehicleTypeController vehicleTypeController = new VehicleTypeController();
                                        ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

                                        return View(vehicleViewModel);
                                    }
                                }

                            }
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, int CompId = 0)
        {

            CompanyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {

                if (CompId < 1)
                {

                    CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    ViewBag.LayoutName = "~/Views/Shared/_Layout.cshtml";
                }
                else
                {
                    //VehicleViewModel vehicleViewModel = new VehicleViewModel();
                    ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
                    CompanyId = CompId;
                }
                vehicleViewModel.Id = id;
                vehicleViewModel.CompanyId = CompanyId;
                var Result = webServices.Post(vehicleViewModel, "Vehicle/Edit");

                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    vehicleViewModel = (new JavaScriptSerializer().Deserialize<List<VehicleViewModel>>(Result.Data.ToString()).FirstOrDefault());
                }

                VehicleTypeController vehicleTypeController = new VehicleTypeController();
                ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();
                return View(vehicleViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(VehicleViewModel vehicleViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    if (vehicleViewModel.CompanyId != 2)
                    {
                        CompanyId = Convert.ToInt32(Session["CompanyId"]);
                        ViewBag.LayoutName = "~/Views/Shared/_Layout.cshtml";
                    }
                    else
                    {
                        //VehicleViewModel vehicleViewModel = new VehicleViewModel();
                        ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
                    }

                    VehicleTypeController vehicleTypeController = new VehicleTypeController();
                    ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

                    return View("Edit", vehicleViewModel);
                }
                else
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase[] httpPostedFileBase = new HttpPostedFileBase[4];
                        if (vehicleViewModel.MulkiaFront1File != null)
                        {
                            httpPostedFileBase[0] = vehicleViewModel.MulkiaFront1File;
                        }
                        if (vehicleViewModel.MulkiaBack1File != null)
                        {
                            httpPostedFileBase[1] = vehicleViewModel.MulkiaBack1File;
                        }
                        if (vehicleViewModel.MulkiaFront2File != null)
                        {
                            httpPostedFileBase[2] = vehicleViewModel.MulkiaFront2File;
                        }
                        if (vehicleViewModel.MulkiaBack2File != null)
                        {
                            httpPostedFileBase[3] = vehicleViewModel.MulkiaBack2File;
                        }

                        var file = vehicleViewModel.MulkiaFront1File;

                        using (HttpClient client = new HttpClient())
                        {
                            using (var content = new MultipartFormDataContent())
                            {
                                if (httpPostedFileBase.ToList().Count > 0)
                                {
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (httpPostedFileBase[i] != null)
                                        {
                                            file = httpPostedFileBase[i];

                                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                            var fileContent = new ByteArrayContent(fileBytes);

                                            if (i == 0)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaFront1") { FileName = file.FileName };
                                            }
                                            else if (i == 1)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaBack1") { FileName = file.FileName };
                                            }
                                            else if (i == 2)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaFront2") { FileName = file.FileName };
                                            }
                                            else if (i == 3)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("MulkiaBack2") { FileName = file.FileName };
                                            }
                                            content.Add(fileContent);
                                        }
                                    }
                                }

                                string UserId = Session["UserId"].ToString();
                                content.Add(new StringContent(UserId), "UpdatBy");
                                content.Add(new StringContent(vehicleViewModel.Id.ToString()), "Id");
                                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                                content.Add(new StringContent(CompanyId.ToString()), "CompanyId");
                                content.Add(new StringContent("ClientDocs"), "ClientDocs");
                                content.Add(new StringContent(vehicleViewModel.VehicleType.ToString()), "VehicleType");
                                content.Add(new StringContent(vehicleViewModel.TraficPlateNumber ?? "" ), "TraficPlateNumber");
                                content.Add(new StringContent(vehicleViewModel.TCNumber ?? ""  ), "TCNumber");
                                content.Add(new StringContent(vehicleViewModel.Model ?? "" ), "Model");
                                content.Add(new StringContent(vehicleViewModel.Brand ?? ""  ), "Brand");
                                content.Add(new StringContent(vehicleViewModel.Color ?? ""  ), "Color");
                                content.Add(new StringContent(vehicleViewModel.MulkiaExpiry ?? "" ), "MulkiaExpiry");
                                content.Add(new StringContent(vehicleViewModel.InsuranceExpiry ?? "" ), "InsuranceExpiry");
                                content.Add(new StringContent(vehicleViewModel.RegisteredRegion ?? "" ), "RegisteredRegion");
                                content.Add(new StringContent(vehicleViewModel.Comments ?? ""), "Comments");
                                                               
                                var result = webServices.PostMultiPart(content, "Vehicle/update", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                {
                                    return Redirect(nameof(Index));
                                }

                            }
                        }
                    }
                    return Redirect(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }
               
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        public List<VehicleViewModel> Vehicles()
        {
            try
            {

                PagingParameterModel pagingParameterModel = new PagingParameterModel { 
                    pageNumber = 1,
                    _pageSize = 1,
                    PageSize = 100,
                    CompanyId = CompanyId
                };

                var VehicleList = webServices.Post(pagingParameterModel, "Vehicle/All");

                if (VehicleList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    VehicleViewModels = (new JavaScriptSerializer().Deserialize<List<VehicleViewModel>>(VehicleList.Data.ToString()));
                }


                return VehicleViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public DriverVehicelViewModel DriverVehicels(int CompanyId)
        {
            SearchViewModel searchViewModel = new SearchViewModel { 
                CompanyId = CompanyId
            };

            var Result = webServices.Post(searchViewModel, "CustomerOrder/DriverandVehicellist", false);

            if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (Result.Data != "[]")
                {
                    driverVehicelViewModel = (new JavaScriptSerializer().Deserialize<DriverVehicelViewModel>(Result.Data.ToString()));
                }
            }

            return driverVehicelViewModel;
        }

        [HttpPost]
        public ActionResult VehicleByCompany()
        {
            try
            {
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var DriverVehicleLists = DriverVehicels(CompanyId);
                DriverVehicleLists.vehicleModels.Insert(0, new VehicleModel() { VehicelId = 0, TraficPlateNumber = "All Vehicles" });
                return Json(DriverVehicleLists.vehicleModels);
            }
            catch (Exception)
            {
                throw;
            }
        }
             
    }
}