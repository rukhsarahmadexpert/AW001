﻿using IT.Core.ViewModels;
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
    public class EmployeeController : Controller
    {
        WebServices webServices = new WebServices();
        List<CountryViewModel> CountryViewModel = new List<CountryViewModel>();
        List<DesignationViewModel> designationViewModels = new List<DesignationViewModel>();
        List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();
        EmployeeViewModel employeeViewModel = new EmployeeViewModel();
        int CompanyId = 0;
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["EmployeeDatas"] != null)
                {

                    employeeViewModels = HttpContext.Cache["EmployeeDatas"] as List<EmployeeViewModel>;
                }
                else
                {
                    var results = webServices.Post(new EmployeeViewModel(), "AWFEmployee/All/" + CompanyId);
                    if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        employeeViewModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeViewModel>>(results.Data.ToString());

                        HttpContext.Cache["EmployeeDatas"] = employeeViewModels;
                    }
                }

                return View(employeeViewModels);


            }
            catch (Exception ex)
            {
                throw ex;
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
                if (result.Data != "[]")
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
                            content.Add(new StringContent(employeeViewModel.Name == null ? "" : employeeViewModel.Name), "Name");
                            content.Add(new StringContent(employeeViewModel.LastName == null ? "" : employeeViewModel.LastName), "LastName");
                            content.Add(new StringContent(employeeViewModel.Nationality == null ? "" : employeeViewModel.Nationality), "Nationality");
                            content.Add(new StringContent(employeeViewModel.Facebook == null ? "" : employeeViewModel.Facebook), "Facebook");
                            content.Add(new StringContent(employeeViewModel.Comments == null ? "" : employeeViewModel.Comments), "Comments");
                            content.Add(new StringContent(employeeViewModel.Nation == null ? "" : employeeViewModel.Nation), "Nation");
                            content.Add(new StringContent(employeeViewModel.Email == null ? "" : employeeViewModel.Email), "Email");
                            content.Add(new StringContent(employeeViewModel.Contact == null ? "" : employeeViewModel.Contact), "Contact");
                            content.Add(new StringContent(employeeViewModel.BasicSalary.ToString() == null ? "" : employeeViewModel.BasicSalary.ToString()), "BasicSalary");



                            var result = webServices.PostMultiPart(content, "Employee/Add", true);
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
        public ActionResult Edit(int id)
        {
            return View();
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