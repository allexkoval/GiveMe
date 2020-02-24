using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [DataType(DataType.ImageUrl)]
        [Display(Name ="Poster")]
        public string ImageUrl { get; set; }

        [Display(Name ="Image File")]
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public string ImageStorageName { get; set; }
    }
}
