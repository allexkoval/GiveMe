using GiveMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveMe.Data.Repository
{
    public interface IRepo
    {
        public Project GetPost(int id);
        List<Project> GetAllPosts();
        void AddPost(Project post);
        void UpdatePost(Project post);
        void RemovePost(int id);

        Task<bool> SaveChangesAsync();
    }
}
