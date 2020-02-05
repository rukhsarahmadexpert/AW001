using CrystalDecisions.CrystalReports.Engine;
using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    public class ReportCustomerController : Controller
    {
        List<IT.Web.Models.ReportsByDatesViewModel> reportsByDatesViewModels = new List<IT.Web.Models.ReportsByDatesViewModel>();

        WebServices webServices = new WebServices();

        public ActionResult Index()
        {
            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.fDate = System.DateTime.Now;
            searchViewModel.Tdate = System.DateTime.Now;

            return View(searchViewModel);
        }

        public ActionResult ReportByVehicleAndDates(SearchViewModel searchViewModel)
        {
            var result = webServices.Post(searchViewModel, "AWReports/ReportByVehicleAndDates", false);

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                reportsByDatesViewModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.ReportsByDatesViewModel>>(result.Data.ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult RepoOrdersByDates(SearchViewModel searchViewModel)
        {
            try
            {
                searchViewModel.FromDate = searchViewModel.fDate.ToString();
                searchViewModel.ToDate = searchViewModel.Tdate.ToString();
                searchViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);

                List<IT.Web.Models.ReportsByDatesVehicleDetails> reportsByDatesVehicleDetails = new List<Web.Models.ReportsByDatesVehicleDetails>();

                if (searchViewModel.searchkey != null && searchViewModel.Flage == "OrderReport")
                {
                    var result = webServices.Post(searchViewModel, "AWReports/RepoOrdersByDates", false);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        string pdfname = "";
                        ReportDocument Report = new ReportDocument();
                        Report.Load(Server.MapPath("~/Reports/CustomerReport/ReportsByDatesVehicleDetails.rpt"));

                        reportsByDatesViewModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.ReportsByDatesViewModel>>(result.Data.ToString());

                        foreach (var item in reportsByDatesViewModels)
                        {
                            foreach (var innerItem in item.reportsByDatesVehicleDetails)
                            {
                                reportsByDatesVehicleDetails.Add(innerItem);
                            }
                        }

                        Report.Database.Tables[0].SetDataSource(reportsByDatesVehicleDetails);

                        Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stram.Seek(0, SeekOrigin.Begin);

                        string FileName = "";

                        if (reportsByDatesVehicleDetails.Count > 0)
                        {
                            FileName = reportsByDatesVehicleDetails[0].Id + " " + reportsByDatesVehicleDetails[0].CustomerOrderNumber;
                        }
                        else
                        {
                            FileName = "Data Not Found";
                        }

                        var root = Server.MapPath("/PDF/");
                        pdfname = String.Format("{0}.pdf", FileName);
                        var path = Path.Combine(root, pdfname);
                        path = Path.GetFullPath(path);

                        //Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        //  stram.Close();

                        stram.Seek(0, SeekOrigin.Begin);
                        return new FileStreamResult(stram, "application/pdf");
                    }
                }

                else if (searchViewModel.searchkey != null && searchViewModel.Flage == "VehicleReport")
                {
                    if (searchViewModel.GroupBy == "ByVehicle")
                    {
                        searchViewModel.Id = Convert.ToInt32(searchViewModel.searchkey);
                        searchViewModel.searchkey = searchViewModel.SearchKey2;
                        var result = webServices.Post(searchViewModel, "AWReports/ReportByVehicleAndDateGroupByVehicle", false);

                        if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            reportsByDatesViewModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.ReportsByDatesViewModel>>(result.Data.ToString());

                            string pdfname = "";
                            ReportDocument Report = new ReportDocument();
                            Report.Load(Server.MapPath("~/Reports/CustomerReport/ReportGroupByVehicle.rpt"));

                            reportsByDatesViewModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.ReportsByDatesViewModel>>(result.Data.ToString());

                            foreach (var item in reportsByDatesViewModels)
                            {
                                foreach (var innerItem in item.reportsByDatesVehicleDetails)
                                {
                                    reportsByDatesVehicleDetails.Add(innerItem);
                                }
                            }

                            if (reportsByDatesVehicleDetails.Count < 1)
                            {
                                ModelState.AddModelError(searchViewModel.searchkey, "No Data Found in this dates and status");
                                return View("Index", searchViewModel);
                            }

                            Report.Database.Tables[0].SetDataSource(reportsByDatesVehicleDetails);

                            Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            stram.Seek(0, SeekOrigin.Begin);

                            string FileName = "";

                            if (reportsByDatesVehicleDetails.Count > 0)
                            {
                                FileName = reportsByDatesVehicleDetails[0].Id + " " + reportsByDatesVehicleDetails[0].CustomerOrderNumber;
                            }
                            else
                            {
                                FileName = "Data Not Found";
                            }

                            var root = Server.MapPath("/PDF/");
                            pdfname = String.Format("{0}.pdf", FileName);
                            var path = Path.Combine(root, pdfname);
                            path = Path.GetFullPath(path);

                            //Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                            //  stram.Close();

                            stram.Seek(0, SeekOrigin.Begin);
                            return new FileStreamResult(stram, "application/pdf");

                        }
                        return View();
                    }
                    else
                    {
                        searchViewModel.Id = Convert.ToInt32(searchViewModel.searchkey);
                        searchViewModel.searchkey = searchViewModel.SearchKey2;
                        var result = webServices.Post(searchViewModel, "AWReports/ReportByVehicleAndDates", false);

                        if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            reportsByDatesViewModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.ReportsByDatesViewModel>>(result.Data.ToString());

                            string pdfname = "";
                            ReportDocument Report = new ReportDocument();
                            Report.Load(Server.MapPath("~/Reports/CustomerReport/ReportsByDatesVehicleDetails.rpt"));

                            reportsByDatesViewModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.ReportsByDatesViewModel>>(result.Data.ToString());

                            foreach (var item in reportsByDatesViewModels)
                            {
                                foreach (var innerItem in item.reportsByDatesVehicleDetails)
                                {
                                    reportsByDatesVehicleDetails.Add(innerItem);
                                }
                            }

                            Report.Database.Tables[0].SetDataSource(reportsByDatesVehicleDetails);

                            Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            stram.Seek(0, SeekOrigin.Begin);

                            string FileName = "";

                            if (reportsByDatesVehicleDetails.Count > 0)
                            {
                                FileName = reportsByDatesVehicleDetails[0].Id + " " + reportsByDatesVehicleDetails[0].CustomerOrderNumber;
                            }
                            else
                            {
                                FileName = "Data Not Found";
                            }

                            var root = Server.MapPath("/PDF/");
                            pdfname = String.Format("{0}.pdf", FileName);
                            var path = Path.Combine(root, pdfname);
                            path = Path.GetFullPath(path);

                            //Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                            //  stram.Close();

                            stram.Seek(0, SeekOrigin.Begin);
                            return new FileStreamResult(stram, "application/pdf");
                        }
                        return View();
                    }

                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Demo()
        {
            List<EmployeeModel> employeeModels = new List<EmployeeModel>();
            int CompanyId = 2;
            var results = webServices.Post(new EmployeeViewModel(), "AWFEmployee/All/" + CompanyId);
            if (results.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                employeeModels = (new JavaScriptSerializer()).Deserialize<List<EmployeeModel>>(results.Data.ToString());

            }
            List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
            PagingParameterModel pagingParameterModel = new PagingParameterModel();
            pagingParameterModel.pageNumber = 1;
            pagingParameterModel._pageSize = 1;
            pagingParameterModel.PageSize = 100;
            var CompanyList = webServices.Post(pagingParameterModel, "Company/CompayAll");
            if (CompanyList.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                compnayModels = (new JavaScriptSerializer().Deserialize<List<IT.Web.Models.CompnayModel>>(CompanyList.Data.ToString()));
            }

            string pdfname = "";
            ReportDocument Report = new ReportDocument();
            Report.Load(Server.MapPath("~/Reports/Demo/RizwanDemo.rpt"));

            Report.Database.Tables[0].SetDataSource(employeeModels);
            Report.Database.Tables[1].SetDataSource(compnayModels);

            Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stram.Seek(0, SeekOrigin.Begin);


            var root = Server.MapPath("/PDF/");
            pdfname = String.Format("{0}.pdf", "Demo");
            var path = Path.Combine(root, pdfname);
            path = Path.GetFullPath(path);

            //Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

            //  stram.Close();

            stram.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stram, "application/pdf");

        }
    }
}