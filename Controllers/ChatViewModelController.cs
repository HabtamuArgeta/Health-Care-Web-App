using HealthCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HealthCareApp.Controllers
{
    public class ChatViewModelController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44343/api");
        HttpClient client;

        public ChatViewModelController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            List<ChatViewModel> chatList= new List<ChatViewModel>();

            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Chats").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                chatList = JsonConvert.DeserializeObject<List<ChatViewModel>>(data);
            }

            return View(chatList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ChatViewModel chats)
        {
            try
            {

                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(chats.DoctorUserName ?? "", Encoding.UTF8), "DoctorUserName");
                multiPartContent.Add(new StringContent(chats.PatientUserName ?? "", Encoding.UTF8), "PatientUserName");
                multiPartContent.Add(new StringContent(chats.CreatedAt ?? "", Encoding.UTF8), "CreatedAt");
                multiPartContent.Add(new StringContent(chats.Message ?? "", Encoding.UTF8), "Message");

                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Chats", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Chats created successfully";
                    return RedirectToAction("Index");
                }
                return View(chats);
            }
            catch (Exception ex)

            {
                TempData["errorMassage"] = ex.Message;
                return View(chats);
            }
        }

        public IActionResult Edit(string id)
        {
            try
            {
                AppointmentViewModel appointment = new AppointmentViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Appointments/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(data);

                }
                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }

        [HttpPost]
        public IActionResult Edit(AppointmentViewModel appointment)
        {
            try
            {
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(appointment.DoctorUserName ?? "", Encoding.UTF8), "DoctorUserName");
                multiPartContent.Add(new StringContent(appointment.PatientUserName ?? "", Encoding.UTF8), "PatientUserName");
                multiPartContent.Add(new StringContent(appointment.Date ?? "", Encoding.UTF8), "Date");

                HttpResponseMessage response = client.PutAsync(client.BaseAddress + $"/Appointments/{appointment.Id}", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Appointment Updated successfully";
                    return RedirectToAction("Index");
                }

                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(appointment);
            }
        }


        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                AppointmentViewModel appointment = new AppointmentViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Appointments/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(data);

                }
                return View(appointment);
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
                AppointmentViewModel appointment = new AppointmentViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Appointments/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(data);

                }
                return View(appointment);
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
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + $"/Appointments/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Appointments deleted successfully";
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
