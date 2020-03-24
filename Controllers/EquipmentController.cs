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
    public class EquipmentController : Controller
    {
        
        // GET: Equipment
        public ActionResult Index()
        {
            if(Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            using (HttpClient client = new HttpClient())
            {
                IEnumerable<EquipmentViewModel> equipments = null;
                client.BaseAddress = new Uri("http://localhost:63216/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                var responseTask = client.GetAsync("api/Equipment");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EquipmentViewModel>>();
                    readTask.Wait();
                    equipments = readTask.Result;
                }
                else
                {
                    equipments = Enumerable.Empty<EquipmentViewModel>();
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                }
                return View(equipments);
            }       
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EquipmentViewModel model)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                //HTTP POST
                var postTask = client.PostAsJsonAsync<EquipmentViewModel>("Create", model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, result.StatusCode.ToString());

            }
            return View(model);
        }
    
        public EquipmentViewModel GetEquipment(string ID)
        {
           
            using (HttpClient client = new HttpClient())
            {
                EquipmentViewModel equipment = null;
                client.BaseAddress = new Uri("http://localhost:63216/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                var responseTask = client.GetAsync("api/Equipment?id="+ID+"");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EquipmentViewModel>();
                    readTask.Wait();
                    equipment = readTask.Result;
                }
                else
                {
                    equipment = null;
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                }
                return equipment;
            }
        }
        public ActionResult Edit(string ID)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            var eqp = GetEquipment(ID);
            return View(eqp);
        }
        [HttpPost]
        public ActionResult Edit(EquipmentViewModel model)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                //HTTP POST
                var postTask = client.PutAsJsonAsync<EquipmentViewModel>("Edit", model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, result.StatusCode.ToString());

            }
            return View(model);
        }
       
        public ActionResult Delete(string ID)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/Equipment?id="+ID+"");
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {                  
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
            }

            return RedirectToAction("Index");
        }
        public IList<EmployeeViewModel> GetAllEmployee()
        {         
            using (HttpClient client = new HttpClient())
            {
                IList<EmployeeViewModel> employees = null;
                client.BaseAddress = new Uri("http://localhost:63216/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                var responseTask = client.GetAsync("api/Employee");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EmployeeViewModel>>();
                    readTask.Wait();
                    employees = readTask.Result;
                }
                else
                {
                    employees = null;
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                }
                return employees;
            }
        }
        public ActionResult Assign(string ID)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            EquipEmployViewModel eevm = new EquipEmployViewModel();
            eevm.IDEquipment = ID;
            eevm.Emps = GetAllEmployee();
            return View(eevm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(string IDEquip,string UserName)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
               
                AssignModel model = new AssignModel();
                model.UserName = UserName;
                model.IDEquip = IDEquip;
                //HTTP PUT
                var postTask = client.PutAsJsonAsync<AssignModel>("Assign",model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                return RedirectToAction("Assign", new { ID = IDEquip });
            }        
        }

        public ActionResult EmployeeInfo()
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            List < EmployeeViewModel > listemp = GetAllEmployee().ToList();
            return View(listemp);
        }
  
        public ActionResult ShowAssignedEquip(string UserName)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            if (!ModelState.IsValid)
                return View();
            using(var client = new HttpClient())
            {
                IList<EquipmentViewModel> equipments = null;
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                var responseTask = client.GetAsync("ShowAssigned?UserName=" +UserName+"");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EquipmentViewModel>>();
                    equipments = readTask.Result;
                }
                else
                {
                    equipments = null;
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());            
                }
                EquipEmployViewModel eevm = new EquipEmployViewModel();
                eevm.Eqps = equipments;
                eevm.UserName = UserName;
                return View(eevm);
            }          
        }

    
        public ActionResult Unassign(string IDEquip,string username)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            if (!ModelState.IsValid)
                return RedirectToAction("EmployeeInfo");
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                var putTask = client.GetAsync("Unassign?IDEquip=" + IDEquip + "");
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("ShowAssignedEquip",new {UserName = username });
                else
                {
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                    return RedirectToAction("ShowAssignedEquip", new { UserName = username });
                }
            }
        }

        public IList<RequestViewModel> GetAllRequest()
        {
            using(var client = new HttpClient())
            {
                IList<RequestViewModel> requests = null;
                client.BaseAddress = new Uri("http://localhost:63216/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                var response = client.GetAsync("api/GetRequests");
                response.Wait();
                var result = response.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<IList<RequestViewModel>>();
                    requests = readtask.Result;
                    return requests;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                    return requests;
                }
            }
        }
        public ActionResult ShowRequests()
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            IList<RequestViewModel> requests = GetAllRequest();
            return View(requests);
        }
        public IList<EquipmentViewModel> GetAllEquipNonAssignByType(string Type)
        {
            using(var client = new HttpClient())
            {
                IList<EquipmentViewModel> equips = null;
                client.BaseAddress = new Uri("http://localhost:63216/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                var response = client.GetAsync("api/GetEquipsNonAssignByType?Type=" + Type + "");
                response.Wait();
                var result = response.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<IList<EquipmentViewModel>>();
                    equips = readtask.Result;
                    return equips;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "server error");
                    return equips;
                }
            }
        }
        public ActionResult ShowEquipNonAssignByType(string Type, string UserName, string IDRequest)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            IList<EquipmentViewModel> equips = GetAllEquipNonAssignByType(Type);
            TickRequestViewModel model = new TickRequestViewModel();
            model.listequip = equips;        
            model.UserName = UserName;
            model.IDRequest = IDRequest;
            return View(model);
        }

        public ActionResult TickRequest(string IDRequest,string IDEquipment,string UserName)
        {
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:63216/api/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["access_token"].ToString());
                
                //HTTP GET
                var response = client.GetAsync(string.Format("Tick?IDRequest={0}&IDEquipment={1}&UserName={2}", IDRequest, IDEquipment, UserName));
                
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("ShowRequests");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.StatusCode.ToString());
                    return RedirectToAction("ShowRequests");
                }
            }
        }
    }
}