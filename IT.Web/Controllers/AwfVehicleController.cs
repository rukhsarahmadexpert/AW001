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
    [ExceptionLog]
    public class AwfVehicleController : Controller
    {
        WebServices webServices = new WebServices();
        readonly List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
        VehicleViewModel vehicleViewModel = new VehicleViewModel();
        readonly List<VehicleTypeViewModel> vehicleTypeViewModels = new List<VehicleTypeViewModel>();

        public List<DriverViewModel> VehicleViewModel { get; private set; }
        public List<VehicleViewModel> VehicleViewModels { get; private set; }
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
                    PageSize = 100,
                };

                var VehicleList = webServices.Post(pagingParameterModel, "AWFVehicle/All");

                if (VehicleList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    VehicleViewModels = (new JavaScriptSerializer().Deserialize<List<VehicleViewModel>>(VehicleList.Data.ToString()));
                }
                if (Request.IsAjaxRequest())
                {
                    if (vehicleViewModels == null || vehicleViewModels.Count < 1)
                    {
                        VehicleViewModel vehicleViewModel = new VehicleViewModel
                        {
                            Id = 0,
                            TraficPlateNumber = "Select vehicle"

                        };

                        VehicleViewModels.Add(vehicleViewModel);
                    }
                    else
                    {
                        VehicleViewModels.Insert(0, new VehicleViewModel() { Id = 0, TraficPlateNumber = "Select Vehicle" });
                    }
                    return Json(VehicleViewModels, JsonRequestBehavior.AllowGet);
                }

                return View(VehicleViewModels);
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
                Result = webServices.Post(vehicleViewModel, "AWFVehicle/ChangeStatus");

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
            VehicleTypeController vehicleTypeController = new VehicleTypeController();
            ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();
            return View(new VehicleViewModel());
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
                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
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


                                if (httpPostedFileBase.ToList().Count > 0)
                                {
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (httpPostedFileBase[i] != null)
                                        {
                                            var file = vehicleViewModel.MulkiaFront1File;

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
                            }

                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId ?? "1"), "CreatedBy");
                            CompanyId = Convert.ToInt32(Session["CompanyId"]);
                            content.Add(new StringContent(CompanyId.ToString()), "CompanyId");
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            content.Add(new StringContent(vehicleViewModel.VehicleType.ToString()), "VehicleType");
                            content.Add(new StringContent(vehicleViewModel.TraficPlateNumber ?? ""), "TraficPlateNumber");
                            content.Add(new StringContent(vehicleViewModel.TCNumber ?? ""), "TCNumber");
                            content.Add(new StringContent(vehicleViewModel.Model ?? ""), "Model");
                            content.Add(new StringContent(vehicleViewModel.Brand ?? ""), "Brand");
                            content.Add(new StringContent(vehicleViewModel.Color ?? ""), "Color");
                            content.Add(new StringContent(vehicleViewModel.MulkiaExpiry ?? ""), "MulkiaExpiry");
                            content.Add(new StringContent(vehicleViewModel.InsuranceExpiry ?? ""), "InsuranceExpiry");
                            content.Add(new StringContent(vehicleViewModel.RegisteredRegion ?? ""), "RegisteredRegion");
                            content.Add(new StringContent(vehicleViewModel.Comments ?? ""), "Comments");

                            var result = webServices.PostMultiPart(content, "AWFVehicle/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                if (result.Data != "TraficPlateNumber Already Availible")
                                {
                                    VehicleTypeController vehicleTypeController = new VehicleTypeController();
                                    ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

                                    ModelState.AddModelError("TraficPlateNumber", "TraficPlateNumber Already Availible, Choose another");

                                    return View(vehicleViewModel);
                                }

                                else
                                {
                                    return RedirectToAction(nameof(Index));
                                }

                            }
                            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                VehicleTypeController vehicleTypeController = new VehicleTypeController();
                                ViewBag.VehicleTypes = vehicleTypeController.VehicleTypes();

                                ModelState.AddModelError("TraficPlateNumber", "TraficPlateNumber Already Availible, Choose another");

                                return View(vehicleViewModel);
                            }
                            else
                            {
                                return RedirectToAction(nameof(Index));
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

        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                //VehicleViewModel vehicleViewModel = new VehicleViewModel();
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                vehicleViewModel.CompanyId = CompanyId;
                vehicleViewModel.Id = id;

                var result = webServices.Post(vehicleViewModel, "AWFVehicle/Edit", true);
                if (result.Data != null)
                {
                    vehicleViewModel = (new JavaScriptSerializer()).Deserialize<VehicleViewModel>(result.Data.ToString());
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return View(vehicleViewModel);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {

                vehicleViewModel.Id = Id;
                vehicleViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var Result = webServices.Post(vehicleViewModel, "AWFVehicle/Edit");

                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    vehicleViewModel = (new JavaScriptSerializer()).Deserialize<VehicleViewModel>(Result.Data.ToString());
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
                                content.Add(new StringContent(vehicleViewModel.TraficPlateNumber ?? ""), "TraficPlateNumber");
                                content.Add(new StringContent(vehicleViewModel.TCNumber ?? ""), "TCNumber");
                                content.Add(new StringContent(vehicleViewModel.Model ?? ""), "Model");
                                content.Add(new StringContent(vehicleViewModel.Brand ?? ""), "Brand");
                                content.Add(new StringContent(vehicleViewModel.Color ?? ""), "Color");
                                content.Add(new StringContent(vehicleViewModel.MulkiaExpiry ?? ""), "MulkiaExpiry");
                                content.Add(new StringContent(vehicleViewModel.InsuranceExpiry ?? ""), "InsuranceExpiry");
                                content.Add(new StringContent(vehicleViewModel.RegisteredRegion ?? ""), "RegisteredRegion");
                                content.Add(new StringContent(vehicleViewModel.Comments ?? ""), "Comments");



                                var result = webServices.PostMultiPart(content, "AWFVehicle/update", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                {
                                    return Redirect(nameof(Index));
                                }

                            }
                        }
                    }
                    return RedirectToAction(nameof(Details), new { vehicleViewModel.Id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public List<VehicleViewModel> AdminVehicles()
        {
            try
            {
                //CompanyId = 2;

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    _pageSize = 1,
                    CompanyId = 2,
                    PageSize = 100,
                };
                var VehicleList = webServices.Post(pagingParameterModel, "AWFVehicle/All");

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

        [HttpPost]
        public ActionResult UploadDocumentsAdd(UploadDocumentsViewModel uploadDocumentsViewModel, HttpPostedFileBase FileUrl)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    using (HttpClient client = new HttpClient())
                    {
                        if (Request.Files.Count > 0)
                        {
                            var file = FileUrl;


                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("FileUrl") { FileName = file.FileName };
                            content.Add(fileContent);

                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                        }
                        string UserId = Session["UserId"].ToString();
                        content.Add(new StringContent(UserId), "CreatedBy");
                        content.Add(new StringContent(uploadDocumentsViewModel.VehicleId.ToString()), "VehicleId");
                        content.Add(new StringContent(uploadDocumentsViewModel.FilesName ?? ""), "FilesName");
                        var result = webServices.PostMultiPart(content, "UploadDocuments/UploadDocumentsAdd", true);
                        if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            return Redirect(nameof(Index));
                        }
                        else
                        {
                            return Redirect(nameof(Index));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
