using IT.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IT.Repository.WebServices
{
    public class WebServices
    {
        HttpClient httpClient;
        string baseURL = "http://localhost:64299/api/"; //ConfigurationManager.AppSettings["BaseURL"].ToString();
        ServiceResponseModel serviceResponseModel;
        public WebServices()
        {
            if (httpClient == null)
            {
                  httpClient = new HttpClient();
            //    httpClient.DefaultRequestHeaders.Add("UserName", "shahid@gmail.com");
            //    httpClient.DefaultRequestHeaders.Add("Password", "shahid");
            }
            serviceResponseModel = new ServiceResponseModel();
        }
        public ServiceResponseModel Post(object input, string service)
        {
            string inputJson = (new JavaScriptSerializer()).Serialize(input);


            HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(baseURL + service, inputContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                serviceResponseModel = (new JavaScriptSerializer()).Deserialize<ServiceResponseModel>(result);
            }
            return serviceResponseModel;
        }
    }
}
