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
    public class AwfDriverController : Controller
    {
        WebServices webServices = new WebServices();
        List<DriverViewModel> driverViewModels = new List<DriverViewModel>();
        List<DriverLoginHistoryViewModel> driverLoginHistoryViewModels = new List<DriverLoginHistoryViewModel>();
        DriverViewModel driverViewModel = new DriverViewModel();
        int CompanyId;

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View();
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
              
                var DriverList = webServices.Post(pagingParameterModel, "AWFDriver/All");

                if (DriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (DriverList.Data != "[]" && DriverList.Data != null)
                    {                       
                        driverViewModels = (new JavaScriptSerializer().Deserialize<List<DriverViewModel>>(DriverList.Data.ToString()));
                        TotalRow = driverViewModels[0].TotalRows;

                        return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = driverViewModels }, JsonRequestBehavior.AllowGet);
                        //compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                    }
                }
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = driverViewModels }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ChangeStatus(DriverViewModel driverViewModel)
        {
            try
            {
                var Result = new ServiceResponseModel();
                
                    driverViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                    Result = webServices.Post(driverViewModel, "AWFDriver/ChangeStatus");
                
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
        public ActionResult Details(int id)
        {
            try
            {
                DriverViewModel driverViewModel = new DriverViewModel();

                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                driverViewModel.CompanyId = CompanyId;
                driverViewModel.Id = id;

                var result = webServices.Post(driverViewModel, "AWFDriver/Edit");
                if (result.Data != null)
                {
                    driverViewModel = (new JavaScriptSerializer()).Deserialize<DriverViewModel>(result.Data.ToString());
                }

                return View(driverViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            DriverViewModel driverViewModel = new DriverViewModel();
            driverViewModel.LicenseExpiry = System.DateTime.Now.ToString("yyyy-MM-dd");

            return View(driverViewModel);
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

                                var result = webServices.PostMultiPart(content, "AWFDriver/Add", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                {
                                    return Redirect(nameof(Index));
                                }
                                else if(result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    ModelState.AddModelError("Email", "Email aready exist please choose another");
                                    return View(driverViewModel);
                                }
                            }
                        }
                    }
                    return Redirect(nameof(Create));
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
                //SearchViewModel searchViewModel = new SearchViewModel();
                driverViewModel.Id = Id;
                driverViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var addResult = webServices.Post(driverViewModel, "AWFDriver/Edit/");

                if (addResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    driverViewModel = (new JavaScriptSerializer().Deserialize<DriverViewModel>(addResult.Data.ToString()));
                    driverViewModel.LicenseExpiry = driverViewModel.LicenseExpiry ?? System.DateTime.Now.ToString("yyyy-MM-dd");
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
                if (!ModelState.IsValid)
                {
                    return View("Edit", driverViewModel);
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
                                if (httpPostedFileBase != null)
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

                                var result = webServices.PostMultiPart(content, "AWFDriver/Update", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                {
                                    return Redirect(nameof(Index));
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

        [HttpGet]
        public ActionResult DriverLoginHistoryWithAsignVehicle()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult DriverLoginHistoryAllForAdmin()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    _pageSize = 1,
                    Id = CompanyId,
                    PageSize = 100,
                };

                var DriverLoginList = webServices.Post(pagingParameterModel, "AWFDriver/DriverLoginHistoryAllForAdmin");
                if (DriverLoginList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                    driverLoginHistoryViewModels = (new JavaScriptSerializer().Deserialize<List<DriverLoginHistoryViewModel>>(DriverLoginList.Data.ToString()));
                }

                return View(driverLoginHistoryViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ReleaseVehicle(SearchViewModel searchViewModel)
        {
            try
            {
                var DriverInfo = webServices.Post(searchViewModel, "AWFDriver/ReleaseVehicle");
                if (DriverInfo.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                    driverViewModel = (new JavaScriptSerializer().Deserialize<DriverViewModel>(DriverInfo.Data.ToString()));
                }
                return Redirect(nameof(DriverLoginHistoryAllForAdmin));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        [HttpPost]
        public ActionResult DriverLogouByAdmin(SearchViewModel searchViewModel)
        {
            try
            {
                var DriverInfo = webServices.Post(searchViewModel, "AWFDriver/DriverLogouByAdmin");
                if (DriverInfo.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                    var Id = (new JavaScriptSerializer().Deserialize<int>(DriverInfo.Data.ToString()));
                }

                return Redirect(nameof(DriverLoginHistoryAllForAdmin));

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult DriverAllOnline(DriverViewModel driverViewModel)
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                if (driverViewModel.Name == "Driver")
                {
                    var DriverInfo = webServices.Post(new DriverViewModel(), "AWFDriver/DriverAllOnline/" + CompanyId);
                    if (DriverInfo.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var OnlineDriverList = (new JavaScriptSerializer().Deserialize<List<DriverViewModel>>(DriverInfo.Data.ToString()));
                        OnlineDriverList.Insert(0, new DriverViewModel() { Id = 0, Name = "Select Driver" });

                        return Json(OnlineDriverList, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var siteViewModels = new List<SiteViewModel>();
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
                    siteViewModels.Insert(0, new SiteViewModel() { Id = 0, SiteName = "Select Site" });

                    var OnlineDriverList = new List<DriverViewModel>();

                    foreach (var item in siteViewModels)
                    {
                        OnlineDriverList.Add(new DriverViewModel()
                        {
                            Id = item.Id,
                            Name = item.SiteName
                        });
                    }
                    return Json(OnlineDriverList, JsonRequestBehavior.AllowGet);
                    
                }
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult DriverAllOnlineByDriverId(int Id)
        {

            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var DriverInfo = webServices.Post(new DriverViewModel(), "AWFDriver/DriverAllOnline/" + CompanyId);
                if (DriverInfo.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var OnlineDriverList = (new JavaScriptSerializer().Deserialize<List<DriverViewModel>>(DriverInfo.Data.ToString()));

                    var SingleDriver = OnlineDriverList.Where(x => x.Id == Id).FirstOrDefault();

                    return Json(SingleDriver, JsonRequestBehavior.AllowGet);
                }

                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult UploadDocumentsAdd(UploadDocumentsViewModel uploadDocumentsViewModel, HttpPostedFileBase FileUrl)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = FileUrl;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("FileUrl") { FileName = file.FileName };
                            content.Add(fileContent);
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "CreatedBy");
                            content.Add(new StringContent(uploadDocumentsViewModel.FilesName ?? ""), "FilesName");
                            content.Add(new StringContent(uploadDocumentsViewModel.DriverId.ToString()), "DriverId");
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
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}