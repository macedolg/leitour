using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webleitour.Container.Models
{
    public class CreatePost
    {
        public int UserId { get; set; }
        public string MessagePost { get; set; }
        public DateTime PostDate { get; set; } = DateTime.UtcNow;
        public DateTime AlteratedDate { get; set; }
    }
}