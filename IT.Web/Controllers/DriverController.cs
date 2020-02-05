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
    public class DriverController : Controller
    {
        WebServices webServices = new WebServices();
        List<DriverViewModel> driverViewModels = new List<DriverViewModel>();
        DriverViewModel driverViewModel = new DriverViewModel();
        int CompanyId = 0;

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

                var DriverList = webServices.Post(pagingParameterModel, "Driver/All");

                if (DriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    driverViewModels = (new JavaScriptSerializer().Deserialize<List<DriverViewModel>>(DriverList.Data.ToString()));
                    return View(driverViewModels);
                }

                return View(driverViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Create()
        {
            return View(new DriverViewModel());
        }

        [HttpPost]
        public ActionResult Create(DriverViewModel driverViewModel)
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
                    if (driverViewModel.DrivingLicenseBackFile != null)
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
                            content.Add(new StringContent(driverViewModel.Name == null ? "" : driverViewModel.Name), "FullName");
                            content.Add(new StringContent(driverViewModel.Contact == null ? "" : driverViewModel.Contact), "Contact");
                            content.Add(new StringContent(driverViewModel.Email == null ? "" : driverViewModel.Email), "Email");
                            content.Add(new StringContent(driverViewModel.Facebook == null ? "" : driverViewModel.Facebook), "Facebook");
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
                            content.Add(new StringContent(driverViewModel.LicenseExpiry == null ? "" : driverViewModel.LicenseExpiry), "DrivingLicenseExpiryDate");
                            content.Add(new StringContent(driverViewModel.Nationality == null ? "" : driverViewModel.Nationality), "Nationality");
                            content.Add(new StringContent(driverViewModel.Comments == null ? "" : driverViewModel.Comments), "Comments");

                            var result = webServices.PostMultiPart(content, "Driver/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Index));
                            }

                        }
                    }
                }
                return RedirectToAction(nameof(Details), new { Id = driverViewModel.Id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Details(int Id)
        {
            CompanyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
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

        public ActionResult Edit(int Id)
        {

            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                driverViewModel.Id = Id;
                driverViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
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
                    if (driverViewModel.DrivingLicenseBackFile != null)
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
                            content.Add(new StringContent(driverViewModel.Name == null ? "" : driverViewModel.Name), "FullName");
                            content.Add(new StringContent(driverViewModel.Contact == null ? "" : driverViewModel.Contact), "Contact");
                            content.Add(new StringContent(driverViewModel.Email == null ? "" : driverViewModel.Email), "Email");
                            content.Add(new StringContent(driverViewModel.Facebook == null ? "" : driverViewModel.Facebook), "Facebook");
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
                            content.Add(new StringContent(driverViewModel.LicenseExpiry == null ? "" : driverViewModel.LicenseExpiry), "LicenseExpiry");
                            content.Add(new StringContent(driverViewModel.Nationality == null ? "" : driverViewModel.Nationality), "Nationality");
                            content.Add(new StringContent(driverViewModel.Comments == null ? "" : driverViewModel.Comments), "Comments");

                            var result = webServices.PostMultiPart(content, "Driver/Update", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Index));
                            }

                        }
                    }
                }
                return RedirectToAction(nameof(Details), new { Id = driverViewModel.Id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}