using CrystalDecisions.CrystalReports.Engine;
using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using IT.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    [Autintication]
    [ExceptionLog]
    public class BillController : Controller
    {
        WebServices webServices = new WebServices();
        List<ProductViewModel> ProductViewModel = new List<ProductViewModel>();
        List<ProductUnitViewModel> productUnitViewModels = new List<ProductUnitViewModel>();
        List<VenderViewModel> venderViewModels = new List<VenderViewModel>();
        LPOInvoiceViewModel lPOInvoiceViewModel = new LPOInvoiceViewModel();
        List<LPOInvoiceDetails> lPOInvoiceDetails = new List<LPOInvoiceDetails>();
        List<LPOInvoiceViewModel> lPOInvoiceViewModels = new List<LPOInvoiceViewModel>();
        
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                
                var result = webServices.Post(new VehicleViewModel(), "LPo/LPOUnconvertedALL");
                lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());

                lPOInvoiceViewModels.Insert(0, new LPOInvoiceViewModel() { Id = 0, PONumber = "select LPO Number" });
                ViewBag.LPO = lPOInvoiceViewModels;

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create(int Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "LPO/Edit/" + Id);

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    SearchViewModel searchViewModel = new SearchViewModel {
                        Id = Id
                    };
                    var ResultRemainingQuanntity = webServices.Post(searchViewModel, "Bill/LPOGetRemainingDetails");

                    LpoRemainingQuantityViewModel lpoRemainingQuantityViewModel = new LpoRemainingQuantityViewModel();
                    lpoRemainingQuantityViewModel = (new JavaScriptSerializer().Deserialize<LpoRemainingQuantityViewModel>(ResultRemainingQuanntity.Data.ToString()));

                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;
                    ViewBag.lpoRemainingQuantityViewModel = lpoRemainingQuantityViewModel;

                    lPOInvoiceViewModel.Heading = "BILL";

                    // lPOInvoiceDetails = lPOInvoiceViewModel.lPOInvoiceDetailsList;
                    ViewBag.lPOInvoiceDetails = lPOInvoiceDetails;

                    HttpContext.Cache.Remove("LPOData");

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
                    BillPONumber billPONumber = new BillPONumber();

                    lPOInvoiceViewModel.PONumber = "Bill-001";

                    LPOInvoiceViewModel lPOInvoiceVModel = new LPOInvoiceViewModel
                    { 
                        FromDate = System.DateTime.Now,
                        DueDate = System.DateTime.Now,
                    };
                    var result = webServices.Post(new ProductViewModel(), "Product/All");
                    ProductViewModel = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());
                    ProductViewModel.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                    ViewBag.Product = ProductViewModel;

                    var results = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");
                    productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(results.Data.ToString());
                    productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
                    ViewBag.ProductUnit = productUnitViewModels;

                    var Res = webServices.Post(new DriverViewModel(), "Vender/All");
                    venderViewModels = (new JavaScriptSerializer()).Deserialize<List<VenderViewModel>>(Res.Data.ToString());
                    venderViewModels.Insert(0, new VenderViewModel() { Id = 0, Name = "Select Vender" });

                    ViewBag.Vender = venderViewModels;

                    List<VatModel> model = new List<VatModel>
                    {
                        new VatModel() { Id = 0, VAT = 0 },
                        new VatModel() { Id = 5, VAT = 5 }
                    };
                    ViewBag.VatDrop = model;

                    return View(lPOInvoiceVModel);
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
        public ActionResult Create(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {

               // return Json("Success",JsonRequestBehavior.AllowGet);

                lPOInvoiceViewModel.FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                lPOInvoiceViewModel.DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                lPOInvoiceViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                var result = webServices.Post(lPOInvoiceViewModel, "BILL/Add");
                int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                if (Res > 0)
                {
                    HttpContext.Cache.Remove("AWFuelBillData");
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
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
                parm.iDisplayStart = 0;
                parm.iDisplayLength = 20;

                if (parm.iDisplayStart >= parm.iDisplayLength)
                {
                    pageNo = (parm.iDisplayStart / parm.iDisplayLength) + 1;
                }

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                if (HttpContext.Cache["AWFuelBillData"] != null)
                {

                    lPOInvoiceViewModels = HttpContext.Cache["AWFuelBillData"] as List<LPOInvoiceViewModel>;
                }

                else
                {

                    var result = webServices.Post(new LPOInvoiceViewModel(), "Bill/All");
                    lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());

                    HttpContext.Cache["AWFuelBillData"] = lPOInvoiceViewModels;
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
                       IsUpdated = x.IsUpdated,
                       RefrenceNumber = x.RefrenceNumber
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
                            IsUpdated = x.IsUpdated,
                            RefrenceNumber = x.RefrenceNumber

                        }).ToList();
                }
                return Json(
                    new
                    {
                        aaData = lPOInvoiceViewModels,
                        parm.sEcho,
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

        [HttpGet]
        public ActionResult BillAllByLpoId(int Id)
        {
            try
            {
                SearchViewModel searchViewModel = new SearchViewModel
                { 
                     Id = Id
                };
            var result = webServices.Post(searchViewModel, "Bill/BillAllByLpoId");
                lPOInvoiceViewModels = (new JavaScriptSerializer()).Deserialize<List<LPOInvoiceViewModel>>(result.Data.ToString());
                
                return View(lPOInvoiceViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Bill/Details/" + Id);

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    lPOInvoiceViewModel.Heading = "BILL";
                    lPOInvoiceDetails = lPOInvoiceViewModel.lPOInvoiceDetailsList;
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
                    ViewBag.RefrenceNumber = lPOInvoiceViewModel.RefrenceNumber;                   
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Bill/Delete/" + Id);

                int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));
                if (Res > 0)
                {
                    HttpContext.Cache.Remove("AWFuelBillData");
                    return Json("Success", JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {
                var Result = webServices.Post(new LPOInvoiceViewModel(), "Bill/Details/" + Id);

                var result = webServices.Post(new ProductViewModel(), "Product/All");
                ProductViewModel = (new JavaScriptSerializer()).Deserialize<List<ProductViewModel>>(result.Data.ToString());
                ProductViewModel.Insert(0, new ProductViewModel() { Id = 0, Name = "Select Item" });
                ViewBag.Product = ProductViewModel;

                var results = webServices.Post(new ProductUnitViewModel(), "ProductUnit/All");
                productUnitViewModels = (new JavaScriptSerializer()).Deserialize<List<ProductUnitViewModel>>(results.Data.ToString());
                productUnitViewModels.Insert(0, new ProductUnitViewModel() { Id = 0, Name = "Select Unit" });
                ViewBag.ProductUnit = productUnitViewModels;


                List<VatModel> model = new List<VatModel>
                { 
                new VatModel() { Id = 0, VAT = 0 },
                new VatModel() { Id = 5, VAT = 5 },
                };
                ViewBag.VatDrop = model;

                if (Result.Data != "[]")
                {
                    lPOInvoiceViewModel = (new JavaScriptSerializer().Deserialize<LPOInvoiceViewModel>(Result.Data.ToString()));
                    ViewBag.lPOInvoiceViewModel = lPOInvoiceViewModel;

                    lPOInvoiceViewModel.Heading = "BILL";
                    lPOInvoiceDetails = lPOInvoiceViewModel.lPOInvoiceDetailsList;
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

                    SearchViewModel searchViewModel = new SearchViewModel();
                    searchViewModel.Id = Id;

                    var ResultRemainingQuanntity = webServices.Post(searchViewModel, "Bill/LPOGetRemainingDetails");

                    LpoRemainingQuantityViewModel lpoRemainingQuantityViewModel = new LpoRemainingQuantityViewModel();
                    lpoRemainingQuantityViewModel = (new JavaScriptSerializer().Deserialize<LpoRemainingQuantityViewModel>(ResultRemainingQuanntity.Data.ToString()));

                    ViewBag.lpoRemainingQuantityViewModel = lpoRemainingQuantityViewModel;

                    ViewBag.RefrenceNumber = lPOInvoiceViewModel.RefrenceNumber;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult DeleteBillDetailsRow(DeleteRowViewModel deleteRowViewModel)
        {
            try
            {

                decimal ResultVAT = CalculateVat(deleteRowViewModel.VAT, deleteRowViewModel.RowTotal);

                lPOInvoiceViewModel.lPOInvoiceDetailsList = new List<LPOInvoiceDetails>();

                var LPOData = webServices.Post(new LPOInvoiceViewModel(), "Bill/Details/" + deleteRowViewModel.Id);
                lPOInvoiceViewModel = (new JavaScriptSerializer()).Deserialize<LPOInvoiceViewModel>(LPOData.Data.ToString());

                lPOInvoiceViewModel.Total = lPOInvoiceViewModel.Total - deleteRowViewModel.RowTotal;
                lPOInvoiceViewModel.GrandTotal = lPOInvoiceViewModel.GrandTotal - ResultVAT;
                lPOInvoiceViewModel.GrandTotal = lPOInvoiceViewModel.GrandTotal - deleteRowViewModel.RowTotal;
                lPOInvoiceViewModel.VAT = lPOInvoiceViewModel.VAT - ResultVAT;
                lPOInvoiceViewModel.detailId = deleteRowViewModel.detailId;

                var result = webServices.Post(lPOInvoiceViewModel, "Bill/DeleteDetailsRow");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                   // HttpContext.Cache.Remove("InvoiceData");
                    return Json("Success", JsonRequestBehavior.AllowGet);

                }
                return new JsonResult { Data = new { Status = "Fail" } };
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { Status = "Fail" } };
            }
        }

        [HttpPost]
        public ActionResult Update(LPOInvoiceViewModel lPOInvoiceViewModel)
        {
            try
            {
                lPOInvoiceViewModel.FromDate = Convert.ToDateTime(lPOInvoiceViewModel.FromDate);
                lPOInvoiceViewModel.DueDate = Convert.ToDateTime(lPOInvoiceViewModel.DueDate);

                var result = webServices.Post(lPOInvoiceViewModel, "Bill/Update");

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer()).Deserialize<int>(result.Data);

                        HttpContext.Cache.Remove("InvoiceData");

                        if (lPOInvoiceViewModel.IsDownload != null)
                        {
                            TempData["Download"] = "True";
                        }

                        TempData["Id"] = Res;
                        TempData["FileName"] = Res + "-" + lPOInvoiceViewModel.PONumber + ".pdf";
                        int Download = UploadFileToFolder(Res);

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

                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
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

                List<IT.Web.Models.CompnayModel> compnayModels = new List<IT.Web.Models.CompnayModel>();
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<IT.Web.Models.LPOInvoiceModel>();
                List<IT.Web.Models.LPOInvoiceDetailsModel> lPOInvoiceDetails = new List<IT.Web.Models.LPOInvoiceDetailsModel>();
                List<VenderModel> venderModels = new List<VenderModel>();

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var lPOInvoiceModel = new IT.Web.Models.LPOInvoiceModel
                { 
                    Id = Id,
                    detailId = CompanyId,
                };
                var LPOInvoice = webServices.Post(lPOInvoiceModel, "Invoice/EditReport");
                lPOInvoiceModel = (new JavaScriptSerializer()).Deserialize<IT.Web.Models.LPOInvoiceModel>(LPOInvoice.Data.ToString());

                lPOInvoiceDetails = lPOInvoiceModel.lPOInvoiceDetailsList;
                compnayModels = lPOInvoiceModel.compnays;
                lPOInvoiceModels.Insert(0, lPOInvoiceModel);
                venderModels = lPOInvoiceModel.venders;

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
        public static decimal CalculateVat(decimal vat, decimal Total)
        {
            decimal Result = 0;
            try
            {
                Result = Convert.ToDecimal((Total / 100) * vat);
                return Result;
            }
            catch (Exception)
            {
                return Result;
            }
        }
        
        [HttpGet]
        public ActionResult PrintBill(int Id)
        {
            string pdfname = "";
            try
            {
                ReportDocument Report = new ReportDocument();
                Report.Load(Server.MapPath("~/Reports/LPO-Invoice/LPOInvoice.rpt"));

                List<IT.Web.Models.CompnayModel> compnayModels = new List<Web.Models.CompnayModel>();
                List<IT.Web.Models.LPOInvoiceModel> lPOInvoiceModels = new List<Web.Models.LPOInvoiceModel>();
                List<IT.Web.Models.LPOInvoiceDetailsModel> lPOInvoiceDetails = new List<LPOInvoiceDetailsModel>();
                List<VenderModel> venderModels = new List<VenderModel>();
                var lPOInvoiceModel = new IT.Web.Models.LPOInvoiceModel();

                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                lPOInvoiceModel.Id = Id;
                lPOInvoiceModel.detailId = CompanyId;

                var LPOInvoice = webServices.Post(lPOInvoiceModel, "Bill/EditReport");
                
                if (LPOInvoice.Data != "[]")
                {
                    lPOInvoiceModel = (new JavaScriptSerializer()).Deserialize<IT.Web.Models.LPOInvoiceModel>(LPOInvoice.Data.ToString());
                }
                             
                lPOInvoiceModels.Insert(0, lPOInvoiceModel);
                compnayModels = lPOInvoiceModel.compnays;
                lPOInvoiceDetails = lPOInvoiceModel.lPOInvoiceDetailsList;
                venderModels = lPOInvoiceModel.venders;

                Report.Database.Tables[0].SetDataSource(compnayModels);
                Report.Database.Tables[1].SetDataSource(venderModels);
                Report.Database.Tables[2].SetDataSource(lPOInvoiceModels);
                Report.Database.Tables[3].SetDataSource(lPOInvoiceDetails);

                string companyName;
                if (lPOInvoiceModels.Count > 0)
                {
                    companyName = Id + "-" + lPOInvoiceModels[0].PONumber;
                }
                else
                {
                    companyName = "Data Not Found";
                }
                var root = Server.MapPath("/PDF/");
                pdfname = String.Format("{0}.pdf", companyName);
                var path = Path.Combine(root, pdfname);
                path = Path.GetFullPath(path);

                Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //stram.Close();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = companyName + ".PDF";
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                Report.SetParameterValue("ImageUrl", "http://itmolen-001-site8.htempurl.com/ClientDocument/" + lPOInvoiceModel.compnays[0].LogoUrl);

                Stream stram = Report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stram.Seek(0, SeekOrigin.Begin);

                return new FileStreamResult(stram, "application/pdf");
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
                            content.Add(new StringContent(uploadDocumentsViewModel.BillId.ToString()), "BillId");
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