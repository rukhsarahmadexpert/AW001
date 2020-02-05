using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT.Web.Controllers
{
    public class UserController : Controller
    {
        UserViewModel userViewModel = new UserViewModel();
        List<UserViewModel> userViewModelList = new List<UserViewModel>();        
        WebServices webServices = new WebServices();
        public ActionResult Index()
        {
            var result = webServices.Post(new UserViewModel(), "User/GetAll");
            userViewModelList = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(result.Data.ToString());
            return View(userViewModelList);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(UserViewModel userViewModel)
        {
            var result = webServices.Post(userViewModel, "User/Add");
            //var resultData = (new JavaScriptSerializer()).Deserialize<object>(result.Data.ToString());
            //return View();
            if (Convert.ToInt32(result.Data) > 0)
                return RedirectToAction("Index");
            else
                return View(userViewModel);
        }
       
    }
}