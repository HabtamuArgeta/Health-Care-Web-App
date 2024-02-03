using HealthCareApp.Helper;
using HealthCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace HealthCareApp.Controllers
{
    public class PatientViewModelController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44343/api");
        HttpClient client;

        public PatientViewModelController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            List<PatientViewModel> patientList = new List<PatientViewModel>();

            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Patients").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                patientList = JsonConvert.DeserializeObject<List<PatientViewModel>>(data);
            }

            return View(patientList);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(Login patient)
        {
            try
            {
                var json = JsonConvert.SerializeObject(patient);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(client.BaseAddress + "/Patients/Authenticate", content);

                if (response.IsSuccessStatusCode)
                {
                    new Roles("Patient", patient.UserName);
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
        public IActionResult Create(PatientViewModel patient)
        {
            try
            {
                HttpResponseMessage search = client.GetAsync(client.BaseAddress + $"/Patients/ByUserName/{patient.UserName}").Result;

                if (search.IsSuccessStatusCode)
                {
                    TempData["errorMassage"] = $"{patient.UserName} is already exist";
                    return View(patient);
                }
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(patient.UserName ?? "", Encoding.UTF8), "UserName");
                multiPartContent.Add(new StringContent(patient.Password ?? "", Encoding.UTF8), "Password");
                multiPartContent.Add(new StringContent(patient.Email ?? "", Encoding.UTF8), "Email");
                multiPartContent.Add(new StringContent(patient.Phone ?? "", Encoding.UTF8), "Phone");

                var imageContent = new StreamContent(patient.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", patient.Photo.FileName);

                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Patients", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Patient created successfully";
                    return RedirectToAction("Login");
                }
                return View(patient);
            }
            catch (Exception ex)

            {
                TempData["errorMassage"] = ex.Message;
                return View(patient);
            }
        }

        public IActionResult Edit(string id)
        {
            try
            {
                PatientViewModel patient = new PatientViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Patients/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    patient = JsonConvert.DeserializeObject<PatientViewModel>(data);

                }
                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }

        [HttpPost]
        public IActionResult Edit(PatientViewModel patient)
        {
            try
            {
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(patient.UserName ?? "", Encoding.UTF8), "UserName");
                multiPartContent.Add(new StringContent(patient.Password ?? "", Encoding.UTF8), "Password");
                multiPartContent.Add(new StringContent(patient.Email ?? "", Encoding.UTF8), "Email");
                multiPartContent.Add(new StringContent(patient.Phone ?? "", Encoding.UTF8), "Phone");

                var imageContent = new StreamContent(patient.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", patient.Photo.FileName);

                HttpResponseMessage response = client.PutAsync(client.BaseAddress + $"/Patients/{patient.Id}", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Patient Updated successfully";
                    return RedirectToAction("Index");
                }

                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(patient);
            }
        }


        [HttpGet]
       
        public IActionResult Details(string id)
        {
            try
            {
                PatientViewModel patient = new PatientViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Patients/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    patient = JsonConvert.DeserializeObject<PatientViewModel>(data);

                }
                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }
        public IActionResult Profile()
        {
            try
            {
                PatientViewModel patient = new PatientViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Patients/{Roles.UserName}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    patient = JsonConvert.DeserializeObject<PatientViewModel>(data);

                }
                return View(patient);
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
                PatientViewModel patient = new PatientViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Patients/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    patient = JsonConvert.DeserializeObject<PatientViewModel>(data);

                }
                return View(patient);
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
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + $"/Patients/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Student deleted successfully";
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
