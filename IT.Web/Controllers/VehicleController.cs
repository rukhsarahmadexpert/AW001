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
    [Autintication]
    public class VehicleController : Controller
    {
        WebServices webServices = new WebServices();
        List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
        VehicleViewModel vehicleViewModel = new VehicleViewModel();
        List<VehicleTypeViewModel> vehicleTypeViewModels = new List<VehicleTypeViewModel>();
        DriverVehicelViewModel driverVehicelViewModel = new DriverVehicelViewModel();

        public List<DriverViewModel> VehicleViewModel { get; private set; }
        public List<VehicleViewModel> VehicleViewModels { get; private set; }
        int CompanyId = 0;
        // GET: Vehicle
        public ActionResult Index(int CompId = 0)
        {
            if (CompId == 0)
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                ViewBag.LayoutName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                CompanyId = CompId;
                ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
            }
            try
            {
                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = 1055;
                pagingParameterModel.PageSize = 100;
                pagingParameterModel.CompanyId = CompanyId;

                var VehicleList = webServices.Post(pagingParameterModel, "Vehicle/All");

                if (VehicleList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (VehicleList.Data != "[]" && VehicleList.Data != null)
                    {
                        VehicleViewModels = (new JavaScriptSerializer().Deserialize<List<VehicleViewModel>>(VehicleList.Data.ToString()));
                    }
                }

                if (Request.IsAjaxRequest())
                {
                    VehicleViewModels.Insert(0, new VehicleViewModel() { Id = 0, TraficPlateNumber = "Select Vehicle" });
                    return Json(VehicleViewModels, JsonRequestBehavior.AllowGet);
                }

                return View(VehicleViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

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

        public ActionResult Details(int id)
        {
            try
            {
                //VehicleViewModel vehicleViewModel = new VehicleViewModel();
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                vehicleViewModel.CompanyId = CompanyId;
                vehicleViewModel.Id = id;

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

        // GET: Vehicle/Create
        public ActionResult Create()
        {

            VehicleTypeController vehicleTypeController = new VehicleTypeController();
            ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();
            return View();
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

        // POST: Vehicle/Create
        [HttpPost]
        public ActionResult Create(VehicleViewModel vehicleViewModel)
        {
            try
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
                            content.Add(new StringContent(vehicleViewModel.TraficPlateNumber == null ? "" : vehicleViewModel.TraficPlateNumber), "TraficPlateNumber");
                            content.Add(new StringContent(vehicleViewModel.TCNumber == null ? "" : vehicleViewModel.TCNumber), "TCNumber");
                            content.Add(new StringContent(vehicleViewModel.Model == null ? "" : vehicleViewModel.Model), "Model");
                            content.Add(new StringContent(vehicleViewModel.Brand == null ? "" : vehicleViewModel.Brand), "Brand");
                            content.Add(new StringContent(vehicleViewModel.Color == null ? "" : vehicleViewModel.Color), "Color");
                            content.Add(new StringContent(vehicleViewModel.MulkiaExpiry == null ? "" : vehicleViewModel.MulkiaExpiry), "MulkiaExpiry");
                            content.Add(new StringContent(vehicleViewModel.InsuranceExpiry == null ? "" : vehicleViewModel.InsuranceExpiry), "InsuranceExpiry");
                            content.Add(new StringContent(vehicleViewModel.RegisteredRegion == null ? "" : vehicleViewModel.RegisteredRegion), "RegisteredRegion");
                            content.Add(new StringContent(vehicleViewModel.Comments == null ? "" : vehicleViewModel.Comments), "Comments");



                            var result = webServices.PostMultiPart(content, "Vehicle/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Index));
                            }

                        }
                    }
                }
                return RedirectToAction(nameof(Details), new { Id = vehicleViewModel.Id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Vehicle/Edit/5
        public ActionResult Edit(int Id)
        {
            try
            {

                vehicleViewModel.Id = Id;
                vehicleViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
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
                            content.Add(new StringContent(vehicleViewModel.TraficPlateNumber == null ? "" : vehicleViewModel.TraficPlateNumber), "TraficPlateNumber");
                            content.Add(new StringContent(vehicleViewModel.TCNumber == null ? "" : vehicleViewModel.TCNumber), "TCNumber");
                            content.Add(new StringContent(vehicleViewModel.Model == null ? "" : vehicleViewModel.Model), "Model");
                            content.Add(new StringContent(vehicleViewModel.Brand == null ? "" : vehicleViewModel.Brand), "Brand");
                            content.Add(new StringContent(vehicleViewModel.Color == null ? "" : vehicleViewModel.Color), "Color");
                            content.Add(new StringContent(vehicleViewModel.MulkiaExpiry == null ? "" : vehicleViewModel.MulkiaExpiry), "MulkiaExpiry");
                            content.Add(new StringContent(vehicleViewModel.InsuranceExpiry == null ? "" : vehicleViewModel.InsuranceExpiry), "InsuranceExpiry");
                            content.Add(new StringContent(vehicleViewModel.RegisteredRegion == null ? "" : vehicleViewModel.RegisteredRegion), "RegisteredRegion");
                            content.Add(new StringContent(vehicleViewModel.Comments == null ? "" : vehicleViewModel.Comments), "Comments");



                            var result = webServices.PostMultiPart(content, "Vehicle/update", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Index));
                            }

                        }
                    }
                }
                return RedirectToAction(nameof(Details), new { Id = vehicleViewModel.Id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Vehicle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vehicle/Delete/5
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

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = 0;
                pagingParameterModel.PageSize = 100;
                pagingParameterModel.CompanyId = CompanyId;

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

        public DriverVehicelViewModel DriverVehicels(int CompanyId)
        {
            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.CompanyId = CompanyId;
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
                DriverVehicleLists.vehicleModels.Insert(0, new VehicleModel() { VehicelId = 0, TraficPlateNumber = "All" });
                return Json(DriverVehicleLists.vehicleModels);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}