using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using UnifiedApiConnect.Helpers;
using UnifiedApiConnect.Models;

namespace UnifiedApiConnect.Controllers
{
    public class OneDriveController : Controller
    {
        // GET: OneDrive
        public ActionResult Index()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = new AuthenticationHeaderValue("Bearer", (string)Session[SessionKeys.Login.AccessToken]).ToString();
                string jsonResul = wc.DownloadString("https://graph.microsoft.com/v1.0/me/drive/items/01TN5TMT2DJFLQXWIAKFBIGDWTTCAXOFU7/children");

                dynamic data = System.Web.Helpers.Json.Decode(jsonResul);
                
                foreach (dynamic file_info in data.value)
                {
                    result.Add(file_info.name, file_info.id);
                }
            }

            return View(result);
        }

        public FileContentResult Download(string id, string fileName) =>
            File(UnifiedApiHelper.GetItem(id, (string)Session[SessionKeys.Login.AccessToken]), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

    }
}