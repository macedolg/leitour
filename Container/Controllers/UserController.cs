using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
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
                    // Configure o URL da API
                    string apiUrl = "https://localhost:7109/api/User/login";

                    // Crie um objeto JSON com base no modelo e nos dados da view
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

                    // Converte o objeto JSON em uma string
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonContent);

                    // Configura o conteúdo da solicitação
                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    // Faça a solicitação POST
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Leia a resposta JSON
                        var responseData = await response.Content.ReadAsStringAsync();

                        // Registre a resposta no log
                        logger.Info($"Resposta da API: {responseData}");

                        ViewBag.ResponseData = responseData;

                        return View("Index", userModel); // Redireciona de volta para a página inicial com os valores do modelo
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao fazer a requisição à API.";
                        return View("Index", userModel); // Redireciona de volta para a página inicial com os valores do modelo
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao fazer a requisição à API.";
                return View("Index", userModel); // Redireciona de volta para a página inicial com os valores do modelo
            }
        }
    }
}
