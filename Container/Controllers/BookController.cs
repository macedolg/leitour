using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using webleitour.Container.Models;
using NLog;
using System.Collections.Generic;

namespace webleitour.Container.Controllers
{
    public class BookController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly HttpClient httpClient;

        public BookController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5226/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> BookPage(string ISBN)
        {
            if (!string.IsNullOrEmpty(ISBN))
            {
                logger.Info($"ISBN received: {ISBN}");

                HttpResponseMessage response = await httpClient.GetAsync($"api/SearchBy/isbn/{ISBN}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Book book = JsonConvert.DeserializeObject<Book>(json);

                    HttpResponseMessage authorResponse = await httpClient.GetAsync($"api/SearchBy/author/{book.Authors}");

                    if (authorResponse.IsSuccessStatusCode)
                    {
                        string authorJson = await authorResponse.Content.ReadAsStringAsync();
                        List<Book> authorBooks = JsonConvert.DeserializeObject<List<Book>>(authorJson);

                        ViewBag.AuthorBooks = authorBooks;
                    }
                    else
                    {
                        logger.Error($"Failed to fetch data for author's books. Status code: {authorResponse.StatusCode}");
                    }

                    return View(book);
                }
                else
                {
                    logger.Error($"Failed to fetch data from the API. Status code: {response.StatusCode}");
                    return RedirectToAction("Error");
                }
            }
            else
            {
                logger.Error("ISBN parameter is missing or invalid.");
                return RedirectToAction("Error");
            }
        }

        public ActionResult SavedBooks() { return View(); }

        public async Task<ActionResult> Annotation()
        {
            return View();
        }
    }
}

