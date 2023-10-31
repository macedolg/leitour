﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using webleitour.Container.Models;

namespace webleitour.Controllers
{
    public class PostController : Controller
    {
        public async Task<ActionResult> Post(string nameUser)
        {
            var apiUrl = "https://localhost:7109/api/Posts";
            var id = ViewBag.Id as int?;

            List<Post> publicacoes = new List<Post>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                    {
                        publicacoes = JsonConvert.DeserializeObject<List<Post>>(content).OrderByDescending(post => post.PostDate).ToList();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "A resposta da API está vazia.";
                        return View("Error");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            ViewBag.NameUser = nameUser;

            return View(publicacoes);
        }
    }
}
