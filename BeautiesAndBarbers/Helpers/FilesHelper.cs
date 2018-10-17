using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Helpers
{
    public class FilesHelper
    {
        public static bool UploadImage(HttpPostedFileBase file, string folder, string name)
        {
            if (file == null || string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(name))
            {
                return false;
            }

            try
            {
                string path = string.Empty;

                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);
                file.SaveAs(path);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}