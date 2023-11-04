using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webleitour.Container.Models
{
    public class UserGeralModel
    {
        public int Id { get; set; }
        public string NameUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePhoto { get; set; }
        public string Access { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}