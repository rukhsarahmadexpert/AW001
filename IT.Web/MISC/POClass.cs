using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.MISC
{
    public class POClass
    {
        public static string PONumber()
        {
            string Day = System.DateTime.Now.Day.ToString();
            string Month = System.DateTime.Now.Month.ToString();
            string YY = System.DateTime.Now.Year.ToString();


            if (Day.Length == 1)
            {
                Day = "0" + Day;
            }
            if (Month.Length == 1)
            {
                Month = "0" + Month;
            }

            YY = YY.Substring(2, 2);

            string PONumber = Day + Month + YY + "01";


            return PONumber;
        }
    }
}