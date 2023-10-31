using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using Newtonsoft.Json;
using webleitour.Container.Models;

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
                    string apiUrl = "https://localhost:7109/api/User/login";

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
                        var responseObj = JsonConvert.DeserializeAnonymousType(responseData, new { user = new { nameUser = "" } });

                        string nameUser = responseObj.user.nameUser;

                        var user = JsonConvert.DeserializeObject<UserModel>(responseData);

                        logger.Info($"Resposta da API: {responseData}");
                        ViewBag.NameUser = nameUser;

                        return RedirectToAction("Post", "Post", new { nameUser = nameUser });
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
    }
}
