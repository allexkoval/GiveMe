using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveMe.ViewModels
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectBody { get; set; }
        public bool AdminRole { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string RoleName { get; set; }
    }
}
