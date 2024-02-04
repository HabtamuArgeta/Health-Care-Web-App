using HealthCareApp.Helper;
using HealthCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace HealthCareApp.Controllers
{
    public class AppointmentViewModelController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44343/api");
        HttpClient client;

        public AppointmentViewModelController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            List<AppointmentViewModel> appointmentList = new List<AppointmentViewModel>();

            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Appointments").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                appointmentList = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(data);
            }

            return View(appointmentList);
        }
       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AppointmentViewModel appointment)
        {
            try
            {
                HttpResponseMessage searchPatient = client.GetAsync(client.BaseAddress + $"/Patients/ByUserName/{appointment.PatientUserName}").Result;
                HttpResponseMessage searchDoctor = client.GetAsync(client.BaseAddress + $"/Doctors/ByUserName/{appointment.DoctorUserName}").Result;
                 if (!searchDoctor.IsSuccessStatusCode && !searchPatient.IsSuccessStatusCode)
                {
                    TempData["errorMassage"] = $"Doctor with user name  {appointment.DoctorUserName} and Patient with user name  {appointment.PatientUserName}  are not exist";
                    return View(appointment);
                }
               else  if (!searchPatient.IsSuccessStatusCode)
                {
                    TempData["errorMassage"] = $"Patient with user name  {appointment.PatientUserName} is not exist";
                    return View(appointment);
                }
                else if (!searchDoctor.IsSuccessStatusCode)
                {
                    TempData["errorMassage"] = $"Doctor with user name  {appointment.DoctorUserName} is not exist";
                    return View(appointment);
                }
               
                if (searchPatient.IsSuccessStatusCode && Roles.UserName != appointment.PatientUserName)
                {
                    TempData["errorMassage"] = $"You are not authorized to make appointment as {appointment.PatientUserName},Use your user name instead !";
                    return View(appointment);
                }
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(appointment.DoctorUserName ?? "", Encoding.UTF8), "DoctorUserName");
                multiPartContent.Add(new StringContent(appointment.PatientUserName ?? "", Encoding.UTF8), "PatientUserName");
                multiPartContent.Add(new StringContent(appointment.Date ?? "", Encoding.UTF8), "Date");
               

                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Appointments", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Appointment created successfully";
                    return RedirectToAction("Index");
                }
                return View(appointment);
            }
            catch (Exception ex)

            {
                TempData["errorMassage"] = ex.Message;
                return View(appointment);
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
