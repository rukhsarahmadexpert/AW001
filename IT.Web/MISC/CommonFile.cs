using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IT.Web.MISC
{
    public class CommonFile
    {
        public static bool DeleteFile(string rootFolder)
        {

            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(rootFolder)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(rootFolder));

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}