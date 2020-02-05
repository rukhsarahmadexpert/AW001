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
    public class CompanyController : Controller
    {
        WebServices webServices = new WebServices();
        List<CompnayModel> compnayModels = new List<CompnayModel>();

        // GET: Company
        public ActionResult Index()
        {

            try
            {
                PagingParameterModel pagingParameterModel = new PagingParameterModel();
                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.PageSize = 100;
                var CompanyList = webServices.Post(pagingParameterModel, "Company/CompayAll");
                if (CompanyList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                }
                return View(compnayModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                CompnayModel compnayModel = new CompnayModel();

                PagingParameterModel pagingParameterModel = new PagingParameterModel();
                pagingParameterModel.Id = id;
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

        // GET: Company/Create
        public ActionResult Create(CompnayModel compnayModel)
        {
            return View();

        }

        [HttpGet]
        public ActionResult Creates()
        {
            return View(new CompnayModel());
        }

        [HttpPost]
        public ActionResult Creates(CompnayModel compnayModel, HttpPostedFileBase file)
        {
            // await Creat(compnayModel,file);
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(CompnayModel compnayModel, HttpPostedFileBase LogoUrl)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = LogoUrl;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("LogoUrl") { FileName = file.FileName };
                            content.Add(fileContent);
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            content.Add(new StringContent("Name"), "Name");
                            content.Add(new StringContent("street Data"), "Street");
                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "CreatedBy");
                            content.Add(new StringContent(compnayModel.Postcode == null ? "" : compnayModel.Postcode), "Postcode");
                            content.Add(new StringContent(compnayModel.City == null ? "" : compnayModel.City), "City");
                            content.Add(new StringContent(compnayModel.Address == null ? "" : compnayModel.Address), "Address");
                            content.Add(new StringContent(compnayModel.State == null ? "" : compnayModel.State), "State");
                            content.Add(new StringContent(compnayModel.Country == null ? "" : compnayModel.Country), "Country");
                            content.Add(new StringContent(compnayModel.Cell == null ? "" : compnayModel.Cell), "Cell");
                            content.Add(new StringContent(compnayModel.Phone == null ? "" : compnayModel.Phone), "Phone");
                            content.Add(new StringContent(compnayModel.Email == null ? "" : compnayModel.Email), "Email");
                            content.Add(new StringContent(compnayModel.Web == null ? "" : compnayModel.Web), "Web");
                            content.Add(new StringContent(compnayModel.TRN == null ? "" : compnayModel.TRN), "TRN");
                            content.Add(new StringContent(compnayModel.Remarks == null ? "" : compnayModel.Remarks), "Remarks");
                            content.Add(new StringContent(compnayModel.OwnerRepresentaive == null ? "" : compnayModel.OwnerRepresentaive), "OwnerRepresentaive");
                            content.Add(new StringContent("true"), "IsActive");



                            //  var result1 = client.PostAsync("http://localhost:64299/api/Company/Add", content).Result;
                            var result = webServices.PostMultiPart(content, "Company/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                var companyViewModel = new CompanyViewModel();
                                companyViewModel = (new JavaScriptSerializer().Deserialize<CompanyViewModel>(result.Data.ToString()));


                                Session["userCompanyViewModel"] = companyViewModel;
                                Session["CompanyId"] = companyViewModel.Id;
                                Session["UserId"] = companyViewModel.CreatedBy;
                                ViewBag.Message = "Created";
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.Message = "Failed";
                            }
                        }
                    }
                }
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Company/Edit/5
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

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
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

        public ActionResult CashCompany()
        {
            return View(new CompnayModel());
        }

        [HttpPost]
        public ActionResult CashCompanyCreate(CompnayModel compnayModel, HttpPostedFileBase LogoUrl)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = LogoUrl;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("LogoUrl") { FileName = file.FileName };
                            content.Add(fileContent);
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            content.Add(new StringContent("Name"), "Name");
                            content.Add(new StringContent("street Data"), "Street");
                            content.Add(new StringContent(compnayModel.Postcode == null ? "" : compnayModel.Postcode), "Postcode");
                            content.Add(new StringContent(compnayModel.City == null ? "" : compnayModel.City), "City");
                            content.Add(new StringContent(compnayModel.Street == null ? "" : compnayModel.Street), "State");
                            content.Add(new StringContent(compnayModel.Country == null ? "" : compnayModel.Country), "Country");
                            content.Add(new StringContent("true"), "IsCashCompany");
                            //  var result1 = client.PostAsync("http://itmolen-001-site8.htempurl.com/api/Company/Add", content).Result;
                            var result = webServices.PostMultiPart(content, "Company/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                ViewBag.Message = "Created";
                            }
                            else
                            {
                                ViewBag.Message = "Failed";
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
    }
}