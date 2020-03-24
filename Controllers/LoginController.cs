using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Token_Based_Authentication_17_3.Models;

namespace Token_Based_Authentication_17_3.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string baseAddress = "http://localhost:63216";
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", model.username},
                   {"password", model.password},
               };
                var tokenResponse = client.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(form)).Result;
                //var token = tokenResponse.Content.ReadAsStringAsync().Result;  
                var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                if (string.IsNullOrEmpty(token.Error))
                {
                    Session["access_token"]= token.AccessToken;
                    Session["Role"] = token.Role;
                    Session["UserName"] = token.UserName;
                    if (Session["Role"].ToString() == "Manager")
                    {
                        return RedirectToAction("Index", "Equipment");
                    }
                    else
                    {
                        return RedirectToAction("Index", "EquipmentEmployee");
                    }
                }
                else
                {
                    ModelState.AddModelError("", token.Error);
                    return RedirectToAction("Index");
                }
               
            }
        }
       
        public ActionResult LogOut()
        {
            Session["access_token"] = null;
            Session["UserName"] = null;
            Session["Role"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "invalid model");
                return View(model);
            }
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                                Session["access_token"].ToString());
                //HTTP POST
                var posttask = client.PostAsJsonAsync<RegisterModel>("Account/Register",model);
                posttask.Wait();

                var result = posttask.Result;
                if(result.IsSuccessStatusCode)
                {
                    ViewData["success"] = "Create account successfully!";
                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create account!");
                    return View(model);
                }
            }
        }
    }
}