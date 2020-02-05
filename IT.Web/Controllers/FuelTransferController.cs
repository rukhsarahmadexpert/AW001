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
    [Autintication]
    public class FuelTransferController : Controller
    {
        WebServices webServices = new WebServices();
        List<FuelTransferViewModel> fuelTransferViewModels = new List<FuelTransferViewModel>();
        List<OrderTransferRequestsViewModel> orderTransferRequestsViewModels = new List<OrderTransferRequestsViewModel>();
        List<TransferFromDriverViewModel> transferFromDriverViewModels = new List<TransferFromDriverViewModel>();
        List<SearchViewModel> searchViewModels = new List<SearchViewModel>();
        int CompanyId;
        public ActionResult Index()
        {
            try
            {
                CompanyId = Convert.ToInt32(Session["companyId"]);

                PagingParameterModel pagingParameterModel = new PagingParameterModel();

                pagingParameterModel.pageNumber = 1;
                pagingParameterModel._pageSize = 1;
                pagingParameterModel.CompanyId = CompanyId;
                pagingParameterModel.PageSize = 100;


                var FuelTransferList = webServices.Post(new FuelTransferViewModel(), "FuelTransfer/All");

                if (FuelTransferList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    fuelTransferViewModels = (new JavaScriptSerializer().Deserialize<List<FuelTransferViewModel>>(FuelTransferList.Data.ToString()));
                }

                return View(fuelTransferViewModels);

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public ActionResult OrderTransferRequestsAll()
        {
            orderTransferRequestsViewModels = new List<OrderTransferRequestsViewModel>();

            try
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"]);
                SearchViewModel searchViewModel = new SearchViewModel();
                searchViewModel.CompanyId = CompanyId;
                var OrderTransferRquestViewList = webServices.Post(searchViewModel, "FuelTransfer/OrderTransferRequestsAll");
                if (OrderTransferRquestViewList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    orderTransferRequestsViewModels = (new JavaScriptSerializer().Deserialize<List<OrderTransferRequestsViewModel>>(OrderTransferRquestViewList.Data.ToString()));
                }
                return View(orderTransferRequestsViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult DriverAllOnlineByDriverId(int Id)
        {

            try
            {

                CompanyId = Convert.ToInt32(Session["CompanyId"]);

                var DriverInfo = webServices.Post(new DriverViewModel(), "AWFDriver/DriverAllOnline/" + CompanyId);
                if (DriverInfo.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var OnlineDriverList = (new JavaScriptSerializer().Deserialize<List<DriverViewModel>>(DriverInfo.Data.ToString()));

                    var SingleDriver = OnlineDriverList.Where(x => x.Id == Id).FirstOrDefault();

                    return Json(SingleDriver, JsonRequestBehavior.AllowGet);


                }

                return Json("failed", JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult OrderTransferRequestsAllByDriverId(OrderTransferRequestsViewModel orderTransferRequestsViewModel)
        {
            orderTransferRequestsViewModels = new List<OrderTransferRequestsViewModel>();
            try
            {
                PagingParameterModel pagingParameterModel = new PagingParameterModel();
                pagingParameterModel.DriverId = orderTransferRequestsViewModel.DriverId;
                var OrderTransferRquestViewList = webServices.Post(pagingParameterModel, "FuelTransfer/OrderTransferRequestsAllByDriverId");
                if (OrderTransferRquestViewList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    orderTransferRequestsViewModels = (new JavaScriptSerializer().Deserialize<List<OrderTransferRequestsViewModel>>(OrderTransferRquestViewList.Data.ToString()));
                }
                return View("OrderTransferRequestsAll", orderTransferRequestsViewModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult OrderTransferRequestsAdminAcceptOrReject(OrderTransferRequestsViewModel orderTransferRequestsViewModel)
        {
            try
            {
                orderTransferRequestsViewModel.AcceptBy = Convert.ToInt32(Session["UserId"]);
                var OrderTransferRquestViewList = webServices.Post(orderTransferRequestsViewModel, "FuelTransfer/OrderTransferRequestsAdminAcceptOrReject");
                if (OrderTransferRquestViewList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    orderTransferRequestsViewModel = (new JavaScriptSerializer().Deserialize<OrderTransferRequestsViewModel>(OrderTransferRquestViewList.Data.ToString()));
                }
                if (orderTransferRequestsViewModel.IsAccepted == false)
                {
                    return Redirect(nameof(OrderTransferRequestsAll));
                }
                else
                {

                    return Json(orderTransferRequestsViewModel, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CustomerOrderGroupTransferFromDriverAdd(TransferFromDriverViewModel transferFromDriverViewModel)
        {


            try
            {
                transferFromDriverViewModel.CreatedBy = Convert.ToInt32(Session["UserId"]);
                transferFromDriverViewModel.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var TransferFromDriverList = webServices.Post(transferFromDriverViewModel, "FuelTransfer/CustomerOrderGroupTransferFromDriverAdd");
                if (TransferFromDriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    transferFromDriverViewModel = (new JavaScriptSerializer().Deserialize<TransferFromDriverViewModel>(TransferFromDriverList.Data.ToString()));
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }

                return Json("failed", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CustomerOrderGroupTransferFromDriverAll()
        {
            CompanyId = Convert.ToInt32(Session["CompanyId"]);

            PagingParameterModel pagingParameterModel = new PagingParameterModel();
            pagingParameterModel.pageNumber = 1;
            pagingParameterModel._pageSize = 1;
            pagingParameterModel.CompanyId = CompanyId;
            pagingParameterModel.PageSize = 100;

            var CustomerOrderGroupFromDriverList = webServices.Post(pagingParameterModel, "FuelTransfer/CustomerOrderGroupTransferFromDriverAll");
            if (CustomerOrderGroupFromDriverList.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                transferFromDriverViewModels = (new JavaScriptSerializer().Deserialize<List<TransferFromDriverViewModel>>(CustomerOrderGroupFromDriverList.Data.ToString()));
            }

            return View(transferFromDriverViewModels);


        }
    }
}