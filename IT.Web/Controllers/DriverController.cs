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
    public class DriverController : Controller
    {
        WebServices webServices = new WebServices();
        List<DriverViewModel> driverViewModels = new List<DriverViewModel>();
        DriverViewModel driverViewModel = new DriverViewModel();
        int CompanyId = 0;

        [HttpGet]
        public ActionResult Index(int CompId = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult All(int CompId = 0)
        {
            if (CompId == 0)
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
            }
            else
            {
                CompanyId = CompId;
            }
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
                if (CompId > 0)
                {
                    CompanyId = CompId;
                }
                else
                {
                    CompanyId = Convert.ToInt32(Session["CompanyId"]);
                }
                PagingParameterModel pagingParameterModel = new PagingParameterModel();
                pagingParameterModel.CompanyId = CompanyId;


                if (Convert.ToInt32(start) >= Convert.ToInt32(length))
                {
                    pagingParameterModel.pageNumber = (Convert.ToInt32(start) / Convert.ToInt32(length)) + 1;
                }
                   int pageNumer = (Convert.ToInt32(start) / Convert.ToInt32(length)) + 1;
                    pagingParameterModel.pageNumber = pageNumer;
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
               

                var DriverList = webServices.Post(pagingParameterModel, "Driver/All");
                int TotalRow = 0;
                if (DriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (DriverList.Data != "[]")
                    {
                        driverViewModels = (new JavaScriptSerializer().Deserialize<List<DriverViewModel>>(DriverList.Data.ToString()));
                        TotalRow = driverViewModels[0].TotalRows;
                    }
                    return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = driverViewModels }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = driverViewModels }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AllDrivers(int CompId = 0)
        {
            try
            {
                driverViewModels = new List<DriverViewModel>();
                driverViewModels = Drivers(CompId);

                return Json(driverViewModels, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult ChangeStatus(DriverViewModel  driverViewModel)
        {
            try
            {
                var Result = new ServiceResponseModel();

                driverViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                Result = webServices.Post(driverViewModel, "Driver/ChangeStatus");

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
        

        public List<DriverViewModel> Drivers(int Id = 0)
        {
            if (Id > 0)
            {
                CompanyId = Id;
            }
            else
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
            }
            PagingParameterModel pagingParameterModel = new PagingParameterModel();
            pagingParameterModel.CompanyId = CompanyId;

            pagingParameterModel.pageNumber = 1;
            pagingParameterModel._pageSize = 500;
            pagingParameterModel.PageSize = 500;
            var DriverList = webServices.Post(pagingParameterModel, "Driver/All");

            if (DriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (DriverList.Data != "[]")
                {
                    driverViewModels = (new JavaScriptSerializer().Deserialize<List<DriverViewModel>>(DriverList.Data.ToString()));
                }
                driverViewModels.Insert(0, new DriverViewModel() { Id = 0, Name = "Select Driver" });
            }
            return driverViewModels;
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View(new DriverViewModel());
        }
                
        [HttpPost]
        public ActionResult Create(DriverViewModel driverViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(driverViewModel);
                }
                else
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase[] httpPostedFileBase = new HttpPostedFileBase[8];
                        if (driverViewModel.PassportBackFile != null)
                        {
                            httpPostedFileBase[0] = driverViewModel.PassportBackFile;
                        }
                        if (driverViewModel.DriverImageUrlFile != null)
                        {
                            httpPostedFileBase[1] = driverViewModel.DriverImageUrlFile;
                        }
                        if (driverViewModel.DrivingLicenseBackFile != null)
                        {
                            httpPostedFileBase[2] = driverViewModel.DrivingLicenseBackFile;
                        }
                        if (driverViewModel.DrivingLicenseFrontFile != null)
                        {
                            httpPostedFileBase[3] = driverViewModel.DrivingLicenseFrontFile;
                        }
                        if (driverViewModel.IDUAECopyBackFile != null)
                        {
                            httpPostedFileBase[4] = driverViewModel.IDUAECopyBackFile;
                        }
                        if (driverViewModel.IDUAECopyFrontFile != null)
                        {
                            httpPostedFileBase[5] = driverViewModel.IDUAECopyFrontFile;
                        }
                        if (driverViewModel.VisaCopyFile != null)
                        {
                            httpPostedFileBase[6] = driverViewModel.VisaCopyFile;
                        }
                        if (driverViewModel.PassportCopyFile != null)
                        {
                            httpPostedFileBase[7] = driverViewModel.PassportCopyFile;
                        }

                        var file = driverViewModel.PassportBackFile;

                        using (HttpClient client = new HttpClient())
                        {
                            using (var content = new MultipartFormDataContent())
                            {

                                if (httpPostedFileBase.ToList().Count > 0)
                                {

                                    for (int i = 0; i < 8; i++)
                                    {
                                        if (httpPostedFileBase[i] != null)
                                        {
                                            file = httpPostedFileBase[i];

                                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                            var fileContent = new ByteArrayContent(fileBytes);

                                            if (i == 0)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportBack") { FileName = file.FileName };
                                            }
                                            else if (i == 1)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("DriverImageUrl") { FileName = file.FileName };
                                            }
                                            else if (i == 2)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("DrivingLicenseBack") { FileName = file.FileName };
                                            }
                                            else if (i == 3)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("DrivingLicenseFront") { FileName = file.FileName };
                                            }
                                            else if (i == 4)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("IDUAECopyBack") { FileName = file.FileName };
                                            }
                                            else if (i == 5)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("IDUAECopyFront") { FileName = file.FileName };
                                            }
                                            else if (i == 6)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("VisaCopy") { FileName = file.FileName };
                                            }
                                            else if (i == 7)
                                            {
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportCopy") { FileName = file.FileName };
                                            }
                                            content.Add(fileContent);
                                        }
                                    }
                                }

                                string UserId = Session["UserId"].ToString();
                                content.Add(new StringContent(UserId), "CreatedBy");
                                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                                content.Add(new StringContent(CompanyId.ToString()), "CompanyId");
                                content.Add(new StringContent(driverViewModel.Name ?? ""), "FullName");
                                content.Add(new StringContent(driverViewModel.Contact ?? ""), "Contact");
                                content.Add(new StringContent(driverViewModel.Email ?? ""), "Email");
                                content.Add(new StringContent(driverViewModel.Facebook ?? ""), "Facebook");
                                content.Add(new StringContent("ClientDocs"), "ClientDocs");

                                if (driverViewModel.LicienceList != null)
                                {
                                    if (driverViewModel.LicienceList.ToList().Count == 1)
                                    {
                                        content.Add(new StringContent("[" + driverViewModel.LicienceList[0].ToString() + "]"), "LicenseTypes");
                                    }
                                    else if (driverViewModel.LicienceList.ToList().Count == 2)
                                    {
                                        content.Add(new StringContent("[" + driverViewModel.LicienceList[0].ToString() + "," + driverViewModel.LicienceList[1].ToString() + "]"), "LicenseTypes");
                                    }
                                    else
                                    {
                                        content.Add(new StringContent("[" + driverViewModel.LicienceList[0].ToString() + "," + driverViewModel.LicienceList[1].ToString() + "," + driverViewModel.LicienceList[2].ToString() + "]"), "LicenseTypes");
                                    }
                                }
                                content.Add(new StringContent(driverViewModel.LicenseExpiry ?? ""), "DrivingLicenseExpiryDate");
                                content.Add(new StringContent(driverViewModel.Nationality ?? ""), "Nationality");
                                content.Add(new StringContent(driverViewModel.Comments ?? ""), "Comments");

                                var result = webServices.PostMultiPart(content, "Driver/Add", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    if(result.Message == "\"Email Already Availible\"")
                                      {
                                        ModelState.AddModelError("Email", "Email already availible choose another");
                                        return View(driverViewModel);
                                      }
                                    else
                                    {
                                        return Redirect(nameof(Index));
                                    }
                                    
                                }
                                else
                                {
                                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                    {
                                        return Redirect(nameof(Index));
                                    }
                                    else
                                    {
                                        return View(driverViewModel);
                                    }
                                }

                            }
                        }
                    }
                    return RedirectToAction(nameof(Details), new { driverViewModel.Id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Details(int Id, int CompId = 0)
        {
            CompanyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
                if (CompId < 1)
                {
                    CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    ViewBag.LayoutName = "~/Views/Shared/_layout.cshtml";
                }
                else
                {
                    //VehicleViewModel vehicleViewModel = new VehicleViewModel();
                    CompanyId = CompId;
                    ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
                }
                driverViewModel.Id = Id;
                driverViewModel.CompanyId = CompanyId;

                var result = webServices.Post(driverViewModel, "Driver/Edit");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        driverViewModel = (new JavaScriptSerializer().Deserialize<DriverViewModel>(result.Data.ToString()));
                    }
                }

                return View(driverViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(int Id, int CompId = 0)
        {
            CompanyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
                if (CompId < 1)
                {
                    CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    ViewBag.LayoutName = "~/Views/Shared/_layout.cshtml";                    
                }
                else
                {                   
                    CompanyId = CompId;
                    ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
                }
                
                driverViewModel.Id = Id;
                driverViewModel.CompanyId = CompanyId;
                var result = webServices.Post(driverViewModel, "Driver/Edit/");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        driverViewModel = (new JavaScriptSerializer().Deserialize<DriverViewModel>(result.Data.ToString()));
                    }
                }

                return View(driverViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(DriverViewModel driverViewModel)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase[] httpPostedFileBase = new HttpPostedFileBase[8];
                    if (driverViewModel.PassportBackFile != null)
                    {
                        httpPostedFileBase[0] = driverViewModel.PassportBackFile;
                    }
                    if (driverViewModel.DriverImageUrlFile != null)
                    {
                        httpPostedFileBase[1] = driverViewModel.DriverImageUrlFile;
                    }
                    if (driverViewModel.DrivingLicenseBackFile != null)
                    {
                        httpPostedFileBase[2] = driverViewModel.DrivingLicenseBackFile;
                    }
                    if (driverViewModel.DrivingLicenseFrontFile != null)
                    {
                        httpPostedFileBase[3] = driverViewModel.DrivingLicenseFrontFile;
                    }
                    if (driverViewModel.IDUAECopyBackFile != null)
                    {
                        httpPostedFileBase[4] = driverViewModel.IDUAECopyBackFile;
                    }
                    if (driverViewModel.IDUAECopyFrontFile != null)
                    {
                        httpPostedFileBase[5] = driverViewModel.IDUAECopyFrontFile;
                    }
                    if (driverViewModel.VisaCopyFile != null)
                    {
                        httpPostedFileBase[6] = driverViewModel.VisaCopyFile;
                    }
                    if (driverViewModel.PassportCopyFile != null)
                    {
                        httpPostedFileBase[7] = driverViewModel.PassportCopyFile;
                    }

                    var file = driverViewModel.PassportBackFile;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {

                            if (httpPostedFileBase.ToList().Count > 0)
                            {

                                for (int i = 0; i < 8; i++)
                                {
                                    if (httpPostedFileBase[i] != null)
                                    {
                                        file = httpPostedFileBase[i];

                                        byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                        file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                        var fileContent = new ByteArrayContent(fileBytes);

                                        if (i == 0)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportBack") { FileName = file.FileName };
                                        }
                                        else if (i == 1)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("DriverImageUrl") { FileName = file.FileName };
                                        }
                                        else if (i == 2)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("DrivingLicenseBack") { FileName = file.FileName };
                                        }
                                        else if (i == 3)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("DrivingLicenseFront") { FileName = file.FileName };
                                        }
                                        else if (i == 4)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("IDUAECopyBack") { FileName = file.FileName };
                                        }
                                        else if (i == 5)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("IDUAECopyFront") { FileName = file.FileName };
                                        }
                                        else if (i == 6)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("VisaCopy") { FileName = file.FileName };
                                        }
                                        else if (i == 7)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportCopy") { FileName = file.FileName };
                                        }
                                        content.Add(fileContent);
                                    }
                                }
                            }

                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "UpdatBy");
                            content.Add(new StringContent(driverViewModel.Id.ToString()), "Id");
                            CompanyId = Convert.ToInt32(Session["CompanyId"]);
                            content.Add(new StringContent(CompanyId.ToString()), "CompanyId");
                            content.Add(new StringContent(driverViewModel.Name ?? ""), "FullName");
                            content.Add(new StringContent(driverViewModel.Contact ?? ""), "Contact");
                            content.Add(new StringContent(driverViewModel.Email ?? ""), "Email");
                            content.Add(new StringContent(driverViewModel.Facebook ?? ""), "Facebook");
                            content.Add(new StringContent(driverViewModel.LicenseExpiry ?? ""), "DrivingLicenseExpiryDate");
                            
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");

                            if (driverViewModel.LicienceList.ToList().Count == 1)
                            {
                                content.Add(new StringContent("[" + driverViewModel.LicienceList[0].ToString() + "]"), "LicenseTypes");
                            }
                            else if (driverViewModel.LicienceList.ToList().Count == 2)
                            {
                                content.Add(new StringContent("[" + driverViewModel.LicienceList[0].ToString() + "," + driverViewModel.LicienceList[1].ToString() + "]"), "LicenseTypes");
                            }
                            else
                            {
                                content.Add(new StringContent("[" + driverViewModel.LicienceList[0].ToString() + "," + driverViewModel.LicienceList[1].ToString() + "," + driverViewModel.LicienceList[2].ToString() + "]"), "LicenseTypes");
                            }
                            content.Add(new StringContent(driverViewModel.LicenseExpiry ?? ""), "LicenseExpiry");
                            content.Add(new StringContent(driverViewModel.Nationality ?? ""), "Nationality");
                            content.Add(new StringContent(driverViewModel.Comments ?? ""), "Comments");

                            var result = webServices.PostMultiPart(content, "Driver/Update", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Index));
                            }

                        }
                    }
                }
                return RedirectToAction(nameof(Details), new { driverViewModel.Id });
                //return RedirectToAction(nameof(Details), new { driverViewModel.Id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}