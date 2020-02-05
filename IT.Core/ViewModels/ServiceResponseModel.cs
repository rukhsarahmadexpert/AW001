using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ServiceResponseModel
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public void Success(string entity)
        {
            this.StatusCode = System.Net.HttpStatusCode.Accepted;
            this.IsSuccess = true;
            this.Message = "Success";
            this.Data = entity;
        }
        public void Failed(string entity, string message)
        {
            this.StatusCode = System.Net.HttpStatusCode.ExpectationFailed;
            this.IsSuccess = true;
            this.Message = message;
            this.Data = entity;
        }
        public void BadRequest()
        {
            this.StatusCode = System.Net.HttpStatusCode.BadRequest;
            this.IsSuccess = true;
            this.Message = "Bad Request";
            this.Data = null;
        }
        public void Exception(string errorMessage)
        {
            this.StatusCode = System.Net.HttpStatusCode.Conflict;
            this.IsSuccess = true;
            this.Message = errorMessage;
            this.Data = null;
        }
        public void UnAuthorized()
        {
            this.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            this.IsSuccess = true;
            this.Message = "UnAuthorized user";
            this.Data = null;
        }
    }
}
