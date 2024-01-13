﻿using HealthCareApp.Models;
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
                    return RedirectToAction("DoctorDashboard");
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

        public IActionResult DoctorDashboard()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AdminViewModel admin)
        {
            try
            {

                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(admin.UserName ?? "", Encoding.UTF8), "UserName");
                multiPartContent.Add(new StringContent(admin.Password ?? "", Encoding.UTF8), "Password");
                multiPartContent.Add(new StringContent(admin.Address ?? "", Encoding.UTF8), "Address");
                multiPartContent.Add(new StringContent(admin.DateOfBirth ?? "", Encoding.UTF8), "DateOfBirth");
                multiPartContent.Add(new StringContent(admin.Email ?? "", Encoding.UTF8), "Email");
                multiPartContent.Add(new StringContent(admin.Gender ?? "", Encoding.UTF8), "Gender");
                multiPartContent.Add(new StringContent(admin.Phone ?? "", Encoding.UTF8), "Phone");

                var imageContent = new StreamContent(admin.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", admin.Photo.FileName);

                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Admins", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Admin created successfully";
                    return RedirectToAction("Index");
                }
                return View(admin);
            }
            catch (Exception ex)

            {
                TempData["errorMassage"] = ex.Message;
                return View(admin);
            }
        }

        public IActionResult Edit(string id)
        {
            try
            {
                AdminViewModel admin = new AdminViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Admins/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    admin = JsonConvert.DeserializeObject<AdminViewModel>(data);

                }
                return View(admin);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }

        [HttpPost]
        public IActionResult Edit(AdminViewModel admin)
        {
            try
            {
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(admin.UserName ?? "", Encoding.UTF8), "UserName");
                multiPartContent.Add(new StringContent(admin.Password ?? "", Encoding.UTF8), "Password");
                multiPartContent.Add(new StringContent(admin.Address ?? "", Encoding.UTF8), "Address");
                multiPartContent.Add(new StringContent(admin.DateOfBirth ?? "", Encoding.UTF8), "DateOfBirth");
                multiPartContent.Add(new StringContent(admin.Email ?? "", Encoding.UTF8), "Email");
                multiPartContent.Add(new StringContent(admin.Gender ?? "", Encoding.UTF8), "Gender");
                multiPartContent.Add(new StringContent(admin.Phone ?? "", Encoding.UTF8), "Phone");

                var imageContent = new StreamContent(admin.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", admin.Photo.FileName);

                HttpResponseMessage response = client.PutAsync(client.BaseAddress + $"/Admins/{admin.Id}", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Admin Updated successfully";
                    return RedirectToAction("Index");
                }

                return View(admin);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(admin);
            }
        }


        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                AdminViewModel admin = new AdminViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Admins/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    admin = JsonConvert.DeserializeObject<AdminViewModel>(data);

                }
                return View(admin);
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
                AdminViewModel admin = new AdminViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Admins/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    admin = JsonConvert.DeserializeObject<AdminViewModel>(data);

                }
                return View(admin);
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
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + $"/Admins/{id}").Result;

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
