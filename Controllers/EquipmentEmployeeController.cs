using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Token_Based_Authentication_17_3.Models;

namespace Token_Based_Authentication_17_3.Controllers
{
    public class EquipmentEmployeeController : Controller
    {
        // GET: EquipmentEmployee
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult CreateRequest()
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                                Session["access_token"].ToString());
                //HTTP GET
                var response = client.GetAsync("Request/GetListOfTypeEquipment");
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<string>>();
                    readTask.Wait();
                    IList<string> types = readTask.Result;
                    TypeRequestViewModel trvm = new TypeRequestViewModel();
                    trvm.Types = types;
                    return View(trvm);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                    return View();
                }
            }
           
        }
        [HttpPost]
        public ActionResult CreateRequest(TypeRequestViewModel model)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            model.Requests.UserName = Session["UserName"].ToString();         
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                                Session["access_token"].ToString());
                //HTTP POST
                var postTask = client.PostAsJsonAsync<RequestViewModel>("Request/Post",model.Requests);

                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                    return View(model);
                }
            }
        }
    }
}