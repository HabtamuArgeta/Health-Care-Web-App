using HealthCareApp.Helper;
using HealthCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace HealthCareApp.Controllers
{
    public class PostViewModelController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44343/api");
        HttpClient client;

        public PostViewModelController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            List<PostViewModel> Posts = new List<PostViewModel>();

            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Posts").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Posts = JsonConvert.DeserializeObject<List<PostViewModel>>(data);
            }

            return View(Posts);
        }
     
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PostViewModel posts)
        {
            try
            {
                HttpResponseMessage search = client.GetAsync(client.BaseAddress + $"/Doctors/ByUserName/{posts.DoctorUserName}").Result;

                if (!search.IsSuccessStatusCode)
                {
                    TempData["errorMassage"] = $"Doctor with user name  {posts.DoctorUserName} is not exist";
                    return View(posts);
                }
                if (search.IsSuccessStatusCode && Roles.UserName != posts.DoctorUserName)
                {
                    TempData["errorMassage"] = $"You are not authorized to post as {posts.DoctorUserName},Use your user name instead !";
                    return View(posts);
                }
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(posts.DoctorUserName ?? "", Encoding.UTF8), "DoctorUserName");
                multiPartContent.Add(new StringContent(posts.DatePublished ?? "", Encoding.UTF8), "DatePublished");
                multiPartContent.Add(new StringContent(posts.Description ?? "", Encoding.UTF8), "Description");

                var imageContent = new StreamContent(posts.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", posts.Photo.FileName);

                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Posts", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "post created successfully";
                    return RedirectToAction("Index", "Home");
                }
                return View(posts);
            }
            catch (Exception ex)

            {
                TempData["errorMassage"] = ex.Message;
                return View(posts);
            }
        }

        public IActionResult Edit(string id)
        {
            try
            {
                PostViewModel posts = new PostViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Posts/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    posts = JsonConvert.DeserializeObject<PostViewModel>(data);

                }
                return View(posts);
            }
            catch (Exception ex)
            {
                TempData["errorMassage"] = ex.Message;
                return View();

            }

        }

        [HttpPost]
        public IActionResult Edit(PostViewModel posts)
        {
            try
            {
                using MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(new StringContent(posts.DoctorUserName ?? "", Encoding.UTF8), "DoctorUserName");
                multiPartContent.Add(new StringContent(posts.DatePublished ?? "", Encoding.UTF8), "DatePublished");
                multiPartContent.Add(new StringContent(posts.Description ?? "", Encoding.UTF8), "Description");

                var imageContent = new StreamContent(posts.Photo.OpenReadStream());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                multiPartContent.Add(imageContent, "Photo", posts.Photo.FileName);

                HttpResponseMessage response = client.PutAsync(client.BaseAddress + $"/Posts/{posts.Id}", multiPartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Posts Updated successfully";
                    return RedirectToAction("Index");
                }

                return View(posts);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(posts);
            }
        }


        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                PostViewModel posts = new PostViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Posts/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    posts = JsonConvert.DeserializeObject<PostViewModel>(data);

                }
                return View(posts);
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
                PostViewModel posts = new PostViewModel();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Posts/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    posts = JsonConvert.DeserializeObject<PostViewModel>(data);

                }
                return View(posts);
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
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + $"/Posts/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Post deleted successfully";
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
