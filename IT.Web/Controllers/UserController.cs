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
        public ActionResult logout()
        {
            Session.Abandon();
            Session.Clear();

            LoginViewModel loginViewModel = new LoginViewModel
            { 
                DeviceId = System.Net.Dns.GetHostName().ToString()
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
                loginViewModel.DeviceId = System.Net.Dns.GetHostName().ToString();
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
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserViewModel userViewModel)
        {
            try
            {

                var result = webServices.Post(userViewModel, "User/Register", false);

                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    userCompanyViewModel = (new JavaScriptSerializer()).Deserialize<UserCompanyViewModel>(result.Data.ToString());

                    if (userCompanyViewModel.CompanyId > 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Create", "Company");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();
        }
    }
}