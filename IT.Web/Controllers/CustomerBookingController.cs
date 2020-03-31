using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web.MISC;
using IT.Web_New.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Controllers
{
    [ExceptionLog]
    [Autintication]
    public class CustomerBookingController : Controller
    {
        WebServices webServices = new WebServices();
        List<CustomerBookingViewModel> customerBookingViewModels = new List<CustomerBookingViewModel>();
        CustomerBookingViewModel customerBookingViewModel = new CustomerBookingViewModel();
        int CompanyId;
        // GET: CustomerBooking

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    CompanyId = 0,
                    PageSize = 10
                };

                var CustomerBookingList = webServices.Post(pagingParameterModel, "CustomerBooking/All");

                if (CustomerBookingList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerBookingViewModel>>(CustomerBookingList.Data.ToString()));
                }

                return View(customerBookingViewModels);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerBookingReserved(CustomerBookingViewModel customerBookingViewModel)
        {

            try
            {
               

                CustomerBookingReservedRemaining customerBookingReservedRemaining = new CustomerBookingReservedRemaining();
                
                var CustomerBookingList = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingReserved");

             
                if (CustomerBookingList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingReservedRemaining = (new JavaScriptSerializer().Deserialize<CustomerBookingReservedRemaining>(CustomerBookingList.Data.ToString()));
                }

                return View(customerBookingReservedRemaining);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult BookingReserved()
        {

            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                CustomerBookingViewModel customerBookingViewModel = new CustomerBookingViewModel();
                customerBookingViewModel.CompanyId = CompanyId;

                CustomerBookingReservedRemaining customerBookingReservedRemaining = new CustomerBookingReservedRemaining();

                var CustomerBookingList = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingReserved");


                if (CustomerBookingList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingReservedRemaining = (new JavaScriptSerializer().Deserialize<CustomerBookingReservedRemaining>(CustomerBookingList.Data.ToString()));
                }

                return View(customerBookingReservedRemaining);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult BookingRemaining()
        {

            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                CustomerBookingViewModel customerBookingViewModel = new CustomerBookingViewModel();
                customerBookingViewModel.CompanyId = CompanyId;

                CustomerBookingReservedRemaining customerBookingReservedRemaining = new CustomerBookingReservedRemaining();

                var CustomerBookingList = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingReserved");


                if (CustomerBookingList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingReservedRemaining = (new JavaScriptSerializer().Deserialize<CustomerBookingReservedRemaining>(CustomerBookingList.Data.ToString()));
                }

                return View(customerBookingReservedRemaining);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerBookingAdd(CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                var CustomerResult = new ServiceResponseModel();
                customerBookingViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                CustomerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingAdd");
              
                if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var reuslt = (new JavaScriptSerializer().Deserialize<int>(CustomerResult.Data));

                    return RedirectToAction(nameof(Index));
                }

                return View(customerBookingViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerBookingUpdate(CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                var CustomerResult = new ServiceResponseModel();
                if (customerBookingViewModel.Id > 0)
                {
                    customerBookingViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    CustomerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingUpdate");

                    if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var reuslt = (new JavaScriptSerializer().Deserialize<int>(CustomerResult.Data));

                        return RedirectToAction(nameof(Index));
                    }

                    return View(customerBookingViewModel);
                }
                else
                {
                    return View(customerBookingViewModel);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult AdminCreate()
        {
            CompanyController companyController = new CompanyController();
            ProductController productController = new ProductController();
            ProductUnitController productUnitController = new ProductUnitController();

            ViewBag.Companies = companyController.Companies();
            ViewBag.Products = productController.Products();
            ViewBag.ProductUnits = productUnitController.ProductUnits();

            CustomerBookingViewModel CustomerBookingViewModel = new CustomerBookingViewModel();
            CustomerBookingViewModel.VAT = (decimal)0.00;
            CustomerBookingViewModel.TotalAmount = (decimal)0.00;

            return View(CustomerBookingViewModel);
        }

        [HttpGet]
        public ActionResult CustomerCreate()
        {
           
            ProductController productController = new ProductController();
            ProductUnitController productUnitController = new ProductUnitController();

            ViewBag.Products = productController.Products();
            ViewBag.ProductUnits = productUnitController.ProductUnits();

            CustomerBookingViewModel CustomerBookingViewModel = new CustomerBookingViewModel();
            CustomerBookingViewModel.VAT = (decimal)0.00;
            CustomerBookingViewModel.TotalAmount = (decimal)0.00;
            return View(CustomerBookingViewModel);
        }

        [HttpPost]
        public ActionResult CustomerCreate(CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ProductController productController = new ProductController();
                    ProductUnitController productUnitController = new ProductUnitController();

                    ViewBag.Products = productController.Products();
                    ViewBag.ProductUnits = productUnitController.ProductUnits();

                    if (customerBookingViewModel.IsAwfuelAdmin == "IsAwfuelAdmin")
                    {
                        if (customerBookingViewModel.CompanyId == 0)
                        {
                            ModelState.AddModelError("CompanyId", "Please select customer");
                        }
                        CompanyController companyController = new CompanyController();
                        ViewBag.Companies = companyController.Companies();
                        return View("AdminCreate", customerBookingViewModel);
                    }
                    else
                    {
                        return View("CustomerCreate", customerBookingViewModel);
                    }
                }                
                else
                {
                    if (customerBookingViewModel.IsAwfuelAdmin == "IsAwfuelAdmin")
                    {
                        if (customerBookingViewModel.CompanyId == 0)
                        {
                            CompanyController companyController = new CompanyController();
                            ProductController productController = new ProductController();
                            ProductUnitController productUnitController = new ProductUnitController();

                            ViewBag.Companies = companyController.Companies();
                            ViewBag.Products = productController.Products();
                            ViewBag.ProductUnits = productUnitController.ProductUnits();

                            ModelState.AddModelError("CompanyId", "Please select customer");

                            return View("AdminCreate", customerBookingViewModel);
                        }
                    }
                    var CustomerResult = new ServiceResponseModel();
                    customerBookingViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    if (customerBookingViewModel.IsAwfuelAdmin != "IsAwfuelAdmin")
                    {
                        customerBookingViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                    }
                    CustomerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingAdd");

                    if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var reuslt = (new JavaScriptSerializer().Deserialize<int>(CustomerResult.Data));

                        if (customerBookingViewModel.IsAwfuelAdmin == "IsAwfuelAdmin")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return RedirectToAction(nameof(Customer));
                        }
                    }

                    return View(customerBookingViewModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult CustomerBookingGetById(int Id)
        {
            try
            {
                customerBookingViewModel.Id = Id;
                var customerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingGetById");

                if (customerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModel = (new JavaScriptSerializer().Deserialize<CustomerBookingViewModel>(customerResult.Data.ToString()));
                }

                CompanyController companyController = new CompanyController();
                ProductController productController = new ProductController();
                ProductUnitController productUnitController = new ProductUnitController();

                ViewBag.Companies = companyController.Companies();
                ViewBag.Products = productController.Products();
                ViewBag.ProductUnits = productUnitController.ProductUnits();

                return View(customerBookingViewModel);
            
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
                customerBookingViewModel.Id = Id;
                var customerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingGetById");

                if (customerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModel = (new JavaScriptSerializer().Deserialize<CustomerBookingViewModel>(customerResult.Data.ToString()));
                }

                var updateDatetList = webServices.Post(customerBookingViewModel, "CustomerBooking/BookingUpdateReasonAllByBookingId");

                if (updateDatetList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerBookingViewModel>>(updateDatetList.Data.ToString()));
                }

                // CompanyController companyController = new CompanyController();
                // ProductController productController = new ProductController();
                // ProductUnitController productUnitController = new ProductUnitController();

                // ViewBag.Companies = companyController.Companies();
                // ViewBag.Products = productController.Products();
                // ViewBag.ProductUnits = productUnitController.ProductUnits();
                ViewBag.customerBookingViewModels = customerBookingViewModels;
                return View(customerBookingViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult CustomerDetails(int Id)
        {
            try
            {
                customerBookingViewModel.Id = Id;
                var customerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingGetById");

                if (customerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModel = (new JavaScriptSerializer().Deserialize<CustomerBookingViewModel>(customerResult.Data.ToString()));
                }

                var updateDatetList = webServices.Post(customerBookingViewModel, "CustomerBooking/BookingUpdateReasonAllByBookingId");

                if (updateDatetList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerBookingViewModel>>(updateDatetList.Data.ToString()));
                }

                // CompanyController companyController = new CompanyController();
                // ProductController productController = new ProductController();
                // ProductUnitController productUnitController = new ProductUnitController();

                // ViewBag.Companies = companyController.Companies();
                // ViewBag.Products = productController.Products();
                // ViewBag.ProductUnits = productUnitController.ProductUnits();
                ViewBag.customerBookingViewModels = customerBookingViewModels;
                return View(customerBookingViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerBookingSetDueDate(CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                var CustomerResult = new ServiceResponseModel();
                if (customerBookingViewModel.Id > 0)
                {
                    CustomerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingSetDueDate");

                    if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var reuslt = (new JavaScriptSerializer().Deserialize<int>(CustomerResult.Data));

                        return Json("suceess", JsonRequestBehavior.AllowGet);
                    }

                    return Json("suceess", JsonRequestBehavior.AllowGet);
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
        public ActionResult CustomerBookingAcceptReject(CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                var CustomerResult = new ServiceResponseModel();
                if (customerBookingViewModel.Id > 0)
                {
                    CustomerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingAcceptReject");

                    if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var reuslt = (new JavaScriptSerializer().Deserialize<int>(CustomerResult.Data));

                        return Json("suceess", JsonRequestBehavior.AllowGet);
                    }

                    return Json("suceess", JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(int Id)
        {
            try
            {
                customerBookingViewModel.Id = Id;
                var customerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingGetById");

                if (customerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModel = (new JavaScriptSerializer().Deserialize<CustomerBookingViewModel>(customerResult.Data.ToString()));
                }
               
                ProductController productController = new ProductController();
                ProductUnitController productUnitController = new ProductUnitController();

                ViewBag.Products = productController.Products();
                ViewBag.ProductUnits = productUnitController.ProductUnits();

                return View(customerBookingViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ProductController productController = new ProductController();
                    ProductUnitController productUnitController = new ProductUnitController();

                    ViewBag.Products = productController.Products();
                    ViewBag.ProductUnits = productUnitController.ProductUnits();

                    return View("Edit", customerBookingViewModel);

                }
                else
                {
                    var CustomerResult = new ServiceResponseModel();
                    if (customerBookingViewModel.Id > 0)
                    {
                        customerBookingViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                        customerBookingViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                        CustomerResult = webServices.Post(customerBookingViewModel, "CustomerBooking/CustomerBookingUpdate");

                        if (CustomerResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            var reuslt = (new JavaScriptSerializer().Deserialize<int>(CustomerResult.Data));

                            return RedirectToAction(nameof(Customer));
                        }

                        return View(customerBookingViewModel);
                    }
                    else
                    {
                        return View(customerBookingViewModel);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Customer()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel
                {
                    pageNumber = 1,
                    CompanyId = CompanyId,
                    PageSize = 10
                };

                var CustomerBookingList = webServices.Post(pagingParameterModel, "CustomerBooking/All");

                if (CustomerBookingList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerBookingViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerBookingViewModel>>(CustomerBookingList.Data.ToString()));
                }
                return View(customerBookingViewModels);

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
                            content.Add(new StringContent(uploadDocumentsViewModel.BookingId.ToString()), "BookingId");
                            var result = webServices.PostMultiPart(content, "UploadDocuments/UploadDocumentsAdd", true);
                            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                            {
                                return Redirect(nameof(Customer));
                            }
                            else
                            {
                                return Redirect(nameof(Customer));
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
                            content.Add(new StringContent("ClientDocs"), "ClientDocs");
                            string UserId = Session["UserId"].ToString();
                            content.Add(new StringContent(UserId), "CreatedBy");
                            content.Add(new StringContent(uploadDocumentsViewModel.BookingId.ToString()), "BookingId");
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