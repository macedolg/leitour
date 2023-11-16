using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webleitour.Container.Models
{
    public class Book
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public string ISBN_10 { get; set; }
        public string ISBN_13 { get; set; }
        public string Language { get; set; }
        public string Cover { get; set; }
    }
}