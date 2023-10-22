using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webleitour.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MessagePost { get; set; }
        public int Likes { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime? AlteratedDate { get; set; }
    }
}