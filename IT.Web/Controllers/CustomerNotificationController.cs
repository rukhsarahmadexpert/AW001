using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    public class CustomerNotificationController : Controller
    {
        WebServices webServices = new WebServices();
        List<CustomerNotificationViewModel> customerNotificationViewModels = new List<CustomerNotificationViewModel>();
        CustomerNotificationViewModel customerNotificationViewModel = new CustomerNotificationViewModel();

        public ActionResult Index()
        {
            try
            {
                CustomerNotificationViewModel customerNotificationViewModel = new CustomerNotificationViewModel();
                var AddList = webServices.Post(customerNotificationViewModel, "Advertisement/All");

                if (AddList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerNotificationViewModels = (new JavaScriptSerializer().Deserialize<List<CustomerNotificationViewModel>>(AddList.Data.ToString()));
                    return View(customerNotificationViewModels);
                }

                return View(customerNotificationViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Create()
        {
            return View(new CustomerNotificationViewModel());
        }

        [HttpPost]
        public ActionResult Create(CustomerNotificationViewModel customerNotificationViewModel, HttpPostedFileBase ImageUrl)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = ImageUrl;

                    using (HttpClient client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("ImageUrl") { FileName = file.FileName };
                            string UserId = Session["UserId"].ToString();
                            if (customerNotificationViewModel.Id < 1)
                            {
                                content.Add(fileContent);
                                content.Add(new StringContent("ClientDocs"), "ClientDocs");
                                content.Add(new StringContent(UserId), "CreatedBy");
                                content.Add(new StringContent(customerNotificationViewModel.MessageTitle == null ? "" : customerNotificationViewModel.MessageTitle), "MessageTitle");
                                content.Add(new StringContent(customerNotificationViewModel.MessageDescription == null ? "" : customerNotificationViewModel.MessageDescription), "MessageDescription");

                                var result = webServices.PostMultiPart(content, "Advertisement/Add", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                {
                                    HttpContext.Cache.Remove("customerNotificationViewModels");
                                    return Redirect(nameof(Index));
                                }
                            }
                            else
                            {
                                content.Add(fileContent);
                                content.Add(new StringContent("ClientDocs"), "ClientDocs");
                                content.Add(new StringContent(UserId), "UpdatBy");
                                content.Add(new StringContent(customerNotificationViewModel.Id.ToString()), "Id");
                                content.Add(new StringContent(customerNotificationViewModel.MessageTitle == null ? "" : customerNotificationViewModel.MessageTitle), "MessageTitle");
                                content.Add(new StringContent(customerNotificationViewModel.MessageDescription == null ? "" : customerNotificationViewModel.MessageDescription), "MessageDescription");

                                var result = webServices.PostMultiPart(content, "Advertisement/Update", true);
                                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                                {
                                    HttpContext.Cache.Remove("customerNotificationViewModels");
                                    return Redirect(nameof(Index));
                                }
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

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {
                SearchViewModel searchViewModel = new SearchViewModel();
                searchViewModel.Id = Id;
                var addResult = webServices.Post(searchViewModel, "Advertisement/Edit/");

                if (addResult.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    customerNotificationViewModel = (new JavaScriptSerializer().Deserialize<CustomerNotificationViewModel>(addResult.Data.ToString()));
                }

                return View("Create", customerNotificationViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}