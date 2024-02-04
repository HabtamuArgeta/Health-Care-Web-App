using HealthCareApp.Helper;
using HealthCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace HealthCareApp.Controllers
{
    public class DocterViewModelController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44343/api");
        HttpClient client;

        public DocterViewModelController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            List<DocterViewModel> doctorList = new List<DocterViewModel>();

            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Doctors").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                doctorList = JsonConvert.DeserializeObject<List<DocterViewModel>>(data);
            }

            return View(doctorList);
         }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(Login doctor)
        {
            try
            {
                var json = JsonConvert.SerializeObject(doctor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(client.BaseAddress + "/Doctors/Authenticate", content);

                if (response.IsSuccessStatusCode)
                {
                    new Roles("Doctor", doctor.UserName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["errorMassage"] = "Invalid username or password";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();
            }
        }

       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DocterViewModel doctor)
        {
            try
            {
               
                HttpResponseMessage search = client.GetAsync(client.BaseAddress + $"/Doctors/ByUserName/{doctor.UserName}").Result;

                if (search.IsSuccessStatusCode)
                {
                    TempData["errorMassage"] = $"{doctor.UserName} is already exist";
                    return View(doctor);
                }
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(doctor.UserName ?? "", Encoding.UTF8), "UserName");
                multiPartContent.Add(new StringContent(doctor.Password ?? "", Encoding.UTF8), "Password");
                multiPartContent.Add(new StringContent(doctor.Speciality?? "", Encoding.UTF8), "Speciality");
                multiPartContent.Add(new StringContent(doctor.Email ?? "", Encoding.UTF8), "Email");
                multiPartContent.Add(new StringContent(doctor.Gender ?? "", Encoding.UTF8), "Gender");
                multiPartContent.Add(new StringContent(doctor.Phone ?? "", Encoding.UTF8), "Phone");

                var imageContent = new StreamContent(doctor.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", doctor.Photo.FileName);

                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Doctors", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Doctor created successfully";
                    return RedirectToAction("Login");
                }
                return View(doctor);
            }
            catch (Exception ex)

            {
                TempData["errorMassage"] = ex.Message;
                return View(doctor);
            }
        }

        public IActionResult Edit(string id)
        {
            try
            {
                DocterViewModel doctor = new DocterViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Doctors/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    doctor = JsonConvert.DeserializeObject<DocterViewModel>(data);

                }
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }

        [HttpPost]
        public IActionResult Edit(DocterViewModel doctor)
        {
            try
            {
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(doctor.UserName ?? "", Encoding.UTF8), "UserName");
                multiPartContent.Add(new StringContent(doctor.Password ?? "", Encoding.UTF8), "Password");
                multiPartContent.Add(new StringContent(doctor.Speciality ?? "", Encoding.UTF8), "Speciality");
                multiPartContent.Add(new StringContent(doctor.Email ?? "", Encoding.UTF8), "Email");
                multiPartContent.Add(new StringContent(doctor.Gender ?? "", Encoding.UTF8), "Gender");
                multiPartContent.Add(new StringContent(doctor.Phone ?? "", Encoding.UTF8), "Phone");

                var imageContent = new StreamContent(doctor.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", doctor.Photo.FileName);

                HttpResponseMessage response = client.PutAsync(client.BaseAddress + $"/Doctors/{doctor.Id}", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Admin Updated successfully";
                    return RedirectToAction("Index");
                }

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(doctor);
            }
        }


        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                DocterViewModel doctor = new DocterViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Doctors/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    doctor = JsonConvert.DeserializeObject<DocterViewModel>(data);

                }
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }
        [HttpGet]
        public IActionResult Profile()
        {
            try
            {
                DocterViewModel doctor = new DocterViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Doctors/{Roles.UserName}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    doctor = JsonConvert.DeserializeObject<DocterViewModel>(data);

                }
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            try
            {
                DocterViewModel doctor = new DocterViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Doctors/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    doctor = JsonConvert.DeserializeObject<DocterViewModel>(data);

                }
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }


        [HttpPost]
        public IActionResult DeleteConfirmed(string id)
        {
            try
            {

                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + $"/Doctors/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Doctor deleted successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
    }
}
