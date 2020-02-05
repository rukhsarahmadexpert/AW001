using CrystalDecisions.CrystalReports.Engine;
using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
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
    [Autintication]
    public class QuotationController : Controller
    {
        WebServices webServices = new WebServices();

        List<ProductViewModel> ProductViewModel = new List<ProductViewModel>();
        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        List<CustomerViewModel> customerViewModels = new List<CustomerViewModel>();
        LPOInvoiceViewModel lPOInvoiceViewModel = new LPOInvoiceViewModel();
        List<LPOInvoiceDetails> lPOInvoiceDetails = new List<LPOInvoiceDetails>();
        List<LPOInvoiceViewModel> lPOInvoiceViewModels = new List<LPOInvoiceViewModel>();
        List<CompanyViewModel> companyViewModels = new List<CompanyViewModel>();
        int CompanyId;

        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = CompanyId;
                pagingParameterModel.PageSize = 100;

                var SiteList = webServices.Post(new LPOInvoiceViewModel(), "Quotation/All");

                if (SiteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    lPOInvoiceViewModels = (new JavaScriptSerializer().Deserialize<List<LPOInvoiceViewModel>>(SiteList.Data.ToString()));
                }
                return View(lPOInvoiceViewModels);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            DataTablesParm parm = new DataTablesParm();
            try
            {
                int pageNo = 1;
                int totalCount = 0;
                parm.iDisplayLength = 20;
                parm.iDisplayStart = 0;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                //int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["QuotationData"] != null)
                {
                    lPOInvoiceViewModels = HttpContext.Cache["QuotationData"] as List<LPOInvoiceViewModel>;
                }
                else
                {
                    var result = webServices.Post(new VehicleViewModel(), "Quotation/All");
                    lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());

                    HttpContext.Cache["QuotationData"] = lPOInvoiceViewModels;
                }
                if (parm.sSearch != null)
                {
                    totalCount = lPOInvoiceViewModels.Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.GrandTotal.ToString().Contains(parm.sSearch) ||
                               x.UserName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.PONumber.ToString().Contains(parm.sSearch)).Count();

                    lPOInvoiceViewModels = lPOInvoiceViewModels.ToList()
                        .Where(x => x.Name.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.GrandTotal.ToString().Contains(parm.sSearch) ||
                               x.UserName.ToLower().Contains(parm.sSearch.ToLower()) ||
                               x.PONumber.ToString().Contains(parm.sSearch))
                               .Skip((pageNo - 1) * parm.iDisplayLength)
                               .Take(parm.iDisplayLength)
                   .Select(x => new LPOInvoiceViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Total = x.Total,
                       VAT = x.VAT,
                       GrandTotal = x.GrandTotal,
                       UserName = x.UserName,
                       PONumber = x.PONumber,
                       FDate = x.FDate,
                       DDate = x.DDate,
                       IsUpdated = x.IsUpdated
                   }).ToList();
                }
                else
                {
                    totalCount = lPOInvoiceViewModels.Count();

                    lPOInvoiceViewModels = lPOInvoiceViewModels
                                                       .Skip((pageNo - 1) * parm.iDisplayLength)
                                                       .Take(parm.iDisplayLength)
                        .Select(x => new LPOInvoiceViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Total = x.Total,
                            VAT = x.VAT,
                            GrandTotal = x.GrandTotal,
                            UserName = x.UserName,
                            PONumber = x.PONumber,
                            FDate = x.FDate,
                            DDate = x.DDate,
                            IsUpdated = x.IsUpdated

                        }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = lPOInvoiceViewModels,
                        sEcho = parm.sEcho,
                        iTotalDisplayRecords = totalCount,
                        data = lPOInvoiceViewModels,
                        iTotalRecords = totalCount,
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Create()
        {
            try
            {
                string SerailNO = "";

                SerailNO = PoNumber();

                var result = webServices.Post(new ProductViewModel(), "Product/All");
                ProductViewModel = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());
                ProductViewModel.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                ViewBag.Product = ProductViewModel;

                var results = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");
                productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(results.Data.ToString());
                productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
                ViewBag.ProductUnit = productUnitViewModels;

                var Res = webServices.Post(new CompanyViewModel(), "Company/CompayAll");
                companyViewModels = (new JavaScriptSerializer()).Deserialize<List<CompanyViewModel>>(Res.Data.ToString());
                companyViewModels.Insert(0, new CompanyViewModel() { Id = 0, Name = "Select Customer Name" });

                ViewBag.Vender = companyViewModels;
                ViewBag.PO = SerailNO;
                ViewBag.titles = "Quotation";

                LPOInvoiceViewModel lPOInvoiceVModel = new LPOInvoiceViewModel();

                lPOInvoiceVModel.FromDate = System.DateTime.Now;
                lPOInvoiceVModel.DueDate = System.DateTime.Now;

                return View(lPOInvoiceVModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string PoNumber()
        {
            string SerailNO = "";
            var LPONoResult = webServices.Post(new SingleStringValueResult(), "Quotation/QuotaNumber");
            if (LPONoResult.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                if (LPONoResult.Data != "[]")
                {
                    string LPNo = (new JavaScriptSerializer()).Deserialize<string>(LPONoResult.Data);


                    SerailNO = LPNo.Substring(4, 8);

                    SerailNO = SerailNO.ToString().Substring(0, 6);


                    string TotdayNumber = POClass.PONumber().Substring(0, 6);
                    int Counts = 0;
                    if (SerailNO == TotdayNumber)
                    {
                        Counts = Convert.ToInt32(LPNo.Substring(10, 2)) + 1;

                        if (Counts.ToString().Length == 1)
                        {
                            SerailNO = "QUO-" + TotdayNumber + "0" + Counts;
                        }
                        else
                        {
                            SerailNO = "QUO-" + TotdayNumber + Counts.ToString();
                        }
                    }
                    else
                    {
                        SerailNO = "QUO-" + POClass.PONumber();
                    }
                }
                else
                {
                    SerailNO = "QUO-" + POClass.PONumber();
                }

            }
            else
            {
                SerailNO = "QUO-" + POClass.PONumber();
            }

            return SerailNO;
        }

        [HttpPost]
        public ActionResult Create(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                lPOInvoiceViewModel.FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                lPOInvoiceViewModel.DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                lPOInvoiceViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var result = webServices.Post(lPOInvoiceViewModel, "Quotation/Add");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("QuotationData");
                        TempData["Id"] = Res;


                        return Json(Res, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(result.Data.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(result.Data.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }

        [HttpGet]
        public ActionResult Details(int? Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Quotation/Edit/" + Id);

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;
                    lPOInvoiceViewModel.Heading = "Quotation";
                    var Results = webServices.Post(new LPOInvoiceDetails(), "Quotation/EditDetails/" + Id);

                    if (Results.Data != "[]")
                    {
                        lPOInvoiceDetails = (new JavaScriptSerializer().Deserialize<List<LPOInvoiceDetails>>(Results.Data.ToString()));
                        ViewBag.lPOInvoiceDetails = lPOInvoiceDetails;

                        if (TempData["Success"] == null)
                        {
                            if (TempData["Download"] != null)
                            {
                                ViewBag.IsDownload = TempData["Download"].ToString();
                                ViewBag.Id = Id;
                            }
                        }
                        else
                        {
                            ViewBag.Success = TempData["Success"];
                        }

                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Quotation/Edit/" + Id);

                var result = webServices.Post(new ProductViewModel(), "Product/All");

                if (result.Data != "[]")
                {
                    ProductViewModel = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());
                }
                if (ProductViewModel.Count > 0)
                {
                    if (ProductViewModel[0].Name != "Select Item")
                    {
                        ProductViewModel.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                    }
                }
                ViewBag.Product = ProductViewModel;

                var results = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");

                if (results.Data != "[]")
                {
                    productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(results.Data.ToString());
                }

                if (productUnitViewModels.Count > 0)
                {
                    if (productUnitViewModels[0].Name != "Select Unit")
                    {
                        productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
                    }
                }
                ViewBag.ProductUnit = productUnitViewModels;

                List<VatModel> model = new List<VatModel>();
                model.Add(new VatModel() { Id = 0, VAT = 0 });
                model.Add(new VatModel() { Id = 5, VAT = 5 });
                ViewBag.VatDrop = model;

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    var Results = webServices.Post(new LPOInvoiceDetails(), "Quotation/EditDetails/" + Id);

                    if (Results.Data != "[]")
                    {
                        lPOInvoiceDetails = (new JavaScriptSerializer().Deserialize<List<LPOInvoiceDetails>>(Results.Data.ToString()));
                        ViewBag.lPOInvoiceDetails = lPOInvoiceDetails;

                        lPOInvoiceViewModel.Heading = "Quotation";

                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                lPOInvoiceViewModel.FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                lPOInvoiceViewModel.DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                var result = webServices.Post(lPOInvoiceViewModel, "Quotation/Update");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("LPOData");

                        if (lPOInvoiceViewModel.IsDownload != null)
                        {
                            TempData["Download"] = "True";
                        }

                        TempData["Id"] = Res;

                        int Download = UploadFileToFolder(Res);

                        return Json(Res, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteQuotationDetailsRow(DeleteRowViewModel deleteRowViewModel)
        {
            try
            {

                decimal ResultVAT = CalculateVat(deleteRowViewModel.VAT, deleteRowViewModel.RowTotal);

                lPOInvoiceViewModel.lPOInvoiceDetailsList = new List<LPOInvoiceDetails>();

                var LPOData = webServices.Post(new LPOInvoiceViewModel(), "Quotation/Edit/" + deleteRowViewModel.Id);
                if (LPOData.Data != "")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer()).Deserialize<LPOInvoiceViewModel>(LPOData.Data.ToString());
                }
                lPOInvoiceViewModel.Total = lPOInvoiceViewModel.Total - deleteRowViewModel.RowTotal;
                lPOInvoiceViewModel.GrandTotal = lPOInvoiceViewModel.GrandTotal - ResultVAT;
                lPOInvoiceViewModel.GrandTotal = lPOInvoiceViewModel.GrandTotal - deleteRowViewModel.RowTotal;
                lPOInvoiceViewModel.VAT = lPOInvoiceViewModel.VAT - ResultVAT;
                lPOInvoiceViewModel.detailId = deleteRowViewModel.detailId;

                var result = webServices.Post(lPOInvoiceViewModel, "Quotation/DeleteDeatlsRow");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                    return Json("Success", JsonRequestBehavior.AllowGet);


                    //var deletequtation = webServices.Post(lPOInvoiceViewModel, "LPO/DeleteDeatlsRow");
                    //if (deletequtation.IsSuccess == true)
                    //{
                    //    return Json("Success", JsonRequestBehavior.AllowGet);
                    //}
                    //return new JsonResult { Data = new { Status = "Success" } };

                }
                return new JsonResult { Data = new { Status = "Fail" } };
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { Status = "Fail" } };
            }
        }

        public static decimal CalculateVat(decimal vat, decimal Total)
        {
            decimal Result = 0;
            try
            {
                Result = Convert.ToDecimal((Total / 100) * vat);
                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }

        [HttpPost]
        public ActionResult SaveDwnload(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                var result = webServices.Post(lPOInvoiceViewModel, "Quotation/Add");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("QuotationData");

                        TempData["Download"] = "True";

                        //TempData.Keep();

                        string FileName = Res + "-" + lPOInvoiceViewModel.PONumber + ".pdf";
                        TempData["FileName"] = FileName;

                        TempData["Id"] = Res;

                        int Download = UploadFileToFolder(Res);

                        return Json(Res, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public int UploadFileToFolder(int Id)
        {
            string pdfname = "";

            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/LPO-Invoice/LPOInvoice.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<Web.Models.CompnayModel>();
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<IT.Web.Models.LPOInvoiceModel>();
                List<LPOInvoiceDetailsModel> lPOInvoiceDetails = new List<LPOInvoiceDetailsModel>();
                List<VenderModel> venderModels = new List<VenderModel>();

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                Web.Models.LPOInvoiceModel lPOInvoiceModel = new Web.Models.LPOInvoiceModel();
                lPOInvoiceModel.Id = Id;
                lPOInvoiceModel.detailId = CompanyId;

                var LPOInvoice = webServices.Post(lPOInvoiceModel, "Quotation/EditReport/" + Id);

                if (LPOInvoice.Data != "[]")
                {
                    lPOInvoiceModel = (new JavaScriptSerializer()).Deserialize<IT.Web.Models.LPOInvoiceModel>(LPOInvoice.Data.ToString());
                }

                lPOInvoiceModels.Add(lPOInvoiceModel);
                venderModels = lPOInvoiceModel.venders;
                compnayModels = lPOInvoiceModel.compnays;
                lPOInvoiceDetails = lPOInvoiceModel.lPOInvoiceDetailsList;

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(lPOInvoiceModels);
                Report.Database.Tables[3].SetDataSource(lPOInvoiceDetails);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = Id + "-" + lPOInvoiceModels[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                stram.Close();

                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult PrintQuotation(int Id)
        {
            string pdfname = "";
            try
            {

                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/LPO-Invoice/LPOInvoice.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<Web.Models.CompnayModel>();
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<IT.Web.Models.LPOInvoiceModel>();
                List<LPOInvoiceDetailsModel> lPOInvoiceDetails = new List<LPOInvoiceDetailsModel>();
                List<VenderModel> venderModels = new List<VenderModel>();

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                Web.Models.LPOInvoiceModel lPOInvoiceModel = new Web.Models.LPOInvoiceModel();
                lPOInvoiceModel.Id = Id;
                lPOInvoiceModel.detailId = CompanyId;

                var LPOInvoice = webServices.Post(lPOInvoiceModel, "Quotation/EditReport/" + Id);

                if (LPOInvoice.Data != "[]")
                {
                    lPOInvoiceModel = (new JavaScriptSerializer()).Deserialize<IT.Web.Models.LPOInvoiceModel>(LPOInvoice.Data.ToString());
                }

                lPOInvoiceModels.Add(lPOInvoiceModel);
                venderModels = lPOInvoiceModel.venders;
                compnayModels = lPOInvoiceModel.compnays;
                lPOInvoiceDetails = lPOInvoiceModel.lPOInvoiceDetailsList;

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(lPOInvoiceModels);
                Report.Database.Tables[3].SetDataSource(lPOInvoiceDetails);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                string companyName = Id + "-" + lPOInvoiceModels[0].PONumber;

                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //stram.Close();


                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                //Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stram.Seek(0, SeekOrigin.Begin);

                return new FileStreamResult(stram, "application/pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult CheckISFileExist(int Id)
        {
            try
            {

                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<Web.Models.LPOInvoiceModel>();
                var LPOInvoice = webServices.Post(new Web.Models.LPOInvoiceModel(), "Quotation/EditReport/" + Id);
                if (LPOInvoice.Data != "")
                {
                    lPOInvoiceModels = (new JavaScriptSerializer()).Deserialize<List<IT.Web.Models.LPOInvoiceModel>>(LPOInvoice.Data.ToString());
                }

                string companyName;
                if (lPOInvoiceModels.Count > 0)
                {
                    companyName = Id + "-" + lPOInvoiceModels[0].PONumber;
                }
                else
                {
                    companyName = "No Data Found";
                }
                string FileName = companyName + ".pdf";

                if (System.IO.File.Exists(Server.MapPath("~/PDF/" + FileName)))
                {
                    TempData["Id"] = Id;
                    TempData["FileName"] = FileName;
                    return Json("Exist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int Res = UploadFileToFolder(Id);
                    if (Res > 0)
                    {
                        TempData["FileName"] = FileName;
                        return Json("Exist", JsonRequestBehavior.AllowGet);
                    }

                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult MakeInvoice(int Id)
        {
            try
            {
                string SerailNO = "";
                InvoicePNNumber invoicePNNumber = new InvoicePNNumber();

                SerailNO = "QUO-001";

                var Result = webServices.Post(new LPOInvoiceViewModel(), "Quotation/Edit/" + Id);

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;
                    lPOInvoiceViewModel.Heading = "Invoice";
                    var Results = webServices.Post(new LPOInvoiceDetails(), "Quotation/EditDetails/" + Id);

                    if (Results.Data != "[]")
                    {
                        lPOInvoiceDetails = (new JavaScriptSerializer().Deserialize<List<LPOInvoiceDetails>>(Results.Data.ToString()));
                        ViewBag.lPOInvoiceDetails = lPOInvoiceDetails;

                        if (TempData["Success"] == null)
                        {
                            if (TempData["Download"] != null)
                            {
                                ViewBag.IsDownload = TempData["Download"].ToString();
                                ViewBag.Id = Id;
                            }
                        }
                        else
                        {
                            ViewBag.Success = TempData["Success"];
                        }

                        lPOInvoiceViewModel.RefrenceNumber = lPOInvoiceViewModel.PONumber;
                        lPOInvoiceViewModel.PONumber = SerailNO;

                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}