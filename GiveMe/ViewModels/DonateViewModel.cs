using GiveMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveMe.ViewModels
{
    public class DonateViewModel
    {
        public Project UserProject { get; set; }
        public int UserCash { get; set; }
    }
}
