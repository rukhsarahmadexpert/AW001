using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    public class CustomerOrderController : Controller
    {
        WebServices webServices = new WebServices();
        List<CustomerNoteOrderViewModel> customerNoteOrderViewModel = new List<CustomerNoteOrderViewModel>();
        CustomerOrderViewModel CustomerOrderViewModel = new CustomerOrderViewModel();
        CustomerOrderGroupViewModel customerOrderGroupViewModel = new CustomerOrderGroupViewModel();
        CustomerOrderListViewModel customerOrderListViewModel = new CustomerOrderListViewModel();
        DriverVehicelViewModel driverVehicelViewModel = new DriverVehicelViewModel();
        VehicleController vehicleController = new VehicleController();

        int CompanyId = 0;

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(string OrderProgress = "all", string IsSend = "true")
        {
            try
            {

                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = CompanyId;
                pagingParameterModel.PageSize = 100;

                pagingParameterModel.OrderProgress = OrderProgress;
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
                    customerNoteOrderViewModel = (new JavaScriptSerializer().Deserialize<List<CustomerNoteOrderViewModel>>(CustomerOrderList.Data.ToString()));
                }

                return View(customerNoteOrderViewModel);
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
                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.OrderProgress = OrderProgress;
                //if (OrderProgress != "All")
                //{
                //    pagingParameterModel.CompanyId = CompanyId;
                //}                
                pagingParameterModel.IsSend = true;
                pagingParameterModel.PageSize = 100;


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
                return View(customerOrderGroupViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Admin(string OrderProgress)
        {
            List<CustomerNoteOrderViewModel> customerNoteOrderViewModels = new List<CustomerNoteOrderViewModel>();
            try
            {
                if (OrderProgress == null)
                {
                    OrderProgress = "All";
                }

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = CompanyId;
                pagingParameterModel.OrderProgress = OrderProgress;
                pagingParameterModel.IsSend = true;
                pagingParameterModel.PageSize = 100;


                var CustomerOrderList = webServices.Post(pagingParameterModel, "CustomerOrder/GetAllCustomerOrderGroupByAdmin", false);

                if (CustomerOrderList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    if (CustomerOrderList.Data != null && CustomerOrderList.Data != "[]")
                    {
                        customerNoteOrderViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerNoteOrderViewModel>>(CustomerOrderList.Data.ToString()));
                    }
                }

                return View(customerNoteOrderViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AdminDetails(int Id)
        {
            try
            {
                var customerOrderGroup = webServices.Post(new CustomerOrderGroupViewModel(), "CustomerOrder/GetAllCustomerOrderGroupByAdmin/ " + Id, false);

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
                    customerOrderListViewModel.LocationFullUrl = customerOrderListViewModel.LocationFullUrl == null ? "UnKnown" : customerOrderListViewModel.LocationFullUrl;

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
                return RedirectToAction("Details", new { Id = searchViewModel.Id });

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

                driverVehicelViewModel.driverModels.Insert(0, new DriverModel() { DriverId = 0, DriverName = "Select Driver" });
                driverVehicelViewModel.vehicleModels.Insert(0, new VehicleModel() { VehicelId = 0, TraficPlateNumber = "Select Vehicle" });

                ViewBag.driverModels = driverVehicelViewModel.driverModels;
                ViewBag.vehicleModels = driverVehicelViewModel.vehicleModels;
                ViewBag.product = productController.Products();

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

        public ActionResult TestMap()
        {
            return View();
        }
    }
}