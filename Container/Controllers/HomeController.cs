using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using webleitour.Container.Models;

namespace webleitour.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Authenticate(User loggingUser, string authCode)
        {
            bool isCredentialsValid = await ValidateCredentials(loggingUser.email, loggingUser.password, authCode);

            if (!isCredentialsValid)
            {
                // Credentials validation failed, you may want to display an error message or return to the login page.
                ModelState.AddModelError(string.Empty, "Invalid email, password, or auth code.");
                return View("Login");
            }

            using (HttpClient client = new HttpClient())
            {
                // Set the base address of your existing API.
                client.BaseAddress = new Uri("https://localhost:7109/api/User/login");

                // Define the data you want to send to your API for authentication.
                var data = new
                {
                    Email = loggingUser.email,
                    Password = loggingUser.password
                };

                // Serialize the data to JSON.
                string jsonData = JsonConvert.SerializeObject(data);

                // Make a POST request to your API for authentication.
                HttpResponseMessage response = await client.PostAsync("api/authenticate", new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    // Authentication succeeded in your API.
                    // Redirect to the "Post/Post" action.
                    return RedirectToAction("Post", "Post");
                }
                else
                {
                    // Authentication failed in your API.
                    // You may want to display an error message or return to the login page.
                    ModelState.AddModelError(string.Empty, "Authentication failed.");
                    return View("Login");
                }
            }
        }


        //    [HttpPost]
        //    public ActionResult RedirectIndex(User user)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (IsTokenValid(user))
        //            {
        //                bool rememberMe = user.RememberMe;
        //                if (rememberMe)
        //                {
        //                    SetAuthenticationCookie(user.Email);
        //                }
        //                return RedirectToAction("Post");
        //            }
        //            else
        //            {
        //                return RedirectToAction("IndexLogin");
        //            }
        //        }

        //        return View("Error");
        //    }

        //    private bool IsTokenValid(User user)
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                string apiUrl = "https://localhost:7109/api/User/login";

        //                var content = new FormUrlEncodedContent(new[]
        //                {
        //                    new KeyValuePair<string, string>("Email", user.Email),
        //                    new KeyValuePair<string, string>("Password", user.Password)
        //        });

        //                HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

        //                if (response.IsSuccessStatusCode)
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    ViewBag.ErrorMessage = "A resposta da API está vazia.";
        //                    return false;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    private void SetAuthenticationCookie(string email)
        //    {
        //        var ticket = new FormsAuthenticationTicket(
        //            1,
        //            email,
        //            DateTime.Now,
        //            DateTime.Now.AddYears(1),
        //            true, 
        //            string.Empty 
        //        );

        //        string encryptedTicket = FormsAuthentication.Encrypt(ticket);
        //        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
        //        {
        //            HttpOnly = true
        //        };

        //        Response.Cookies.Add(cookie);
        //    }
        //}

        private async Task<bool> ValidateCredentials(string email, string password, string authCode)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the base address of the external API.
                client.BaseAddress = new Uri("https://localhost:7109/api/User/login");

                // Prepare a model containing email, password, and authCode if required.
                var requestModel = new
                {
                    email,
                    password,
                    authCode
                };

                // Serialize the requestModel to JSON.
                string requestJson = JsonConvert.SerializeObject(requestModel);

                // Make a POST request to the external API to validate the email, password, and authCode.
                HttpResponseMessage response = await client.PostAsync("api/validation", new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return true; // Email, password, and authCode are valid.
                }
                else
                {
                    return false; // Validation failed.
                }
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }

}
    