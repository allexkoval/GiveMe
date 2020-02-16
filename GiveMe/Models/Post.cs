﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveMe.Models
{
    public class Post
    {
        public int Id { get; set; }
        public String Title { get; set; } = "";
        public String Body { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
        
    }
}