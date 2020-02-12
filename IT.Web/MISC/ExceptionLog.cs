using IT.Core.ViewModels;
using IT.Repository.WebServices;
using IT.Web_New.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT.Web.MISC
{
    public class ExceptionLog : FilterAttribute, IExceptionFilter
    {
        WebServices webServices = new WebServices();

        ExceptionLogViewModel exceptionLogViewModel = new ExceptionLogViewModel();
        HomeController Controller = new HomeController();

        public void OnException(ExceptionContext filterContext)
        {
            var line = Environment.NewLine + Environment.NewLine;
            try
            {

                Exception ex = filterContext.Exception;
                filterContext.ExceptionHandled = true;
                var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");
                exceptionLogViewModel.ControllerName = filterContext.Controller.ToString();
                exceptionLogViewModel.ActionName = (string)filterContext.RouteData.Values["action"];
                exceptionLogViewModel.ExceptionDatetime = System.DateTime.Now;
                if (filterContext.Exception.ToString().Length > 140)
                {
                    exceptionLogViewModel.ExceptionDescription = filterContext.Exception.StackTrace.ToString().Substring(0, 140);
                }
                else
                {
                    exceptionLogViewModel.ExceptionDescription = filterContext.Exception.ToString();
                }
                exceptionLogViewModel.ExceptionType = filterContext.Exception.GetType().ToString();
                exceptionLogViewModel.CompanyId = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);
                exceptionLogViewModel.UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                var Result = webServices.Post(exceptionLogViewModel, "ExceptionLog/AddException");
                if (Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary(model)
                    };
                }
                else
                {
                    string filepath = HttpContext.Current.Server.MapPath("~/DirectoryFolder/");
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";

                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Dispose();
                    }
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        string error = "Log Written Date:" + " " + DateTime.Now.ToString() + exceptionLogViewModel.ExceptionDatetime + "Error Message:" + " " + exceptionLogViewModel.ExceptionDescription + "Exception Type:" + " " + exceptionLogViewModel.ExceptionType + "Error Controller :" + " " + exceptionLogViewModel.ControllerName + " Error Action Name:" + " " + exceptionLogViewModel.ActionName + "UserId:" + " " + exceptionLogViewModel.UserId + "CompanyId" + exceptionLogViewModel.CompanyId;

                        sw.WriteLine("-------------------------------------------------------------------------------------");
                        sw.WriteLine(line);
                        sw.WriteLine(error);
                        sw.WriteLine("--------------------------------*End*------------------------------------------");
                        sw.WriteLine(line);
                        sw.Flush();
                        sw.Close();

                    }
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary(model)
                    };
                }
            }
            catch (Exception ex)
            {
                var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");
                string filepath = HttpContext.Current.Server.MapPath("~/DirectoryFolder");
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";

                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + exceptionLogViewModel.ExceptionDatetime + "Error Message:" + " " + exceptionLogViewModel.ExceptionDescription + "Exception Type:" + " " + exceptionLogViewModel.ExceptionType + "Error Controller :" + " " + exceptionLogViewModel.ControllerName + " Error Action Name:" + " " + exceptionLogViewModel.ActionName + "UserId:" + " " + exceptionLogViewModel.UserId + "CompanyId" + exceptionLogViewModel.CompanyId;

                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }

                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary(model)
                };
            }
        }
    }
}