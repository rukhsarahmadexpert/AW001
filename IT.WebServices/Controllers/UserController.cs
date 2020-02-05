using IT.Core.Common;
using IT.Core.ViewModels;
using IT.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class UserController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        //UserViewModel userViewModel = new UserViewModel();
        string contentType = "application/json"; //ConfigurationManager.AppSettings["ContentType"].ToString();

        [HttpPost]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var userViewModel = new UserViewModel();

                /*
                SqlParameter[] sqlParameters;
                string parameter = SQLParameters.GetParameters<DoctorViewModel>(doctorViewModel);
                sqlParameters = SQLParameters.GetSQLParameters<DoctorViewModel>(doctorViewModel, "GetAll").ToArray();
                var doctorsList = unitOfWork.GetRepositoryInstance<DoctorViewModel>().ReadStoredProcedure("Doctor_Detail " + parameter, sqlParameters).ToList();
                */
                var userList = unitOfWork.GetRepositoryInstance<UserViewModel>().ReadStoredProcedure("SPGetUser"
                    //var userList = unitOfWork.GetRepositoryInstance<UserViewModel>().ReadStoredProcedure("SPGetUser @Id,@Name"
                    //,new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = 1 }
                    //,new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value  =1}
                    ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(userList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
        [HttpPost]
        public HttpResponseMessage Add([FromBody] UserViewModel userViewModel)
        {
            try
            {
                //var userViewModel = new UserViewModel();

                /*
                SqlParameter[] sqlParameters;
                string parameter = SQLParameters.GetParameters<DoctorViewModel>(doctorViewModel);
                sqlParameters = SQLParameters.GetSQLParameters<DoctorViewModel>(doctorViewModel, "GetAll").ToArray();
                var doctorsList = unitOfWork.GetRepositoryInstance<DoctorViewModel>().ReadStoredProcedure("Doctor_Detail " + parameter, sqlParameters).ToList();
                */
                var userList = unitOfWork.GetRepositoryInstance<object>().WriteStoredProcedure("AddUser @FirstName,@LastName,@UserName,@Password"                    
                    ,new SqlParameter("FirstName", System.Data.SqlDbType.VarChar) { Value = userViewModel.FirstName}
                    ,new SqlParameter("LastName", System.Data.SqlDbType.VarChar) { Value  = userViewModel.LastName }
                    , new SqlParameter("UserName", System.Data.SqlDbType.VarChar) { Value = userViewModel.UserName }
                    , new SqlParameter("Password", System.Data.SqlDbType.VarChar) { Value = "12345" }
                    );
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(userList));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception exception)
            {
                userRepsonse.Exception(exception.Message);
                return Request.CreateResponse(HttpStatusCode.Conflict, userRepsonse, contentType);
            }
        }
    }
}
