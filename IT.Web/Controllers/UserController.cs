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
           
            try
            {
                changePasswordViewModel.Id = Convert.ToInt32(Session["userId"]);
                return View(changePasswordViewModel);
            }
            catch(Exception ex)
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
                        return RedirectToAction("Index", "Home");                        
                    }
                    else if(result.StatusCode == System.Net.HttpStatusCode.NotFound)
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

    }
}