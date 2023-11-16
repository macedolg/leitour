using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using Newtonsoft.Json;
using webleitour.Container.Models;
using System.Web;

namespace webleitour.Container.Controllers
{
    public class UserController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserModel userModel)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:5226/api/User/login";

                    var jsonContent = new
                    {
                        Id = 0,
                        NameUser = "string",
                        Email = userModel.Email,
                        Password = userModel.Password,
                        ProfilePhoto = "string",
                        Access = "string",
                        CreatedDate = "2023-10-26T23:42:33.281Z"
                    };

                    string jsonString = JsonConvert.SerializeObject(jsonContent);
                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseData);

                        string token = responseObject.token;
                        int userId = responseObject.user.id;

                        logger.Info($"Token saved in cookie: {token}");

                        var tokenCookie = new HttpCookie("AuthToken", token);
                        Response.Cookies.Add(tokenCookie);

                        var idCookie = new HttpCookie("UserID", userId.ToString());
                        Response.Cookies.Add(idCookie);

                        return RedirectToAction("Post", "Post");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao fazer a requisição à API.";
                        return View("Index", userModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao fazer a requisição à API.";
                return View("Index", userModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(UserModel user)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:5226/api/registrar";

                    user.ProfilePhoto = "https://wallpapers.com/images/hd/basic-default-pfp-pxi77qv5o0zuz8j3.jpg";
                    user.Access = "COMUM";
                    user.CreatedDate = DateTime.UtcNow;
                    var jsonString = JsonConvert.SerializeObject(user);
                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao fazer a requisição à API.";
                        return View("Registrar", user);
                    }
                }
            }
            catch (Exception ex)
            {
                // Configurando mensagem de erro genérico em caso de exceção
                ViewBag.ErrorMessage = "Erro ao fazer a requisição à API.";
                return View("Registrar", user);
            }
        }



    }
}
