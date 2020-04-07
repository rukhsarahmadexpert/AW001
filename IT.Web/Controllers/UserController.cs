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
    public class UserController : Controller
    {
        WebServices webServices = new WebServices();

        readonly UserViewModel userViewModel = new UserViewModel();
        readonly List<UserViewModel> userViewModelList = new List<UserViewModel>();
        UserCompanyViewModel userCompanyViewModel = new UserCompanyViewModel();

        [HttpGet]
        public ActionResult Index()
        {
            //var result = webServices.Post(new UserViewModel(), "User/GetAll");
            //userViewModelList = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(result.Data.ToString());
            return View();
        }

        [HttpGet]
        public ActionResult UserInformation()
        {
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                var usercCompany = Session["userCompanyViewModel"] as UserCompanyViewModel;
                userViewModel.UserName = usercCompany.UserName;
                var userList = webServices.Post(userViewModel, "User/UserInformationByUserName");

                if (userList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    userViewModel = (new JavaScriptSerializer().Deserialize<UserViewModel>(userList.Data.ToString()));

                    return View(userViewModel);
                }

                return View(userViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult UserInformationEdit()
        {
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                var usercCompany = Session["userCompanyViewModel"] as UserCompanyViewModel;
                userViewModel.UserName = usercCompany.UserName;
                var userList = webServices.Post(userViewModel, "User/UserInformationByUserName");

                if (userList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    userViewModel = (new JavaScriptSerializer().Deserialize<UserViewModel>(userList.Data.ToString()));

                    return View(userViewModel);
                }

                return View(userViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult UserInformationUpdate(UserViewModel userViewModel, HttpPostedFileBase ImageUrl1)
        {
            try
            {

                //if (Request.Files.Count > 0 && LogoUrl != null)
                //{
                var file = ImageUrl1;

                using (HttpClient client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        if (ImageUrl1 != null)
                        {
                            byte[] fileBytes = new byte[file.InputStream.Length + 1];
                            file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("ImageUrl") { FileName = file.FileName };
                            content.Add(fileContent);
                        }
                        else
                        {
                            content.Add(new StringContent(userViewModel.ImageUrl ?? ""), "ImageUrl");
                        }
                        content.Add(new StringContent("ClientDocs"), "ClientDocs");
                        content.Add(new StringContent(userViewModel.UserId.ToString()), "UserId");
                        content.Add(new StringContent(userViewModel.FullName ?? ""), "FullName");
                        content.Add(new StringContent(userViewModel.UserName ?? ""), "UserName");
                        content.Add(new StringContent(userViewModel.Gender ?? ""), "Gender");
                        content.Add(new StringContent(userViewModel.DOB.ToString() ?? System.DateTime.Now.ToString()), "DOB");



                        var result = webServices.PostMultiPart(content, "User/UserInformationUpdate", true);
                        if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {

                            userViewModel = (new JavaScriptSerializer().Deserialize<UserViewModel>(result.Data.ToString()));

                            //return RedirectToAction("/");
                            return RedirectToAction(nameof(UserInformation));
                        }
                        else
                        {
                            ViewBag.Message = "Failed";
                        }
                    }
                }

                return RedirectToAction(nameof(UserInformation));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();

            LoginViewModel loginViewModel = new LoginViewModel
            {
                // DeviceId = System.Net.Dns.GetHostName().ToString()
                //  DeviceId = System.Environment.GetEnvironmentVariable("COMPUTERNAME").ToString()
                // DeviceId = System.Web.HttpContext.Current.Server.MachineName;
                //DeviceId = Request.UserHostAddress
                DeviceId = Request.Browser.Id
            };
            var result = webServices.Post(loginViewModel, "User/LogOut", false);
            return Redirect(nameof(Login));
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(UserViewModel userViewModel)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (TempData["SuccessPWD"] != null)
            {
                ViewBag.success = TempData["SuccessPWD"];
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                loginViewModel.Token = loginViewModel.Token ?? "token not availibe";
                loginViewModel.Device = "web";
                //loginViewModel.DeviceId = System.Environment.GetEnvironmentVariable("COMPUTERNAME").ToString();
                //loginViewModel.DeviceId = System.Environment.MachineName.ToString();
                // loginViewModel.DeviceId = System.Web.HttpContext.Current.Server.MachineName;
                //loginViewModel.DeviceId = Request.UserHostAddress;
                loginViewModel.DeviceId = Request.Browser.Id;
                if (ModelState.IsValid)
                {
                    var result = webServices.Post(loginViewModel, "User/Login", false);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        userCompanyViewModel = (new JavaScriptSerializer()).Deserialize<UserCompanyViewModel>(result.Data.ToString());

                        if (userCompanyViewModel != null)
                        {
                            Session["userCompanyViewModel"] = userCompanyViewModel;
                            Session["CompanyId"] = userCompanyViewModel.CompanyId;
                            Session["UserId"] = userCompanyViewModel.UserId;
                        }

                        if (userCompanyViewModel.Authority == "CustomerAdmin")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (userCompanyViewModel.Authority == "Admin")
                        {
                            return RedirectToAction("AdminHome", "Home");
                        }
                    }

                    ModelState.AddModelError("UserName", "Username or Password Incorrect");
                    return View(loginViewModel);
                }
                else
                {
                    return View(loginViewModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Registration(UserViewModel userViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = webServices.Post(userViewModel, "User/Register", false);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        userCompanyViewModel = (new JavaScriptSerializer()).Deserialize<UserCompanyViewModel>(result.Data.ToString());

                        if (userCompanyViewModel.CompanyId > 0)
                        {
                            Session["userCompanyViewModel"] = userCompanyViewModel;

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Create", "Company");
                        }
                    }
                    else if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ModelState.AddModelError("UserName", "This email exist, choose another");
                        return View(userViewModel);
                    }
                }
                else
                {
                    return View(userViewModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }

        [Autintication]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                changePasswordViewModel.Id = Convert.ToInt32(Session["userId"]);

                var usercCompany = Session["userCompanyViewModel"] as UserCompanyViewModel;
                userViewModel.UserName = usercCompany.UserName;
                var userList = webServices.Post(userViewModel, "User/UserInformationByUserName");

                if (userList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    userViewModel = (new JavaScriptSerializer().Deserialize<UserViewModel>(userList.Data.ToString()));

                }

                ViewBag.userViewModel = userViewModel;

                return View(changePasswordViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Autintication]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(changePasswordViewModel);
                }
                else
                {
                    var result = webServices.Post(changePasswordViewModel, "User/ChangePassword", false);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if (result.Message == "Data Not Found")
                        {
                            ModelState.AddModelError("Error", "Email not found");
                            return View(changePasswordViewModel);
                        }

                        var Result = (new JavaScriptSerializer()).Deserialize<string>(result.Data.ToString());
                        return RedirectToAction(nameof(UserInformation));
                    }
                    else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ModelState.AddModelError("Error", "Email not found");
                        return View(changePasswordViewModel);
                    }

                    return View(changePasswordViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            try
            {
                return View(new LoginViewModel());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ForgotPassword(LoginViewModel loginViewModel)
        {
            try
            {
                ModelState.Remove("Password");
                if (!ModelState.IsValid)
                {
                    return View(loginViewModel);
                }
                else
                {
                    var result = webServices.Post(loginViewModel, "User/ForgotPassword", false);

                    if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var data = (new JavaScriptSerializer().Deserialize<SearchModelForRecover>(result.Data.ToString()));

                        loginViewModel.Device = data.searchkey;

                        Session["loginViewModel"] = loginViewModel;

                        TempData["Success"] = "we have send password reset code to your email, please check inbox or spam";

                        return View("PasswordRestCode", new SearchViewModel());
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult PasswordRestCode()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public ActionResult PasswordRestCode(SearchViewModel searchViewModel)
        {
            try
            {
                if (Session["loginViewModel"] != null)
                {
                    if (searchViewModel.SearchKey2 != null || searchViewModel.SearchKey2 != "")
                    {
                        searchViewModel.SearchKey2 = searchViewModel.SearchKey2.Replace(" ", "");
                    }

                    var loginViewModel = new LoginViewModel();
                    loginViewModel = Session["loginViewModel"] as LoginViewModel;

                    if (loginViewModel.Device == searchViewModel.SearchKey2)
                    {
                        UserViewModel userViewModel = new UserViewModel();
                        userViewModel.UserName = loginViewModel.UserName;
                       return RedirectToAction(nameof(UpdateForGotPassword));                       
                    }
                    else
                    {
                        TempData["Success"] = "Incorrect Code, Please enter the accurate code";
                        return View(searchViewModel);
                    }
                }
                else
                {
                    TempData["Success"] = "Code has been expired";
                    return View(searchViewModel);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        [HttpGet]
        public ActionResult UpdateForGotPassword()
        {
            try
            {
                var loginViewModel = new LoginViewModel();
                loginViewModel = Session["loginViewModel"] as LoginViewModel;

                UserViewModel userViewModel = new UserViewModel();
                userViewModel.UserName = loginViewModel.UserName;
                
                return View(userViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult UpdateForGotPassword(UserViewModel userViewModel)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    ViewBag.success = "Password and Confirm Password not matched";
                    return View(userViewModel);
                }
                else
                {
                    ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
                    changePasswordViewModel.Email = userViewModel.UserName;
                    changePasswordViewModel.NewPassword = userViewModel.Password;

                    var Result = webServices.Post(changePasswordViewModel, "User/UpdatePassword");
                    if(Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        if(Result.Data == "\"Password chnage successfully\"")
                        {
                            TempData["SuccessPWD"] = "Password change successfully";
                            return RedirectToAction(nameof(Login));
                        }
                    }
                }

                ViewBag.success = "Filed to update password, Please try again later";
                return View(userViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

    public class SearchModelForRecover
    {       
        public int Id { get; set; }
        public string searchkey { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int CompanyId { get; set; }
        public bool Status { get; set; }
        public string Flage { get; set; }
        public string DeviceTiken { get; set; }
        public string DeviceId { get; set; }
        public string CompanyName { get; set; }
        public string NotificationCode { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    
    }
}