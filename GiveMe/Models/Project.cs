using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveMe.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string Body { get; set; } = "";
        //public string Media { get; set; } = "";
        
        public bool Featured { get; set; }
                
        public DateTime Created { get; set; } = DateTime.Now;
        
        /*
        [Authorize(Roles = "developer")]
        public bool IsFeatured()
        {
            return false;
        }
        */
    }
}
