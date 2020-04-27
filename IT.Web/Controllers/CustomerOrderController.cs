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
    [ExceptionLog]
    [Autintication]
    public class CustomerOrderController : Controller
    {
        WebServices webServices = new WebServices();
        List<CustomerNoteOrderViewModel> customerNoteOrderViewModel = new List<CustomerNoteOrderViewModel>();
        readonly CustomerOrderViewModel CustomerOrderViewModel = new CustomerOrderViewModel();
        CustomerOrderGroupViewModel customerOrderGroupViewModel = new CustomerOrderGroupViewModel();
        readonly CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();
        DriverVehicelViewModel driverVehicelViewModel = new DriverVehicelViewModel();
        VehicleController vehicleController = new VehicleController();

        int CompanyId = 0;

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(string OrderProgress = "all", string IsSend = "true", int CompId = 0)
        {
            //if (CompId == 0)
            //{
            //    CompanyId = Convert.ToInt32(Session["CompanyId"]);
            //    ViewBag.LayoutName = "~/Views/Shared/_Layout.cshtml";
            //}
            //else
            //{
            //    CompanyId = CompId;
            //    ViewBag.LayoutName = "~/Views/Shared/_layoutAdmin.cshtml";
            //}
            try
            {
                return View();
              //  CompanyId = Convert.ToInt32(Session["CompanyId"]);
                //PagingParameterModel pagingParameterModel = new PagingParameterModel
                //{ 
                //    pageNumber = 1,
                //    _pageSize = 1,
                //    CompanyId = CompanyId,
                //    PageSize = 100,
                //};
                //pagingParameterModel.OrderProgress = OrderProgress;
                //if (IsSend == "False")
                //{
                //    pagingParameterModel.IsSend = false;
                //}
                //else
                //{
                //    pagingParameterModel.IsSend = true;
                //}
                //var CustomerOrderList = webServices.Post(pagingParameterModel, "CustomerOrder/CustomerOrderAllByCompanyId", false);


                //if (CustomerOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                //{
                //    customerNoteOrderViewModel = (new JavaScriptSerializer().Deserialize<List<CustomerNoteOrderViewModel>>(CustomerOrderList.Data.ToString()));
                //}

                //return View(customerNoteOrderViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CustomerAll(string OrderProgress = "all", string IsSend = "true", int CompId = 0)
        {
            if (CompId == 0)
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);             
            }
            else
            {
                CompanyId = CompId;                
            }
            //List<CustomerNoteOrderViewModel> customerNoteOrderViewModels = new List<CustomerNoteOrderViewModel>();
            try
            {
                // CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" +
                Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                string search = Request.Form.GetValues("search[value]")[0];
                //int skip = start != null ? Convert.ToInt32(start) : 0;

                if (OrderProgress == null)
                {
                    OrderProgress = "All";
                }
                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                if (Convert.ToInt32(start) == 0)
                {
                    pagingParameterModel.pageNumber = 1;
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
                    pagingParameterModel.IsSend = true;
                    pagingParameterModel.OrderProgress = OrderProgress;
                    pagingParameterModel.CompanyId = CompanyId;
                }
                else
                {
                    pagingParameterModel.pageNumber = Convert.ToInt32(draw);
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.IsSend = true;
                    pagingParameterModel.OrderProgress = OrderProgress;
                    pagingParameterModel.CompanyId = CompanyId;
                }
                if (IsSend == "False")
                {
                    pagingParameterModel.IsSend = false;
                }
                else
                {
                    pagingParameterModel.IsSend = true;
                }
                var CustomerOrderList = webServices.Post(pagingParameterModel, "CustomerOrder/CustomerOrderAllByCompanyId", false);
                if (CustomerOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (CustomerOrderList.Data != "[]" && CustomerOrderList.Data != null)
                    {
                        customerNoteOrderViewModel = (new JavaScriptSerializer().Deserialize<List<CustomerNoteOrderViewModel>>(CustomerOrderList.Data.ToString()));
                        TotalRow = customerNoteOrderViewModel.Count;
                        return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = customerNoteOrderViewModel }, JsonRequestBehavior.AllowGet);
                        //compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                    }
                }
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = customerNoteOrderViewModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetAll(string OrderProgress)
        {
            try
            {
                //CompanyId = Convert.ToInt32(Session["CompanyId"]);
                PagingParameterModel pagingParameterModel = new PagingParameterModel
                { 
                    pageNumber = 1,
                    _pageSize = 1,
                    OrderProgress = OrderProgress,
                    //if (OrderProgress != "All")
                    //{
                    //    pagingParameterModel.CompanyId = CompanyId;
                    //}                
                    IsSend = true,
                    PageSize = 100,
                };

                var CustomerOrderList = webServices.Post(pagingParameterModel, "CustomerOrder/CustomerOrderAllByCompanyId", false);

                if (CustomerOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerNoteOrderViewModel = (new JavaScriptSerializer().Deserialize<List<CustomerNoteOrderViewModel>>(CustomerOrderList.Data.ToString()));
                }

                return Json(customerNoteOrderViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var CustomerOrderList = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id, false);

                if (CustomerOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (CustomerOrderList.Data != "[]" || CustomerOrderList.Data != "No Data Exist on This Id")
                    {
                        customerOrderGroupViewModel = (new JavaScriptSerializer().Deserialize<CustomerOrderGroupViewModel>(CustomerOrderList.Data.ToString()));
                    }
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(customerOrderGroupViewModel, JsonRequestBehavior.AllowGet);
                }

                return View(customerOrderGroupViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Admin()
        {  
            try
            {
                return View();
                //if (OrderProgress == null)
                //{
                //    OrderProgress = "All";
                //}
                //PagingParameterModel pagingParameterModel = new PagingParameterModel
                //{ 
                //    pageNumber = 1,
                //    _pageSize = 1,
                //    CompanyId = CompanyId,
                //    OrderProgress = OrderProgress,
                //    IsSend = true,
                //    PageSize = 100,
                //};
                //var CustomerOrderList = webServices.Post(pagingParameterModel, "CustomerOrder/GetAllCustomerOrderGroupByAdmin", false);

                //if (CustomerOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                //{
                //    if (CustomerOrderList.Data != null && CustomerOrderList.Data != "[]")
                //    {
                //        customerNoteOrderViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerNoteOrderViewModel>>(CustomerOrderList.Data.ToString()));
                //    }
                //}
                //return View(customerNoteOrderViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult All(string OrderProgress)
        {
            List<CustomerNoteOrderViewModel> customerNoteOrderViewModels = new List<CustomerNoteOrderViewModel>();
            try
            {
               // CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" +
                Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                string search = Request.Form.GetValues("search[value]")[0];
                //int skip = start != null ? Convert.ToInt32(start) : 0;

                if (OrderProgress == null)
                {
                    OrderProgress = "All";
                }
                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                if (Convert.ToInt32(start) == 0)
                {
                    pagingParameterModel.pageNumber = 1;
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
                    pagingParameterModel.IsSend = true;
                    pagingParameterModel.OrderProgress = OrderProgress;
                    pagingParameterModel.CompanyId = 0;
                }
                else
                {
                    pagingParameterModel.pageNumber = Convert.ToInt32(draw);
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.IsSend = true;
                    pagingParameterModel.OrderProgress = OrderProgress;
                    pagingParameterModel.CompanyId = 0;
                }

                var customerNoteList = webServices.Post(pagingParameterModel, "CustomerOrder/GetAllCustomerOrderGroupByAdmin");

                if (customerNoteList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (customerNoteList.Data != "[]" && customerNoteList.Data != null)
                    {
                        customerNoteOrderViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerNoteOrderViewModel>>(customerNoteList.Data.ToString()));

                        TotalRow = customerNoteOrderViewModels.Count;

                        return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = customerNoteOrderViewModels }, JsonRequestBehavior.AllowGet);
                        //compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                    }
                }
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = customerNoteOrderViewModels }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult AdminDetails(int Id)
        {
            try
            {
                var customerOrderGroup = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/GetAllCustomerOrderGroupByAdmin/" + Id, false);

                if (customerOrderGroup.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerOrderGroupViewModel = (new JavaScriptSerializer().Deserialize<CustomerOrderGroupViewModel>(customerOrderGroup.Data.ToString()));
                }


                var CustomerOrderGroupDetailsList = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderDetailsByOrderId/" + Id, false);

                if (CustomerOrderGroupDetailsList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerOrderGroupViewModel.customerGroupOrderDetailsViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerGroupOrderDetailsViewModel>>(CustomerOrderGroupDetailsList.Data.ToString()));
                }

                return View(customerOrderGroupViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult OrderDetails(int Id)
        {
            try
            {
                CustomerOrderGroupViewModel customerOrderGroupViewModel = new CustomerOrderGroupViewModel();

                var customerOrderGroup = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id, false);

                if (customerOrderGroup.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerOrderGroupViewModel = (new JavaScriptSerializer().Deserialize<CustomerOrderGroupViewModel>(customerOrderGroup.Data.ToString()));
                }
                //if(customerOrderGroupViewModel.OrderProgress == "Order Accepted")
                //{
                //    return View("AssignToDriver",customerOrderGroupViewModel);
                //}

                return View(customerOrderGroupViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        [HttpGet]
        public ActionResult SiteAssiedOrder()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SiteAssignedOrderAll()
        {
          var orderSiteAssignedList =  new List<CustomerOrderSiteAssignedViewModel>();

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
                    pagingParameterModel.SerachKey = search;
                }
                else
                {
                    pagingParameterModel.pageNumber = Convert.ToInt32(draw);
                    pagingParameterModel._pageSize = pageSize;
                    pagingParameterModel.PageSize = pageSize;
                    pagingParameterModel.SerachKey = search;
                }

                var assignedOrderList = webServices.Post(pagingParameterModel, "CustomerOrder/CustomerOrderGroupAsignedForSite");

                if (assignedOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    int TotalRow = 0;
                    if (assignedOrderList.Data != "[]" && assignedOrderList.Data != null)
                    {
                        orderSiteAssignedList = (new JavaScriptSerializer().Deserialize<List<CustomerOrderSiteAssignedViewModel>>(assignedOrderList.Data.ToString()));

                        TotalRow = orderSiteAssignedList.Count;

                        return Json(new { draw = draw, recordsFiltered = TotalRow, recordsTotal = TotalRow, data = orderSiteAssignedList }, JsonRequestBehavior.AllowGet);
                        //compnayModels = (new JavaScriptSerializer().Deserialize<List<CompnayModel>>(CompanyList.Data.ToString()));
                    }
                }
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = orderSiteAssignedList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        [HttpPost]
        public ActionResult AcceptOrder(CustomerOrderViewModel customerOrderViewModel)
        {
            try
            {

                var Result = webServices.Post(customerOrderViewModel, "CustomerOrder/CustomerOrderRejectAcceptByAdmin", false);

                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (Result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));
                    }
                }

                return Json("suceess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RejectOrder(CustomerOrderViewModel customerOrderViewModel)
        {
            try
            {
                customerOrderViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);

                var Result = webServices.Post(customerOrderViewModel, "CustomerOrder/CustomerOrderRejectAcceptByAdmin", false);

                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (Result.Data != "[]")
                    {
                        int Res = (new JavaScriptSerializer().Deserialize<int>(Result.Data));
                    }
                }

                return Json("suceess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                driverVehicelViewModel = vehicleController.DriverVehicels(CompanyId);

                ProductController productController = new ProductController();
                CustomerSitesController customerSites = new CustomerSitesController();

                driverVehicelViewModel.driverModels.Insert(0, new DriverModel() { DriverId = 0, DriverName = "Select Driver" });
                driverVehicelViewModel.vehicleModels.Insert(0, new VehicleModel() { VehicelId = 0, TraficPlateNumber = "Select Vehicle" });

                ViewBag.driverModels = driverVehicelViewModel.driverModels;
                ViewBag.vehicleModels = driverVehicelViewModel.vehicleModels;
                ViewBag.product = productController.Products();
                ViewBag.Sites = customerSites.SitesAll(CompanyId);

                return View(new CustomerOrderGroupViewModel());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult CustomerGroupOrderAdd(CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {
                //return View("Create", new CustomerOrderGroupViewModel());
                if (customerOrderListViewModel.Id > 0)
                {
                    customerOrderListViewModel.UpdatedBy = Convert.ToInt32(Session["UserId"]);

                    var result = webServices.Post(customerOrderListViewModel, "CustomerOrder/CustomerGroupOrderUpdate");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            customerOrderListViewModel = (new JavaScriptSerializer().Deserialize<CustomerOrderListViewModel>(result.Data.ToString()));
                            return Json(customerOrderListViewModel, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {

                    OrderNumber orderNumber = new OrderNumber();

                    customerOrderListViewModel.CustomerId = Convert.ToInt32(Session["CompanyId"]);
                    customerOrderListViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    customerOrderListViewModel.RequestThrough = "web";
                    customerOrderListViewModel.DeliveryNoteNumber = "0";
                    customerOrderListViewModel.LocationFullUrl = customerOrderListViewModel.LocationFullUrl ?? "UnKnown";

                    var result = webServices.Post(customerOrderListViewModel, "CustomerOrder/CustomerGroupOrderAdd");

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Data != "[]")
                        {
                            customerOrderListViewModel = (new JavaScriptSerializer().Deserialize<CustomerOrderListViewModel>(result.Data.ToString()));
                            return Json(customerOrderListViewModel, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult CustomerOrderSend(SearchViewModel searchViewModel)
        {
            try
            {
                var CustomerOrderList = webServices.Post(searchViewModel, "CustomerOrder/CustomerOrderSend", false);

                if (CustomerOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerOrderGroupViewModel = (new JavaScriptSerializer().Deserialize<CustomerOrderGroupViewModel>(CustomerOrderList.Data.ToString()));
                }
                return RedirectToAction("Details", new { searchViewModel.Id });

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
                int CompanyId = Convert.ToInt32(Session["CompanyId"]);

                CustomerOrderGroupViewModel customerOrderGroupViewModel = new CustomerOrderGroupViewModel();

                var customerOrderGroup = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/CustomerGroupOrderById/" + Id, false);

                if (customerOrderGroup.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerOrderGroupViewModel = (new JavaScriptSerializer().Deserialize<CustomerOrderGroupViewModel>(customerOrderGroup.Data.ToString()));
                }
                driverVehicelViewModel = vehicleController.DriverVehicels(CompanyId);

                ProductController productController = new ProductController();
                CustomerSitesController customerSites = new CustomerSitesController();

                driverVehicelViewModel.driverModels.Insert(0, new DriverModel() { DriverId = 0, DriverName = "Select Driver" });
                driverVehicelViewModel.vehicleModels.Insert(0, new VehicleModel() { VehicelId = 0, TraficPlateNumber = "Select Vehicle" });

                ViewBag.driverModels = driverVehicelViewModel.driverModels;
                ViewBag.vehicleModels = driverVehicelViewModel.vehicleModels;
                ViewBag.product = productController.Products();
                ViewBag.Sites = customerSites.SitesAll(CompanyId);

                return View("Create", customerOrderGroupViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult CustomerOrderDetailsDelete(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            try
            {
                //return Json("success", JsonRequestBehavior.AllowGet);
                customerOrderDeliverVewModel.Quantity = customerOrderDeliverVewModel.Quantity - customerOrderDeliverVewModel.RowQuantity;

                var customerOrderGroup = webServices.Post(customerOrderDeliverVewModel, "CustomerOrder/CustomerOrderDetailsDelete", false);

                if (customerOrderGroup.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (customerOrderGroup.Data != null || customerOrderGroup.Data != "[]")
                    {
                        int Result = (new JavaScriptSerializer().Deserialize<int>(customerOrderGroup.Data.ToString()));
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult CustomerOrderAssignToDriver(CustomerOrderListViewModel customerOrderListViewModel)
        {
            try
            {
                // return Json("Success", JsonRequestBehavior.AllowGet);

                customerOrderListViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                var customerOrderGroup = webServices.Post(customerOrderListViewModel, "CustomerOrder/CustomerOrderGroupAsignedDriverAdd", false);

                if (customerOrderGroup.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (customerOrderGroup.Data != null || customerOrderGroup.Data != "[]")
                    {
                        //int Result = (new JavaScriptSerializer().Deserialize<int>(customerOrderGroup.Data.ToString()));
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
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
        public ActionResult TestMap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadDocumentsAddAdmin(UploadDocumentsViewModel uploadDocumentsViewModel, HttpPostedFileBase FileUrl)
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
                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "CreatedBy");
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            content.Add(new StringContent(uploadDocumentsViewModel.OrderId.ToString()), "OrderId");
                            content.Add(new StringContent(uploadDocumentsViewModel.FilesName.ToString()), "FilesName");

                            var result = webServices.PostMultiPart(content, "UploadDocuments/UploadDocumentsAdd", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Admin));
                            }
                            else
                            {
                                return Redirect(nameof(Admin));
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
        public ActionResult DeliverdSiteAssignedOrder(CustomerOrderDeliverVewModel customerOrderDeliverVewModel)
        {
            try
            {
                //return Json("success", JsonRequestBehavior.AllowGet);
                
                var customerOrderGroupDeliverd = webServices.Post(customerOrderDeliverVewModel, "CustomerOrder/CustomerOrderDetailsGroupDeliveryByDriver", false);

                if (customerOrderGroupDeliverd.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (customerOrderGroupDeliverd.Data != null || customerOrderGroupDeliverd.Data != "[]")
                    {
                        int Result = (new JavaScriptSerializer().Deserialize<int>(customerOrderGroupDeliverd.Data.ToString()));
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult CustomerOrderDetailsGroupAsignedByOrderId(SearchViewModel searchViewModel)
        {
            try
            {
               var ResultData = new CustomerGroupOrderDetailsViewModel();
                //return Json("success", JsonRequestBehavior.AllowGet);

                var customerOrderGroupAssignDetails = webServices.Post(new CustomerGroupOrderDetailsViewModel(), "CustomerOrder/CustomerOrderDetailsGroupAsignedByOrderId/" + searchViewModel.Id, false);

                if (customerOrderGroupAssignDetails.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (customerOrderGroupAssignDetails.Data != null || customerOrderGroupAssignDetails.Data != "[]")
                    {
                        ResultData = (new JavaScriptSerializer().Deserialize<List<CustomerGroupOrderDetailsViewModel>>(customerOrderGroupAssignDetails.Data.ToString())).FirstOrDefault();
                    }
                    return Json(ResultData, JsonRequestBehavior.AllowGet);
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

    }
}