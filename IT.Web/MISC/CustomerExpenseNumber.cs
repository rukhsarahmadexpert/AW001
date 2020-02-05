using IT.Core.ViewModels;
using IT.Repository.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace IT.Web.MISC
{
    public class CustomerExpenseNumber
    {
        //WebServices webServices = new WebServices();

        //public string ExNumber()
        //{
        //    string AlreadyNumber = "";
        //    string SerailNO = "";
        //    var EXPNoResult = webServices.Post(new SingleStringValueResult(), "Expense/ExpenseNumber");
        //    if (EXPNoResult.StatusCode == System.Net.HttpStatusCode.Accepted)
        //    {
        //        if (EXPNoResult.Data != "[]")
        //        {
        //            string ExP = (new JavaScriptSerializer()).Deserialize<string>(EXPNoResult.Data);


        //            SerailNO = ExP.Substring(4, 8);

        //            SerailNO = SerailNO.ToString().Substring(0, 6);

        //            string TotdayNumber = POClass.PONumber().Substring(0, 6);
        //            int Counts = 0;
        //            if (SerailNO == TotdayNumber)
        //            {
        //                Counts = Convert.ToInt32(ExP.Substring(10, 2)) + 1;

        //                if (Counts.ToString().Length == 1)
        //                {
        //                    SerailNO = "EXP-" + TotdayNumber + "0" + Counts;
        //                }
        //                else
        //                {
        //                    SerailNO = "EXP-" + TotdayNumber + Counts.ToString();
        //                }
        //            }
        //            else
        //            {
        //                AlreadyNumber = POClass.PONumber();

        //                SerailNO = "EXP-" + AlreadyNumber;
        //            }
        //        }
        //        else
        //        {
        //            AlreadyNumber = POClass.PONumber();

        //            SerailNO = "EXP-" + AlreadyNumber;
        //        }

        //    }
        //    else
        //    {
        //        AlreadyNumber = POClass.PONumber();

        //        SerailNO = "EXP-" + AlreadyNumber;
        //    }

        //    return SerailNO;
        //}
    }
}