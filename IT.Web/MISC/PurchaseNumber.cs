using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace IT.Web.MISC
{
    public class PurchaseNumber
    {
        WebServices webServices = new WebServices();

        //public string PurchaseNumbers()
        //{
        //    string AlreadyNumber = "";
        //    string SerailNO = "";
        ////    var LPONoResult = webServices.Post(new SingleStringValueResult(), "Purchase/PurchaseNumber");
        ////    if (LPONoResult.StatusCode == System.Net.HttpStatusCode.Accepted)
        ////    {
        ////        if (LPONoResult.Data != "[]")
        ////        {
        ////            string LPNo = (new JavaScriptSerializer()).Deserialize<string>(LPONoResult.Data);


        ////            SerailNO = LPNo.Substring(4, 8);

        ////            SerailNO = SerailNO.ToString().Substring(0, 6);


        ////            string TotdayNumber = POClass.PONumber().Substring(0, 6);
        ////            int Counts = 0;
        ////            if (SerailNO == TotdayNumber)
        ////            {
        ////                Counts = Convert.ToInt32(LPNo.Substring(10, 2)) + 1;

        ////                if (Counts.ToString().Length == 1)
        ////                {
        ////                    SerailNO = "PUR-" + TotdayNumber + "0" + Counts;
        ////                }
        ////                else
        ////                {
        ////                    SerailNO = "PUR-" + TotdayNumber + Counts.ToString();
        ////                }
        ////            }
        ////            else
        ////            {
        ////                AlreadyNumber = POClass.PONumber();

        ////                SerailNO = "PUR-" + AlreadyNumber;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            AlreadyNumber = POClass.PONumber();

        ////            SerailNO = "PUR-" + AlreadyNumber;
        ////        }

        ////    }
        ////    else
        ////    {
        ////        AlreadyNumber = POClass.PONumber();

        ////        SerailNO = "PUR-" + AlreadyNumber;
        ////    }

        ////    return SerailNO;
        ////}
    }
}