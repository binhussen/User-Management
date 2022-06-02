using mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using mvc.Models;
using PagedList;

namespace mvc.Controllers
{
    public class UsersController : Controller
    {
        Uri baseUrl = new Uri("https://localhost:7258/");
        HttpClient client;

        public UsersController()
        {
            client = new HttpClient();
            client.BaseAddress = baseUrl;

        }
        public IActionResult Index(string search, int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            List<User> user = new List<User>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/User").Result;
            if (!String.IsNullOrEmpty(search))
            {
                response = client.GetAsync(client.BaseAddress + "api/User?FirstName=" + search).Result;
            }
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<List<User>>(data);
            }
            var headers = response.Headers;
            var pagination = "";
            if (headers.TryGetValues("X-pagination", out IEnumerable<string> headerValues))
            {
                pagination = headerValues.FirstOrDefault();
                JsonConvert.DeserializeObject<MetaData>(pagination);
            }
            return View(user.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8,"application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/User/", content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create",user);
        }
        public IActionResult Edit(int id)
        {
            User model = new User();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/User/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<User>(data);
            }
            return View("Create", model);
        }

        [HttpPut]
        public IActionResult Edit(User user)
        {
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "api/User/" + user.id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create", user);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            User model = new User();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/User/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<User>(data);
            }
            if(model != null)
            {
                HttpResponseMessage res = client.DeleteAsync(client.BaseAddress + "api/User/" + model.id).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}
