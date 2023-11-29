using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using Newtonsoft.Json;
using webleitour.Container.Models;
using System.Web;

namespace webleitour.Controllers
{
    public class UserController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Teste() { return View(); }

        public ActionResult Index()
        {
            return View(new UserModel());
        }

        public ActionResult Registrar()
        {
            return View(new RegisterUserModel());
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
        public async Task<ActionResult> Register(RegisterUserModel model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = "https://localhost:5226/api/User/register";

                    var jsonContent = new
                    {
                        Id = model.Id,
                        NameUser = model.NameUser,
                        Email = model.Email,
                        Password = model.Password,
                        ProfilePhoto = model.ProfilePhoto,
                        Access = model.Access,
                        CreatedDate = model.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    };

                    string jsonString = JsonConvert.SerializeObject(jsonContent);
                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

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
                        return View("Registrar", model);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle 
                return View("Error", new HandleErrorInfo(ex, "User", "Register"));
            }

        }

        public async Task<ActionResult> Perfil(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"https://localhost:5226/api/User/{id}";

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<UserGeralModel>(responseData);

                        return View(user);
                    }
                    else
                    {
                        return View("Index", new UserGeralModel());
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Index", new UserGeralModel());
            }
        }

    }
}