using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    [ExceptionLog]
    [Autintication]
    public class EmployeeController : Controller
    {
        WebServices webServices = new WebServices();
        readonly List<CountryViewModel> CountryViewModel = new List<CountryViewModel>();
        readonly List<DesignationViewModel> designationViewModels = new List<DesignationViewModel>();
        List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();
        EmployeeViewModel employeeViewModel = new EmployeeViewModel();
        int CompanyId = 0;
        /**/
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                
                    var results = webServices.Post(new EmployeeViewModel(), "AWFEmployee/All/" + CompanyId);
                    if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString());

                        
                    }
               
                return View(employeeViewModels);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult ChangeStatus(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var Result = new ServiceResponseModel();

                employeeViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);
                Result = webServices.Post(employeeViewModel, "AWFEmployee/ChangeStatus");

                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                   
                    return Json("success", JsonRequestBehavior.AllowGet);


                }
                return Json("success", JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult Details(int Id)
        {
            CompanyId = Convert.ToInt32(Session["CompanyId"]);
            employeeViewModel.Id = Id;
            employeeViewModel.CompanyId = CompanyId;

            var result = webServices.Post(new EmployeeViewModel(), "AWFEmployee/Edit/" + Id);

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (result.Data != null)
                {
                    employeeViewModel = (new JavaScriptSerializer().Deserialize<EmployeeViewModel>(result.Data.ToString()));
                }
            }

            return View(employeeViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            DesignationController designationController = new DesignationController();
            ViewBag.Designations = designationController.Designations();
            return View();
        }
               
        [HttpPost]
        public ActionResult Create(EmployeeViewModel employeeViewModel)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase[] httpPostedFileBase = new HttpPostedFileBase[2];
                    if (employeeViewModel.PassportFrontFile != null)
                    {
                        httpPostedFileBase[0] = employeeViewModel.PassportFrontFile;
                    }
                    if (employeeViewModel.PassportBackFile != null)
                    {
                        httpPostedFileBase[1] = employeeViewModel.PassportBackFile;
                    }
                   

                    var file = employeeViewModel.PassportFrontFile;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            if (httpPostedFileBase.ToList().Count > 0)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    if (httpPostedFileBase[i] != null)
                                    {
                                        file = httpPostedFileBase[i];

                                        byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                        file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                        var fileContent = new ByteArrayContent(fileBytes);

                                        if (i == 0)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportFront") { FileName = file.FileName };
                                        }
                                        else if (i == 1)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportBack") { FileName = file.FileName };
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
                            content.Add(new StringContent(employeeViewModel.Designation.ToString()), "Designation");
                            content.Add(new StringContent(employeeViewModel.Name ?? ""), "Name");
                            content.Add(new StringContent(employeeViewModel.Nationality ?? ""), "Nationality");
                            content.Add(new StringContent(employeeViewModel.Facebook ?? ""), "Facebook");
                            content.Add(new StringContent(employeeViewModel.Comments ?? ""), "Comments");
                            content.Add(new StringContent(employeeViewModel.Nation ?? ""), "Nation");
                            content.Add(new StringContent(employeeViewModel.Email ?? ""), "Email");
                            content.Add(new StringContent(employeeViewModel.Contact ?? ""), "Contact");
                            content.Add(new StringContent(employeeViewModel.BasicSalary.ToString() ?? ""), "BasicSalary");
                                                       
                            var result = webServices.PostMultiPart(content, "AWFEmployee/Add", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Index));
                            }
                        }
                    }
                }
                return Redirect(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(EmployeeViewModel employeeViewModel)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase[] httpPostedFileBase = new HttpPostedFileBase[2];
                    if (employeeViewModel.PassportFrontFile != null)
                    {
                        httpPostedFileBase[0] = employeeViewModel.PassportFrontFile;
                    }
                    if (employeeViewModel.PassportBackFile != null)
                    {
                        httpPostedFileBase[1] = employeeViewModel.PassportBackFile;
                    }


                    var file = employeeViewModel.PassportFrontFile;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            if (httpPostedFileBase.ToList().Count > 0)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    if (httpPostedFileBase[i] != null)
                                    {
                                        file = httpPostedFileBase[i];

                                        byte[] fileBytes = new byte[file.InputStream.Length + 1];
                                        file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                        var fileContent = new ByteArrayContent(fileBytes);

                                        if (i == 0)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportFront") { FileName = file.FileName };
                                        }
                                        else if (i == 1)
                                        {
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("PassportBack") { FileName = file.FileName };
                                        }

                                        content.Add(fileContent);
                                    }
                                }
                            }

                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "UpdatedBy");
                            CompanyId = Convert.ToInt32(Session["CompanyId"]);
                            content.Add(new StringContent(CompanyId.ToString()), "CompanyId");
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            content.Add(new StringContent(employeeViewModel.Designation.ToString()), "Designation");
                            content.Add(new StringContent(employeeViewModel.Name ?? ""), "Name");
                            content.Add(new StringContent(employeeViewModel.Id.ToString()), "Id");
                            content.Add(new StringContent(employeeViewModel.Nationality ?? ""), "Nationality");
                            content.Add(new StringContent(employeeViewModel.Facebook ?? ""), "Facebook");
                            content.Add(new StringContent(employeeViewModel.Comments ?? ""), "Comments");
                            content.Add(new StringContent(employeeViewModel.Nation ?? ""), "Nation");
                            content.Add(new StringContent(employeeViewModel.Email ?? ""), "Email");
                            content.Add(new StringContent(employeeViewModel.Contact ?? ""), "Contact");
                            content.Add(new StringContent(employeeViewModel.BasicSalary.ToString() ?? ""), "BasicSalary");

                            var result = webServices.PostMultiPart(content, "AWFEmployee/Update", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Index));
                            }
                        }
                    }
                }
                return Redirect(nameof(Index));
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
                 employeeViewModel = new EmployeeViewModel();
                var employeeResult = webServices.Post(new EmployeeViewModel(), "AWFEmployee/Edit/" + Id);

                DesignationController designationController = new DesignationController();
                ViewBag.Designations = designationController.Designations();

                if (employeeResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if(employeeResult.Data !=null)
                    {
                         employeeViewModel = (new JavaScriptSerializer().Deserialize<EmployeeViewModel>(employeeResult.Data.ToString()));
                    }
                }
                return View(employeeViewModel);
                
            }
            catch (Exception ex)
            {

                throw ex;
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
    }
}