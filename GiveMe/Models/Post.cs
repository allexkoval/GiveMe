﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveMe.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string Body { get; set; } = "";
        //public string Media { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
        
    }
}
