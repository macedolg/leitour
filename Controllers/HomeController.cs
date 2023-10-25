using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using webleitour.Models;

namespace webleitour.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RedirectIndex(User user)
        {
            if (ModelState.IsValid)
            {
                if (IsTokenValid(user))
                {
                    bool rememberMe = user.RememberMe;
                    if (rememberMe)
                    {
                        SetAuthenticationCookie(user.Email);
                    }
                    return RedirectToAction("Post");
                }
                else
                {
                    return RedirectToAction("IndexLogin");
                }
            }

            return View("Error");
        }

        private bool IsTokenValid(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "https://localhost:7109/api/User/login";

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Email", user.Email),
                        new KeyValuePair<string, string>("Password", user.Password)
            });

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "A resposta da API está vazia.";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        private void SetAuthenticationCookie(string email)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                email,
                DateTime.Now,
                DateTime.Now.AddYears(1),
                true, 
                string.Empty 
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true
            };

            Response.Cookies.Add(cookie);
        }
    }

}