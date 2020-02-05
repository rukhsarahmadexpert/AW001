using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace IT.Web.MISC
{
    public class OrderNumber
    {
        //WebServices webServices = new WebServices();

        //public string OrderNewNumber()
        //{
        //    try
        //    {
        //        string AlreadyNumber = "";
        //        string SerailNO = "";
        //        var EXPNoResult = webServices.Post(new SingleStringValueResult(), "CustomerOrder/OrderNumber");
        //        if (EXPNoResult.StatusCode == System.Net.HttpStatusCode.Accepted)
        //        {
        //            if (EXPNoResult.Data != "[]" && EXPNoResult.Data != "null")
        //            {
        //                string ExP = (new JavaScriptSerializer()).Deserialize<string>(EXPNoResult.Data);


        //                SerailNO = ExP.Substring(4, 8);

        //                SerailNO = SerailNO.ToString().Substring(0, 6);


        //                string TotdayNumber = POClass.PONumber().Substring(0, 6);
        //                int Counts = 0;
        //                if (SerailNO == TotdayNumber)
        //                {
        //                    Counts = Convert.ToInt32(ExP.Substring(10, 2)) + 1;

        //                    if (Counts.ToString().Length == 1)
        //                    {
        //                        SerailNO = "ODR-" + TotdayNumber + "0" + Counts;
        //                    }
        //                    else
        //                    {
        //                        SerailNO = "ODR-" + TotdayNumber + Counts.ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    AlreadyNumber = POClass.PONumber();

        //                    SerailNO = "ODR-" + AlreadyNumber;
        //                }
        //            }
        //            else
        //            {
        //                AlreadyNumber = POClass.PONumber();

        //                SerailNO = "ODR-" + AlreadyNumber;
        //            }
        //        }
        //        else
        //        {
        //            AlreadyNumber = POClass.PONumber();

        //            SerailNO = "ODR-" + AlreadyNumber;
        //        }

        //        return SerailNO;
        //    }
        //    catch(Exception ex)
        //    {
        //        return ex.ToString();
        //    }

          
        //}

    }
}