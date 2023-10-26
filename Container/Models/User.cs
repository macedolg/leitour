using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webleitour.Container.Models
{
    public class User
    {
        public int Id { get; set; }

        public string nameUser { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string profilePhoto { get; set; }

        public string access { get; set; }
    }
}