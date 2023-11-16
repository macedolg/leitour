using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using webleitour.Container.Models;
using System.Text;

namespace webleitour.Controllers
{
    public class PostController : Controller
    {
        private readonly string apiUrl = "https://localhost:5226/api/posts";

        [HttpPost]
        public async Task<ActionResult> CreatePost(string feedPost)
        {
            int userId;
            if (Request.Cookies["UserID"] != null && int.TryParse(Request.Cookies["UserID"].Value, out userId))
            {

                var newPost = new
                {
                    id = 0,
                    userId = userId,
                    messagePost = feedPost,
                    postDate = DateTime.UtcNow,
                    alteratedDate = DateTime.UtcNow
                };

                using (HttpClient client = new HttpClient())
                {
                    if (Request.Cookies["AuthToken"] != null)
                    {
                        string token = Request.Cookies["AuthToken"].Value;
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    var postContent = new StringContent(JsonConvert.SerializeObject(newPost), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, postContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Post");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao criar o post. Tente novamente.";
                        return RedirectToAction("Post");
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "O ID do usuário não está presente no cookie.";
                return RedirectToAction("Index", "User");
            }
        }
        public async Task<ActionResult> Post()
        {
            int userId;
            if (Request.Cookies["UserID"] != null && int.TryParse(Request.Cookies["UserID"].Value, out userId))
            {
                var apiUrlUser = $"https://localhost:5226/api/User/{userId}";
                var apiUrlPosts = "https://localhost:5226/api/Posts";

                UserModel user = new UserModel();
                List<Post> publicacoes = new List<Post>();

                using (HttpClient client = new HttpClient())
                {
                    if (Request.Cookies["AuthToken"] != null)
                    {
                        string token = Request.Cookies["AuthToken"].Value;
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    HttpResponseMessage responseUser = await client.GetAsync(apiUrlUser);
                    HttpResponseMessage responsePosts = await client.GetAsync(apiUrlPosts);

                    if (responseUser.IsSuccessStatusCode && responsePosts.IsSuccessStatusCode)
                    {
                        string contentUser = await responseUser.Content.ReadAsStringAsync();
                        string contentPosts = await responsePosts.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(contentUser) && !string.IsNullOrEmpty(contentPosts))
                        {
                            user = JsonConvert.DeserializeObject<UserModel>(contentUser);
                            publicacoes = JsonConvert.DeserializeObject<List<Post>>(contentPosts).OrderByDescending(post => post.PostDate).ToList();
                            return View(Tuple.Create(publicacoes, user));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "A resposta da API está vazia.";
                            return RedirectToAction("Index", "User");
                        }
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "O ID do usuário não está presente no cookie.";
                return RedirectToAction("Index", "User");
            }
        }

        public async Task<ActionResult> SearchResult(string query)
        {
            int userId;
            if (Request.Cookies["UserID"] != null && int.TryParse(Request.Cookies["UserID"].Value, out userId))
            {
                var apiUrlSearch = $"https://localhost:5226/api/SearchBy/search/{query}"; // URL da sua API de busca

                UserModel user = new UserModel();
                List<Post> searchResults = new List<Post>();

                using (HttpClient client = new HttpClient())
                {
                    if (Request.Cookies["AuthToken"] != null)
                    {
                        string token = Request.Cookies["AuthToken"].Value;
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    HttpResponseMessage responseSearch = await client.GetAsync(apiUrlSearch);

                    if (responseSearch.IsSuccessStatusCode)
                    {
                        string contentSearch = await responseSearch.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(contentSearch))
                        {
                            searchResults = JsonConvert.DeserializeObject<List<Post>>(contentSearch).OrderByDescending(post => post.PostDate).ToList();
                            return View(searchResults);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "A resposta da API de busca está vazia.";
                            return View("Error");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao buscar na API.";
                        return View("Error");
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "O ID do usuário não está presente no cookie.";
                return RedirectToAction("Index", "User");
            }
        }

    }
}
