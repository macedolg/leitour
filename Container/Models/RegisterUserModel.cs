using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webleitour.Container.Models
{
    public class RegisterUserModel
    {
        public int Id { get; set; }

        [Required]
        [JsonProperty("nameUser")]
        public string NameUser { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ProfilePhoto { get; set; }

        public string Access { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}