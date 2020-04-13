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
    public class CompanyController : Controller
    {
        WebServices webServices = new WebServices();
        List<CompnayModel> compnayModels = new List<CompnayModel>();
        List<CompanyViewModel> companyViewModels = new List<CompanyViewModel>();
        readonly CompanyViewModel companyViewModel = new CompanyViewModel();

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View();

                //PagingParameterModel pagingParameterModel = new PagingParameterModel
                //{
                //    pageNumber = 1,
                //    _pageSize = 1,
                //    PageSize = 100,
                //};
                //var CompanyList = webServices.Post(pagingParameterModel, "Company/CompayAll");
                //if (CompanyList.StatusCode == System.Net.HttpStatusCode.Accepted)
                //{
                //    if (CompanyList.Data != "[]" && CompanyList.Data != null)
                //    {
                //        compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                //    }
                //}
                //return View(compnayModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CompanyAll()
        {
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
                //int skip = start != null ? Convert.ToInt32(start) : 0;

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                if (Convert.ToInt32(start) == 0)
                {
                    pagingParameterModel.pageNumber = 1;
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
                }
                else
                {
                    pagingParameterModel.pageNumber = Convert.ToInt32(draw);
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
                }

                var CompanyList = webServices.Post(pagingParameterModel, "Company/CompayAll");
                if (CompanyList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (CompanyList.Data != "[]" && CompanyList.Data != null)
                    {
                        compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                        TotalRow = compnayModels.Count;

                        return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = compnayModels }, JsonRequestBehavior.AllowGet);


                        //compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                    }
                }
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = compnayModels }, JsonRequestBehavior.AllowGet);

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
                CompnayModel compnayModel = new CompnayModel();

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    Id = id
                };
                var companyData = webServices.Post(pagingParameterModel, "Company/CompanyById");
                if (companyData.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (companyData.Data != "[]" && companyData.Data != null)
                    {
                        compnayModel = (new JavaScriptSerializer().Deserialize<CompnayModel>(companyData.Data.ToString()));
                    }
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(compnayModel, JsonRequestBehavior.AllowGet);
                }
                return View(compnayModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            CountryController countryController = new CountryController();
            ViewBag.Countries = countryController.Countries();
            return View(new CompnayModel());
        }

        [HttpGet]
        public ActionResult Creates()
        {
            return View(new CompnayModel());
        }

        [HttpPost]
        public ActionResult Create(CompnayModel compnayModel, HttpPostedFileBase LogoUrl)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View("Create", compnayModel);
                }
                else
                {
                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            if (LogoUrl != null)
                            {
                                if (Request.Files.Count > 0)
                                {
                                    var file = LogoUrl;

                                    byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                    file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                    var fileContent = new ByteArrayContent(fileBytes);
                                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("LogoUrl") { FileName = file.FileName };
                                    content.Add(fileContent);

                                    content.Add(new StringContent("ClientDocs"), "ClientDocs");
                                }
                            }
                            content.Add(new StringContent(compnayModel.Name ?? "Unknown"), "Name");
                            content.Add(new StringContent("street Data"), "Street");
                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "CreatedBy");
                            content.Add(new StringContent(compnayModel.Postcode ?? ""), "Postcode");
                            content.Add(new StringContent(compnayModel.City ?? ""), "City");
                            content.Add(new StringContent(compnayModel.Address ?? ""), "Address");
                            content.Add(new StringContent(compnayModel.State ?? ""), "State");
                            content.Add(new StringContent(compnayModel.Country ?? ""), "Country");
                            content.Add(new StringContent(compnayModel.Cell ?? ""), "Cell");
                            content.Add(new StringContent(compnayModel.Phone ?? ""), "Phone");
                            content.Add(new StringContent(compnayModel.Email ?? ""), "Email");
                            content.Add(new StringContent(compnayModel.Web ?? ""), "Web");
                            content.Add(new StringContent(compnayModel.TRN ?? ""), "TRN");
                            content.Add(new StringContent(compnayModel.Remarks ?? ""), "Remarks");
                            content.Add(new StringContent(compnayModel.OwnerRepresentaive ?? ""), "OwnerRepresentaive");
                            content.Add(new StringContent("true"), "IsActive");

                            //  var result1 = client.PostAsync("http://localhost:64299/api/Company/Add", content).Result;
                            var result = webServices.PostMultiPart(content, "Company/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                var companyViewModel = new CompanyViewModel();
                                companyViewModel = (new JavaScriptSerializer().Deserialize<CompanyViewModel>(result.Data.ToString()));

                                UserCompanyViewModel userCompanyViewModel1 = new UserCompanyViewModel
                                {
                                    Authority = companyViewModel.Authority,
                                    CompanyId = companyViewModel.Id,
                                    UserId = companyViewModel.CreatedBy,
                                    LogoUrl = companyViewModel.LogoUrl,
                                    FirstName = companyViewModel.UserName,
                                    CompanyName = companyViewModel.Name,
                                    UserName = companyViewModel.UserName,
                                    ImageUrl = companyViewModel.ImageUrl
                                };

                                Session["userCompanyViewModel"] = userCompanyViewModel1;
                                Session["CompanyId"] = companyViewModel.Id;
                                Session["UserId"] = companyViewModel.CreatedBy;
                                ViewBag.Message = "Created";
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.Message = "Failed";
                                return View(compnayModel);
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
        public ActionResult CustomerUpdate(CompnayModel compnayModel, HttpPostedFileBase LogoUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", compnayModel);
                }
                else
                {
                    //if (Request.Files.Count > 0 && LogoUrl != null)
                    //{
                    var file = LogoUrl;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            if (LogoUrl != null)
                            {
                                byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                var fileContent = new ByteArrayContent(fileBytes);
                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("LogoUrl") { FileName = file.FileName };
                                content.Add(fileContent);
                            }

                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            content.Add(new StringContent(compnayModel.Id.ToString()), "Id");
                            content.Add(new StringContent(compnayModel.Name ?? ""), "Name");
                            content.Add(new StringContent(compnayModel.Street ?? ""), "Street");
                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "UpdatedBy");
                            content.Add(new StringContent(compnayModel.Postcode ?? ""), "Postcode");
                            content.Add(new StringContent(compnayModel.City ?? ""), "City");
                            content.Add(new StringContent(compnayModel.Address ?? ""), "Address");
                            content.Add(new StringContent(compnayModel.State ?? ""), "State");
                            content.Add(new StringContent(compnayModel.Country ?? ""), "Country");
                            content.Add(new StringContent(compnayModel.Cell ?? ""), "Cell");
                            content.Add(new StringContent(compnayModel.Phone ?? ""), "Phone");
                            content.Add(new StringContent(compnayModel.Email ?? ""), "Email");
                            content.Add(new StringContent(compnayModel.Web ?? ""), "Web");
                            content.Add(new StringContent(compnayModel.TRN ?? ""), "TRN");
                            content.Add(new StringContent(compnayModel.Remarks ?? ""), "Remarks");
                            content.Add(new StringContent(compnayModel.OwnerRepresentaive ?? ""), "OwnerRepresentaive");
                            content.Add(new StringContent("true"), "IsActive");

                            //  var result1 = client.PostAsync("http://localhost:64299/api/Company/Add", content).Result;
                            var result = webServices.PostMultiPart(content, "Company/Update", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                var companyViewModel = new CompanyViewModel();
                                companyViewModel = (new JavaScriptSerializer().Deserialize<CompanyViewModel>(result.Data.ToString()));
                                
                                var userCompanyViewModel2 = Session["userCompanyViewModel"] as UserCompanyViewModel;

                                userCompanyViewModel2.LogoUrl = companyViewModel.LogoUrl;
                                userCompanyViewModel2.CompanyName = companyViewModel.Name;
                                
                                Session["userCompanyViewModel"] = userCompanyViewModel2;
                                Session["CompanyId"] = companyViewModel.Id;
                                Session["UserId"] = companyViewModel.CreatedBy;

                                //return RedirectToAction("/");
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.Message = "Failed";
                            }
                        }
                    }
                    //}
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(CompnayModel compnayModel, HttpPostedFileBase LogoUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", compnayModel);
                }
                else
                {
                    if (Request.Files.Count > 0 && LogoUrl != null)
                    {
                        var file = LogoUrl;

                        using (HttpClient client = new HttpClient())
                        {
                            using (var content = new MultipartFormDataContent())
                            {
                                if (LogoUrl != null)
                                {
                                    byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                    file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                    var fileContent = new ByteArrayContent(fileBytes);
                                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("LogoUrl") { FileName = file.FileName };
                                    content.Add(fileContent);
                                }

                                content.Add(new StringContent("ClientDocs"), "ClientDocs");
                                content.Add(new StringContent(compnayModel.Id.ToString()), "Id");
                                content.Add(new StringContent(compnayModel.Name ?? ""), "Name");
                                content.Add(new StringContent(compnayModel.Street ?? ""), "Street");
                                string UserId = Session["UserId"].ToString();
                                content.Add(new StringContent(UserId), "UpdatedBy");
                                content.Add(new StringContent(compnayModel.Postcode ?? ""), "Postcode");
                                content.Add(new StringContent(compnayModel.City ?? ""), "City");
                                content.Add(new StringContent(compnayModel.Address ?? ""), "Address");
                                content.Add(new StringContent(compnayModel.State ?? ""), "State");
                                content.Add(new StringContent(compnayModel.Country ?? ""), "Country");
                                content.Add(new StringContent(compnayModel.Cell ?? ""), "Cell");
                                content.Add(new StringContent(compnayModel.Phone ?? ""), "Phone");
                                content.Add(new StringContent(compnayModel.Email ?? ""), "Email");
                                content.Add(new StringContent(compnayModel.Web ?? ""), "Web");
                                content.Add(new StringContent(compnayModel.TRN ?? ""), "TRN");
                                content.Add(new StringContent(compnayModel.Remarks ?? ""), "Remarks");
                                content.Add(new StringContent(compnayModel.OwnerRepresentaive ?? ""), "OwnerRepresentaive");
                                content.Add(new StringContent("true"), "IsActive");

                                //  var result1 = client.PostAsync("http://localhost:64299/api/Company/Add", content).Result;
                                var result = webServices.PostMultiPart(content, "Company/Update", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                {
                                    var companyViewModel = new CompanyViewModel();
                                    companyViewModel = (new JavaScriptSerializer().Deserialize<CompanyViewModel>(result.Data.ToString()));

                                    return RedirectToAction(nameof(Index));
                                }
                                else
                                {
                                    ViewBag.Message = "Failed";
                                }
                            }
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                CompnayModel compnayModel = new CompnayModel();

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    Id = id
                };
                var companyData = webServices.Post(pagingParameterModel, "Company/CompanyById");
                if (companyData.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (companyData.Data != "[]" && companyData.Data != null)
                    {
                        compnayModel = (new JavaScriptSerializer().Deserialize<CompnayModel>(companyData.Data.ToString()));
                    }
                }

                CountryController countryController = new CountryController();
                ViewBag.Countries = countryController.Countries();

                return View(compnayModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult CompanyEdit(int id)
        {
            try
            {
                CompnayModel compnayModel = new CompnayModel();

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    Id = id
                };
                var companyData = webServices.Post(pagingParameterModel, "Company/CompanyById");
                if (companyData.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (companyData.Data != "[]" && companyData.Data != null)
                    {
                        compnayModel = (new JavaScriptSerializer().Deserialize<CompnayModel>(companyData.Data.ToString()));
                    }
                }

                CountryController countryController = new CountryController();
                ViewBag.Countries = countryController.Countries();

                if(Request.IsAjaxRequest())
                {
                    return Json(compnayModel, JsonRequestBehavior.AllowGet);
                }

                return View(compnayModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult CompanyInformation(int id)
        {
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                CompnayModel compnayModel = new CompnayModel();

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    Id = id
                };
                var companyData = webServices.Post(pagingParameterModel, "Company/CompanyById");
                if (companyData.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (companyData.Data != "[]" && companyData.Data != null)
                    {
                        compnayModel = (new JavaScriptSerializer().Deserialize<CompnayModel>(companyData.Data.ToString()));
                    }
                }
                var usercCompany = Session["userCompanyViewModel"] as UserCompanyViewModel;
                userViewModel.UserName = usercCompany.UserName;
                var userList = webServices.Post(userViewModel, "User/UserInformationByUserName");

                if (userList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    userViewModel = (new JavaScriptSerializer().Deserialize<UserViewModel>(userList.Data.ToString()));

                }

                ViewBag.userViewModel = userViewModel;

                return View(compnayModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AdminCompanyInformation(int id)
        {
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                CompnayModel compnayModel = new CompnayModel();

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    Id = id
                };
                var companyData = webServices.Post(pagingParameterModel, "Company/CompanyById");
                if (companyData.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (companyData.Data != "[]" && companyData.Data != null)
                    {
                        compnayModel = (new JavaScriptSerializer().Deserialize<CompnayModel>(companyData.Data.ToString()));
                    }
                }
                var usercCompany = Session["userCompanyViewModel"] as UserCompanyViewModel;
                userViewModel.UserName = usercCompany.UserName;
                var userList = webServices.Post(userViewModel, "User/UserInformationByUserName");

                if (userList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    userViewModel = (new JavaScriptSerializer().Deserialize<UserViewModel>(userList.Data.ToString()));

                }

                ViewBag.userViewModel = userViewModel;

                return View(compnayModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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

        [HttpGet]
        public ActionResult CashCompany()
        {
            CountryController countryController = new CountryController();
            ViewBag.Countries = countryController.Countries();
            return View(new CompnayModel());
        }

        [HttpPost]
        public ActionResult CashCompanyCreate(CompnayModel compnayModel, HttpPostedFileBase LogoUrl)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    CountryController countryController = new CountryController();
                    ViewBag.Countries = countryController.Countries();
                    return View("CashCompany", compnayModel);
                }
                else
                {

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            if (Request.Files.Count > 0)
                            {
                                if (LogoUrl != null)
                                {
                                    var file = LogoUrl;
                                    byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                    file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                    var fileContent = new ByteArrayContent(fileBytes);
                                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("LogoUrl") { FileName = file.FileName };
                                    content.Add(fileContent);
                                }
                            }
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            content.Add(new StringContent(compnayModel.Name ?? ""), "Name");
                            content.Add(new StringContent(compnayModel.Street ?? ""), "street");
                            content.Add(new StringContent(compnayModel.Postcode ?? ""), "Postcode");
                            content.Add(new StringContent(compnayModel.City ?? ""), "City");
                            content.Add(new StringContent(compnayModel.Street ?? ""), "State");
                            content.Add(new StringContent(compnayModel.Country ?? ""), "Country");
                            content.Add(new StringContent(compnayModel.Email ?? ""), "Email");
                            content.Add(new StringContent(compnayModel.Phone ?? ""), "Phone");
                            content.Add(new StringContent(compnayModel.Cell ?? ""), "Cell");
                            content.Add(new StringContent(compnayModel.OwnerRepresentaive ?? ""), "OwnerRepresentaive");
                            content.Add(new StringContent(compnayModel.Remarks ?? ""), "Commentes");
                            content.Add(new StringContent(compnayModel.TRN ?? ""), "TRN");
                            content.Add(new StringContent(compnayModel.Address ?? ""), "Address");
                            content.Add(new StringContent("true"), "IsCashCompany");
                            //  var result1 = client.PostAsync("http://itmolen-001-site8.htempurl.com/api/Company/Add", content).Result;
                            var result = webServices.PostMultiPart(content, "Company/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                ViewBag.Message = "Created";
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                ViewBag.Message = "Failed";
                                CountryController countryController = new CountryController();
                                ViewBag.Countries = countryController.Countries();
                                return View("CashCompany", compnayModel);
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
        public ActionResult CompanyFrezeOrBlackListByAdmin(SearchViewModel searchViewModel)
        {
            try
            {
                var Result = webServices.Post(searchViewModel, "Company/CompanyFrezeOrBlackListByAdmin");
                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));

                    if (res > 0)
                    {
                        return RedirectToAction("Details", new { searchViewModel.Id });
                    }
                }
                return RedirectToAction("Details", new { searchViewModel.Id });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [NonAction]
        public List<CompanyViewModel> Companies()
        {
            try
            {

                var CompanyList = webServices.Post(new PagingParameterModel(), "Company/CompayAllWithOutPagination");
                if (CompanyList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (CompanyList.Data != "[]")
                    {
                        companyViewModels = (new JavaScriptSerializer().Deserialize<List<CompanyViewModel>>(CompanyList.Data.ToString()));
                        companyViewModels.Insert(0,new CompanyViewModel() { Id = 0, Name = "Select Customer" });
                    }
                    else
                    {
                        companyViewModels.Add(new CompanyViewModel() { Id = 0, Name = "Select Customer" });
                    }
                }
                return companyViewModels;
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
                            content.Add(new StringContent(uploadDocumentsViewModel.CompanyId.ToString()), "CompanyId");
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


        [HttpPost]
        public ActionResult GetClientCompanyList()
        {
            try
            {
                var companyList = Companies();
                return Json(companyList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

    }
}