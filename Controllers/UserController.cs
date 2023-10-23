using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using webleitour.Models;
using webleitour.Services;

namespace webleitour.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly MsgActionResultService _msgService;
        
        public UserController(IUserService userService, MsgActionResultService msgService)
        {
            _userService = userService;
            _msgService = msgService;
        }

        private string apiUrl = "https://localhost:7109/api/User";

[HttpGet]
[Route("User/GetUsers")]
public async Task<ActionResult> GetUsers()
{
    using (var client = new HttpClient())
    {
        HttpResponseMessage response = await client.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadAsAsync<IEnumerable<User>>();
            return View(users); 
        }
        else
        {
            // Handle the error response
            ModelState.AddModelError(string.Empty, "Failed to get users");
            return View(); 
        }
    }
}


    [HttpPost]
    public async Task<ActionResult> Authenticate(User loggingUser)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync("login", loggingUser);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<dynamic>();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errorMessage = _msgService.MsgActionResultService(); // Replace with your error message logic
                return Content((int)response.StatusCode, errorMessage);
            }
        }
        }
        
 [HttpGet]
    public async Task<ActionResult> GetUser(string email)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"api/User/{email}");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsAsync<User>();
                return View("UserView", user); // Assuming you have a UserView to display user details
            }
            else
            {
                var errorMessage = _msgService.MsgUserNotFound(); // Replace with your error message logic
                return Content((int)response.StatusCode, errorMessage);
            }
        }
    }

    [HttpPut]
    public async Task<ActionResult> PutUser([FromHeader] string token, [FromBody] User user)
    {
        int id = TokenService.DecodeToken(token); // Assuming you have a TokenService to decode the token

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PutAsJsonAsync($"api/User/alter", user);

            if (response.IsSuccessStatusCode)
            {
                var successMessage = await response.Content.ReadAsStringAsync();
                return Content((int)response.StatusCode, successMessage);
            }
            else
            {
                var errorMessage = _msgService.MsgUserNotFound(); // Replace with your error message logic
                return Content((int)response.StatusCode, errorMessage);
            }
        }
        }
        
        [HttpPost]
    public async Task<ActionResult> PostUser(User newUser)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync("api/User/register", newUser);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<dynamic>();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var errorMessage = _msgService.MsgAlreadyExists();
                return Content((int)response.StatusCode, errorMessage);
            }
            else
            {
                var errorMessage = _msgService.MsgUserNotFound();
                return Content((int)response.StatusCode, errorMessage);
            }
        }
    }

[HttpDelete]
    public async Task<ActionResult> DeactivateUser(string token)
    {
        int id = TokenService.DecodeToken(token); // Assuming you have a TokenService to decode the token

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.DeleteAsync("api/User/deactivate");

            if (response.IsSuccessStatusCode)
            {
                var successMessage = await response.Content.ReadAsStringAsync();
                return Content((int)response.StatusCode, successMessage);
            }
            else
            {
                var errorMessage = _msgService.MsgUserNotFound();
                return Content((int)response.StatusCode, errorMessage);
            }
        }
    }
    [HttpPost]
    [Route("User/Login")]
    public async Task<ActionResult> LoginAsync(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var authenticatedUser = await AuthenticateUserAsync(model.Email, model.Password);

            if (authenticatedUser != null)
            {
                return RedirectToAction("Post");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login credentials");
                return View("IndexLogin", model);
            }
        }

        return View("IndexLogin", model);
    }

    private async Task<LoginModel> AuthenticateUserAsync(string email, string password)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authenticationData = new { Email = email, Password = password };

            HttpResponseMessage response = await client.PostAsJsonAsync("User/Authenticate", authenticationData);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<LoginModel>();
            }
            else
            {
                return null;
            }
        }
    }
}
}
