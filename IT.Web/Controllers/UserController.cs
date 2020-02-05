using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web_New.Controllers
{
    public class UserController : Controller
    {
        WebServices webServices = new WebServices();

        readonly UserViewModel userViewModel = new UserViewModel();
        readonly List<UserViewModel> userViewModelList = new List<UserViewModel>();
        UserCompanyViewModel userCompanyViewModel = new UserCompanyViewModel();
       

        public ActionResult Index()
        {
            //var result = webServices.Post(new UserViewModel(), "User/GetAll");
            //userViewModelList = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(result.Data.ToString());
            return View();
        }

        public ActionResult logout()
        {
            Session.Abandon();
            Session.Clear();
            return Redirect(nameof(Login));
        }

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